using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.TimeRecord;
using DasTeamRevolution.Models.Enums;
using DasTeamRevolution.Services.AuthTokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Controllers
{
    /// <summary>
    /// <see cref="TimeRecord"/> endpoints.
    /// </summary>
    [ODataRouting]
    [Authorize]
    public class TimeRecordsController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthTokenService _authTokenService;
        private readonly IMapper _mapper;

        public TimeRecordsController(ApplicationDbContext db, IAuthTokenService authTokenService, IMapper mapper)
        {
            _db = db;
            _authTokenService = authTokenService;
            _mapper = mapper;
        }

        // TODO: decide if " && !timeRecord.RemovedOn.HasValue" should be appended to the query to return only entries that weren't deleted, or if clients should do that via OData...

        // TODO: try to find a way to extract all these repetitive permission checks into a service interface (ticket  #360) - it's starting to get messy here!

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get(ODataQueryOptions<TimeRecord> query)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();
            SupplierUser supplierUser = await _db.SupplierUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            IQueryable<TimeRecord> timeRecordQuery;

            if (clientUser is not null)
            {
                timeRecordQuery =
                    from clientUserPermission in _db.ClientUserPermissions
                    where clientUserPermission.ClientUserId == clientUser.Id
                    from client in _db.Clients
                    where client.Id == clientUserPermission.ClientId
                    from vacancy in _db.Vacancies
                    where vacancy.ClientId == client.Id
                    from proposal in _db.Proposals
                    where proposal.VacancyId == vacancy.Id
                    from assignment in _db.Assignments
                    where assignment.ProposalId == proposal.Id
                    from timeRecord in _db.TimeRecords
                        .Include(t => t.Creation)
                        .Include(t => t.LastModification)
                        .Include(t => t.States)
                    where timeRecord.AssignmentId == assignment.Id
                    select timeRecord;
            }
            else if (supplierUser is not null)
            {
                timeRecordQuery =
                    from supplierUserPermission in _db.SupplierUserPermissions
                    where supplierUserPermission.SupplierUserId == supplierUser.Id
                    from supplier in _db.Suppliers
                    where supplier.Id == supplierUserPermission.SupplierId
                    from clientSupplier in _db.ClientSuppliers
                    where clientSupplier.SupplierId == supplier.Id
                    from client in _db.Clients
                    where client.Id == clientSupplier.ClientId
                    from vacancy in _db.Vacancies
                    where vacancy.ClientId == client.Id
                    from proposal in _db.Proposals
                    where proposal.VacancyId == vacancy.Id
                    from assignment in _db.Assignments
                    where assignment.ProposalId == proposal.Id
                    from timeRecord in _db.TimeRecords
                        .Include(t => t.Creation)
                        .Include(t => t.LastModification)
                        .Include(t => t.States)
                    where timeRecord.AssignmentId == assignment.Id
                    select timeRecord;
            }
            else
            {
                return Forbidden();
            }

            IEnumerable<TimeRecord> orders = (IEnumerable<TimeRecord>) query.ApplyTo(timeRecordQuery);

            return EnvelopeResult.Ok(_mapper.Map<IEnumerable<TimeRecordResponseDto>>(orders));
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();
            SupplierUser supplierUser = await _db.SupplierUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            IQueryable<TimeRecord> query;

            if (clientUser is not null)
            {
                query =
                    from clientUserPermission in _db.ClientUserPermissions
                    where clientUserPermission.ClientUserId == clientUser.Id
                    from client in _db.Clients
                    where client.Id == clientUserPermission.ClientId
                    from vacancy in _db.Vacancies
                    where vacancy.ClientId == client.Id
                    from proposal in _db.Proposals
                    where proposal.VacancyId == vacancy.Id
                    from assignment in _db.Assignments
                    where assignment.ProposalId == proposal.Id
                    from timeRecord in _db.TimeRecords
                        .Include(t => t.Creation)
                        .Include(t => t.LastModification)
                        .Include(t => t.States)
                    where timeRecord.AssignmentId == assignment.Id && timeRecord.Id == key
                    select timeRecord;
            }
            else if (supplierUser is not null)
            {
                query =
                    from supplierUserPermission in _db.SupplierUserPermissions
                    where supplierUserPermission.SupplierUserId == supplierUser.Id
                    from supplier in _db.Suppliers
                    where supplier.Id == supplierUserPermission.SupplierId
                    from clientSupplier in _db.ClientSuppliers
                    where clientSupplier.SupplierId == supplier.Id
                    from client in _db.Clients
                    where client.Id == clientSupplier.ClientId
                    from vacancy in _db.Vacancies
                    where vacancy.ClientId == client.Id
                    from proposal in _db.Proposals
                    where proposal.VacancyId == vacancy.Id
                    from assignment in _db.Assignments
                    where assignment.ProposalId == proposal.Id
                    from timeRecord in _db.TimeRecords
                        .Include(t => t.Creation)
                        .Include(t => t.LastModification)
                        .Include(t => t.States)
                    where timeRecord.AssignmentId == assignment.Id && timeRecord.Id == key
                    select timeRecord;
            }
            else
            {
                return Forbidden();
            }

            TimeRecord foundEntry = await query.SingleOrDefaultAsync();

            return foundEntry switch
            {
                null => Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<TimeRecord>(key)),
                _ => EnvelopeResult.Ok(_mapper.Map<TimeRecordResponseDto>(foundEntry))
            };
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TimeRecordRequestDto timeRecordRequest)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();
            SupplierUser supplierUser = await _db.SupplierUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            TimeRecord entry = _mapper.Map<TimeRecord>(timeRecordRequest);

            if (clientUser is not null)
            {
                IQueryable<Assignment> query =
                    from clientUserPermission in _db.ClientUserPermissions
                    where clientUserPermission.ClientUserId == clientUser.Id
                    from client in _db.Clients
                    where client.Id == clientUserPermission.ClientId
                    from vacancy in _db.Vacancies
                    where vacancy.ClientId == client.Id
                    from proposal in _db.Proposals
                    where proposal.VacancyId == vacancy.Id
                    from assignment in _db.Assignments
                    where assignment.ProposalId == proposal.Id && assignment.Id == entry.Id
                    select assignment;

                if (!await query.AnyAsync())
                {
                    return Forbidden(); // TODO: return decent error message and dto and all...
                }
            }
            else if (supplierUser is not null)
            {
                IQueryable<Assignment> query =
                    from supplierUserPermission in _db.SupplierUserPermissions
                    where supplierUserPermission.SupplierUserId == supplierUser.Id
                    from supplier in _db.Suppliers
                    where supplier.Id == supplierUserPermission.SupplierId
                    from clientSupplier in _db.ClientSuppliers
                    where clientSupplier.SupplierId == supplier.Id
                    from client in _db.Clients
                    where client.Id == clientSupplier.ClientId
                    from vacancy in _db.Vacancies
                    where vacancy.ClientId == client.Id
                    from proposal in _db.Proposals
                    where proposal.VacancyId == vacancy.Id
                    from assignment in _db.Assignments
                    where assignment.ProposalId == proposal.Id && assignment.Id == entry.AssignmentId
                    select assignment;

                if (!await query.AnyAsync())
                {
                    return Forbidden(); // TODO: return decent error message and dto and all...
                }
            }
            else
            {
                return Forbidden();
            }

            entry.Creation = new RecordCreation(userId.Value);

            await _db.TimeRecords.AddAsync(entry);
            await _db.SaveChangesAsync();

            await _db.TimeRecordStates.AddAsync(new TimeRecordStateHistory
            {
                Creation = entry.Creation,
                StateType = TimeRecordStateType.Created,
                TimeRecordId = entry.Id
            });

            await _db.SaveChangesAsync();

            entry = await _db.TimeRecords
                .Include(t => t.Creation)
                .Include(t => t.LastModification)
                .Include(t => t.States)
                .FirstOrDefaultAsync(t => t.Id == entry.Id);

            return EnvelopeResult.Created($"/api/v1/timerecords/{entry.Id}", entry);
        }

        [HttpPut]
        public async Task<IActionResult> Put(long key, [FromBody] TimeRecordRequestDto timeRecordRequest)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();
            SupplierUser supplierUser = await _db.SupplierUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            if (clientUser is not null)
            {
                IQueryable<Assignment> query =
                    from clientUserPermission in _db.ClientUserPermissions
                    where clientUserPermission.ClientUserId == clientUser.Id
                    from client in _db.Clients
                    where client.Id == clientUserPermission.ClientId
                    from vacancy in _db.Vacancies
                    where vacancy.ClientId == client.Id
                    from proposal in _db.Proposals
                    where proposal.VacancyId == vacancy.Id
                    from assignment in _db.Assignments
                    where assignment.ProposalId == proposal.Id && assignment.Id == key
                    select assignment;

                if (!await query.AnyAsync())
                {
                    return Forbidden(); // TODO: return decent error message and dto and all...
                }
            }
            else if (supplierUser is not null)
            {
                IQueryable<Assignment> query =
                    from supplierUserPermission in _db.SupplierUserPermissions
                    where supplierUserPermission.SupplierUserId == supplierUser.Id
                    from supplier in _db.Suppliers
                    where supplier.Id == supplierUserPermission.SupplierId
                    from clientSupplier in _db.ClientSuppliers
                    where clientSupplier.SupplierId == supplier.Id
                    from client in _db.Clients
                    where client.Id == clientSupplier.ClientId
                    from vacancy in _db.Vacancies
                    where vacancy.ClientId == client.Id
                    from proposal in _db.Proposals
                    where proposal.VacancyId == vacancy.Id
                    from assignment in _db.Assignments
                    where assignment.ProposalId == proposal.Id && assignment.Id == key
                    select assignment;

                if (!await query.AnyAsync())
                {
                    return Forbidden(); // TODO: return decent error message and dto and all...
                }
            }
            else
            {
                return Forbidden();
            }

            TimeRecord entry = _mapper.Map<TimeRecord>(timeRecordRequest);
            
            TimeRecord storedEntry = await _db.TimeRecords.FirstOrDefaultAsync(t => t.Id == key);

            if (storedEntry is null)
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<TimeRecord>(key));

            entry.Id = storedEntry.Id;
            entry.Assignment = null;
            entry.ClientUser = null;
            entry.LastModification = new RecordModification { ModifiedById = userId, ModifiedOn = DateTime.Now }; // TODO: add a reason string to the mod entry

            _db.TimeRecords.Update(entry);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(_mapper.Map<TimeRecordResponseDto>(storedEntry));
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(long key)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();
            SupplierUser supplierUser = await _db.SupplierUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            if (clientUser is not null)
            {
                IQueryable<Assignment> query =
                    from clientUserPermission in _db.ClientUserPermissions
                    where clientUserPermission.ClientUserId == clientUser.Id
                    from client in _db.Clients
                    where client.Id == clientUserPermission.ClientId
                    from vacancy in _db.Vacancies
                    where vacancy.ClientId == client.Id
                    from proposal in _db.Proposals
                    where proposal.VacancyId == vacancy.Id
                    from assignment in _db.Assignments
                    where assignment.ProposalId == proposal.Id && assignment.Id == key
                    select assignment;

                if (!await query.AnyAsync())
                {
                    return Forbidden(); // TODO: return decent error message and dto and all...
                }
            }
            else if (supplierUser is not null)
            {
                IQueryable<Assignment> query =
                    from supplierUserPermission in _db.SupplierUserPermissions
                    where supplierUserPermission.SupplierUserId == supplierUser.Id
                    from supplier in _db.Suppliers
                    where supplier.Id == supplierUserPermission.SupplierId
                    from clientSupplier in _db.ClientSuppliers
                    where clientSupplier.SupplierId == supplier.Id
                    from client in _db.Clients
                    where client.Id == clientSupplier.ClientId
                    from vacancy in _db.Vacancies
                    where vacancy.ClientId == client.Id
                    from proposal in _db.Proposals
                    where proposal.VacancyId == vacancy.Id
                    from assignment in _db.Assignments
                    where assignment.ProposalId == proposal.Id && assignment.Id == key
                    select assignment;

                if (!await query.AnyAsync())
                {
                    return Forbidden(); // TODO: return decent error message and dto and all...
                }
            }
            else
            {
                return Forbidden();
            }

            TimeRecord entry = await _db.TimeRecords
                .Include(t => t.Creation)
                .Include(t => t.LastModification)
                .Include(t => t.States)
                .FirstOrDefaultAsync(t => t.Id == key);

            if (entry is null)
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<TimeRecord>(key));

            entry.RemovedOn = DateTime.Now;
            entry.LastModification = new RecordModification { ModifiedById = userId, ModifiedOn = DateTime.Now }; // TODO: add a reason string to the mod entry

            _db.TimeRecords.Update(entry);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(_mapper.Map<TimeRecordResponseDto>(entry));
        }
    }
}