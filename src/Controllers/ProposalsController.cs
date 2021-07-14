using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Document;
using DasTeamRevolution.Models.Dto.Proposal;
using DasTeamRevolution.Models.Enums;
using DasTeamRevolution.Services.AuthTokenService;
using IronSphere.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Controllers
{
    /// <summary>
    /// <see cref="Proposal"/> controller for posting employee proposals and uploading additional attachments to them.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProposalsController : ApiBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthTokenService _authTokenService;
        private readonly IMapper _mapper;

        public ProposalsController(ApplicationDbContext db, IAuthTokenService authTokenService, IMapper mapper)
        {
            _db = db;
            _authTokenService = authTokenService;
            _mapper = mapper;
        }

        /// <summary>
        /// Propose an employee.<para> </para>
        /// IMPORTANT:
        /// This query will fail if the provided vacancy and/or employee
        /// is not part of what the requesting user has access permission to access!
        /// (e.g. clients may not modify suppliers' data sets!).
        /// </summary>
        /// <param name="dto">Request body DTO.</param>
        [HttpPost, Route("")]
        public async Task<IActionResult> PostProposal([FromBody] ProposalCreationRequestDto dto)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            SupplierUser user = await _db.SupplierUsers
                .Include(u => u.ApplicationUser)
                .Where(u => u.ApplicationUserId == userId && u.ApplicationUser.IsActive)
                .FirstOrDefaultAsync();

            if (user is null)
                return Forbidden();

            IQueryable<Vacancy> vacancyQuery =
                from supplierUser in _db.SupplierUsers
                where supplierUser.ApplicationUserId == user.ApplicationUserId
                from supplierUserSetting in _db.SupplierUserPermissions
                where supplierUserSetting.SupplierUserId == supplierUser.Id
                from supplier in _db.Suppliers
                where supplier.Id == supplierUserSetting.SupplierId
                from clientSupplier in _db.ClientSuppliers
                where clientSupplier.SupplierId == supplier.Id
                from client in _db.Clients
                where client.Id == clientSupplier.ClientId
                from _vacancy in _db.Vacancies
                    .Include(v => v.PostalAddress)
                    .Include(v => v.Creation)
                    .Include(v => v.LastModification)
                    .Include(v => v.Proposals)
                where _vacancy.Id == dto.VacancyId && _vacancy.ClientId == client.Id
                select _vacancy;

            Vacancy vacancy = await vacancyQuery.FirstOrDefaultAsync();

            if (vacancy is null)
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<Vacancy>(dto.VacancyId));

            IQueryable<PoolEmployee> poolEmployeeQuery =
                from supplierUser in _db.SupplierUsers
                where supplierUser.ApplicationUserId == userId
                from supplierUserSetting in _db.SupplierUserPermissions
                where supplierUserSetting.SupplierUserId == supplierUser.Id
                from supplier in _db.Suppliers
                where supplier.Id == supplierUserSetting.SupplierId
                from _poolEmployee in _db.PoolEmployees
                where _poolEmployee.SupplierId == supplier.Id && _poolEmployee.Id == dto.PoolEmployeeId
                select _poolEmployee;

            PoolEmployee poolEmployee = await poolEmployeeQuery.FirstOrDefaultAsync();

            if (poolEmployee is null)
                return Error(HttpStatusCode.NotFound,
                    Constants.Errors.ResourceNotFound<PoolEmployee>(dto.PoolEmployeeId));

            Proposal proposal = new()
            {
                Comment = dto.Comment,
                VacancyId = vacancy.Id,
                SupplierUserId = user.Id,
                PoolEmployeeDocuments = await _db.PoolEmployeeDocuments.Where(d =>
                    d.PoolEmployeeId == poolEmployee.Id && dto.PoolEmployeeDocumentIds.Contains(d.Id)).ToListAsync()
            };

            Employee employee = await _db.Employees
                .Where(e => e.ClientId == vacancy.ClientId && e.PoolEmployeeId == poolEmployee.Id)
                .FirstOrDefaultAsync();

            bool success;

            if (employee is null)
            {
                employee = new Employee
                {
                    PoolEmployeeId = poolEmployee.Id,
                    FirstName = poolEmployee.FirstName,
                    LastName = poolEmployee.LastName,
                    Email = poolEmployee.Email,
                    Phone = poolEmployee.Phone,
                    Mobile = poolEmployee.Mobile,
                    ClientId = vacancy.ClientId,
                    DateOfBirth = poolEmployee.DateOfBirth,
                    Creation = new RecordCreation {CreatedById = userId, CreatedOn = DateTime.UtcNow}
                };

                await _db.Employees.AddAsync(employee);
                success = await _db.SaveChangesAsync() > 0;

                if (!success)
                    return Error(HttpStatusCode.InternalServerError, Constants.Errors.ProposalCreationFailed);
            }

            proposal.EmployeeId = employee.Id;

            await _db.Proposals.AddAsync(proposal);
            success = await _db.SaveChangesAsync() > 0;

            if (!success)
                return Error(HttpStatusCode.InternalServerError, Constants.Errors.ProposalCreationFailed);

            bool uploadedAdditionalDocuments = false;

            foreach (DocumentUploadDto file in dto.ProposalDocuments)
            {
                // await using MemoryStream ms = new();
                // await file.CopyToAsync(ms);

                ProposalDocument doc = new()
                {
                    Proposal = proposal,
                    ProposalId = proposal.Id,
                    DocumentName = file.FileName,
                    DocumentBytes = file.Content,
                    Creation = new RecordCreation
                    {
                        CreatedById = userId,
                        CreatedOn = DateTime.UtcNow
                    }
                };

                await _db.ProposalDocuments.AddAsync(doc);
                uploadedAdditionalDocuments = true;
            }

            if (uploadedAdditionalDocuments)
            {
                success = await _db.SaveChangesAsync() > 0;

                if (!success)
                    return Error(HttpStatusCode.InternalServerError, Constants.Errors.ProposalDocumentUploadFailed);
            }

            await _db.ProposalStates.AddAsync(new ProposalStateHistory
            {
                ProposalId = proposal.Id,
                StateType = ProposalStateType.Created,
                Creation = new RecordCreation
                {
                    CreatedById = userId,
                    CreatedOn = DateTime.UtcNow
                }
            });

            success = await _db.SaveChangesAsync() > 0;

            if (!success)
                return Error(HttpStatusCode.InternalServerError, Constants.Errors.ProposalDocumentUploadFailed);

            return Created($"api/v1/proposals/{proposal.Id}", 1,
                new[] {_mapper.Map<ProposalCreationResponseDto>(proposal)});
        }

        [HttpGet, Route("get-proposals-of-client")]
        public async Task<IActionResult> GetProposalsOfClient()
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            long clientUserId = await _db.ClientUsers
                .Where(c => c.ApplicationUserId == userId)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();

            if (clientUserId == default)
                return Forbidden();

            IQueryable<Proposal> proposalsQuery =
                from clientUserSetting in _db.ClientUserPermissions
                where clientUserSetting.ClientUserId == clientUserId
                from client in _db.Clients
                where client.Id == clientUserSetting.ClientId
                from vacancy in _db.Vacancies
                where vacancy.ClientId == client.Id
                from proposal in _db.Proposals
                    .Include(x => x.Employee)
                    .ThenInclude(x => x.PostalAddress)
                    .Include(p => p.Employee)
                    .ThenInclude(c => c.PoolEmployee)
                    .ThenInclude(c => c.Supplier)
                    .ThenInclude(c => c.Header)
                    .Include(p => p.States)
                where proposal.VacancyId == vacancy.Id
                select proposal;

            return EnvelopeResult.Ok(_mapper.Map<IEnumerable<ProposalResponseDto>>(await proposalsQuery.ToListAsync()));
        }

        [HttpGet, Authorize, Route("")]
        public async Task<IActionResult> GetProposalsOfSupplier(long? clientId = null, long? vacancyId = null)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            long? supplierUserId = (await _db.SupplierUsers
                .Include(u => u.ApplicationUser)
                .Where(u => u.ApplicationUserId == userId && u.ApplicationUser.IsActive)
                .FirstOrDefaultAsync())?.Id;

            if (supplierUserId is null)
                return Forbidden();

            IQueryable<Proposal> allProposals = _db.Proposals
                .Include(x => x.Employee)
                .ThenInclude(c => c.PoolEmployee)
                .ThenInclude(c => c.Supplier)
                .ThenInclude(c => c.Header)
                .Include(x => x.States)
                .Include(x => x.Vacancy)
                .Where(w => w.SupplierUserId == supplierUserId);

            IQueryable<Proposal> result;
            if (vacancyId.HasValue)
                result = allProposals.Where(w => w.VacancyId == vacancyId);
            else if (clientId.HasValue)
                result = allProposals.Where(w => w.Vacancy.ClientId == clientId);
            else
                result = allProposals;

            return EnvelopeResult.Ok(
                (IEnumerable<ProposalResponseDto>) _mapper.Map<List<ProposalResponseDto>>(await result.ToListAsync()));
        }

        [HttpGet, Authorize, Route("get-proposals-of-vacancy/{id}")]
        public async Task<IActionResult> GetProposalsOfVacancy(long id)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            List<Proposal> result = await _db.Proposals
                .Include(x => x.Employee)
                .ThenInclude(x => x.PostalAddress)
                .Include(x => x.Employee)
                .ThenInclude(c => c.PoolEmployee)
                .ThenInclude(c => c.Supplier)
                .ThenInclude(c => c.Header)
                .Include(x => x.States)
                .Where(w => w.VacancyId == id)
                .ToListAsync();
            
            return EnvelopeResult.Ok((IEnumerable<ProposalResponseDto>) _mapper.Map<List<ProposalResponseDto>>(result));
        }

        [HttpGet, Authorize, Route("get-proposals-of-employee/{id}")]
        public async Task<IActionResult> GetProposalsOfEmployee(long id)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers
                .Include(u => u.ApplicationUser)
                .Where(u => u.ApplicationUserId == userId && u.ApplicationUser.IsActive)
                .FirstOrDefaultAsync();

            if (clientUser is null)
                return Forbidden();

            IQueryable<Proposal> proposalsQuery =
                from clientUserSetting in _db.ClientUserPermissions
                where clientUserSetting.ClientUserId == clientUser.Id
                from client in _db.Clients
                where client.Id == clientUserSetting.ClientId
                from proposal in _db.Proposals
                    .Include(p => p.Employee)
                    .ThenInclude(c => c.PoolEmployee)
                    .ThenInclude(c => c.Supplier)
                    .ThenInclude(c => c.Header)
                    .Include(p => p.States)
                where proposal.EmployeeId == id && proposal.Employee.ClientId == client.Id
                select proposal;

            return EnvelopeResult.Ok(
                (IEnumerable<ProposalResponseDto>) _mapper.Map<List<ProposalResponseDto>>(proposalsQuery));
        }

        [HttpGet, Authorize, Route("get-proposals-of-poolemployee/{id}")]
        public async Task<IActionResult> GetProposalsOfPoolEmployee(long id)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers
                .Include(u => u.ApplicationUser)
                .Where(u => u.ApplicationUserId == userId && u.ApplicationUser.IsActive)
                .FirstOrDefaultAsync();

            if (clientUser is null)
                return Forbidden();

            IQueryable<Proposal> proposalsQuery =
                from clientUserSetting in _db.ClientUserPermissions
                where clientUserSetting.ClientUserId == clientUser.Id
                from client in _db.Clients
                where client.Id == clientUserSetting.ClientId
                from proposal in _db.Proposals
                    .Include(x => x.Employee)
                    .ThenInclude(x => x.PostalAddress)
                    .Include(p => p.Employee)
                    .ThenInclude(c => c.PoolEmployee)
                    .ThenInclude(c => c.Supplier)
                    .ThenInclude(c => c.Header)
                    .Include(p => p.States)
                where proposal.Employee.PoolEmployeeId == id && proposal.Employee.ClientId == client.Id
                select proposal;

            return EnvelopeResult.Ok(
                (IEnumerable<ProposalResponseDto>) _mapper.Map<List<ProposalResponseDto>>(proposalsQuery));
        }

        [HttpPut]
        [Route("accept")]
        public async Task<IActionResult> AcceptProposal([FromBody] ProposalAcceptanceRequestDto dto)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers
                .Include(u => u.ApplicationUser)
                .Where(u => u.ApplicationUserId == userId && u.ApplicationUser.IsActive)
                .FirstOrDefaultAsync();

            if (clientUser is null)
                return Forbidden();

            if (dto.ProposalIds.IsNullOrEmpty())
                return BadRequest();

            IQueryable<Proposal> proposalQuery =
                from clientUserSetting in _db.ClientUserPermissions
                where clientUserSetting.ClientUserId == clientUser.Id
                from client in _db.Clients
                where client.Id == clientUserSetting.ClientId
                from vacancy in _db.Vacancies
                where vacancy.ClientId == client.Id
                from proposal in _db.Proposals
                    .Include(p => p.States)
                    .Include(p => p.ProposalDocuments)
                where proposal.VacancyId == vacancy.Id && dto.ProposalIds.Contains(proposal.Id) &&
                      proposal.States.OrderByDescending(s => s.Creation).First().StateType != ProposalStateType.Accepted
                select proposal;

            IList<Proposal> modifications = new List<Proposal>();
            IList<Proposal> proposals = await proposalQuery.ToListAsync();

            foreach (Proposal proposal in proposals)
            {
                ProposalStateHistory stateHistoryEntry = new()
                {
                    Creation = new RecordCreation
                    {
                        CreatedById = userId.Value,
                        CreatedOn = DateTime.Now
                    },
                    ProposalId = proposal.Id,
                    StateType = ProposalStateType.Accepted
                };

                await _db.ProposalStates.AddAsync(stateHistoryEntry);

                proposal.States.Add(stateHistoryEntry);

                modifications.Add(proposal);

                await _db.EmployeeDocuments.AddRangeAsync(proposal.ProposalDocuments.Select(proposalDocument =>
                    new EmployeeDocument
                    {
                        Creation = new RecordCreation(userId.Value),
                        DocumentBytes = proposalDocument.DocumentBytes,
                        DocumentName = proposalDocument.DocumentName,
                        EmployeeId = proposal.EmployeeId
                    }));
            }

            _db.Proposals.UpdateRange(modifications);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(_mapper.Map<IEnumerable<ProposalResponseDto>>(modifications));
        }

        [HttpPut]
        [Route("reject")]
        public async Task<IActionResult> RejectProposal([FromBody] ProposalRejectionRequestDto dto)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers
                .Include(u => u.ApplicationUser)
                .Where(u => u.ApplicationUserId == userId && u.ApplicationUser.IsActive)
                .FirstOrDefaultAsync();

            if (clientUser is null)
                return Forbidden();

            if (dto.ProposalIds.IsNullOrEmpty())
                return BadRequest();

            IQueryable<Proposal> proposalQuery =
                from clientUserSetting in _db.ClientUserPermissions
                where clientUserSetting.ClientUserId == clientUser.Id
                from client in _db.Clients
                where client.Id == clientUserSetting.ClientId
                from vacancy in _db.Vacancies
                where vacancy.ClientId == client.Id
                from proposal in _db.Proposals
                    .Include(p => p.States)
                where proposal.VacancyId == vacancy.Id && dto.ProposalIds.Contains(proposal.Id) &&
                      proposal.States.OrderByDescending(s => s.Creation).First().StateType != ProposalStateType.Rejected
                select proposal;

            IList<Proposal> modifications = new List<Proposal>();
            IList<Proposal> proposals = await proposalQuery.ToListAsync();

            foreach (Proposal proposal in proposals)
            {
                ProposalStateHistory stateHistoryEntry = new()
                {
                    Creation = new RecordCreation
                    {
                        CreatedById = userId.Value,
                        CreatedOn = DateTime.Now
                    },
                    ProposalId = proposal.Id,
                    StateType = ProposalStateType.Rejected
                };

                await _db.ProposalStates.AddAsync(stateHistoryEntry);

                proposal.States.Add(stateHistoryEntry);

                modifications.Add(proposal);
            }

            _db.Proposals.UpdateRange(modifications);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(_mapper.Map<IEnumerable<ProposalResponseDto>>(modifications));
        }
    }
}