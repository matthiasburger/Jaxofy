using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Vacancy;
using DasTeamRevolution.Services.AddressResolution;
using DasTeamRevolution.Services.AuthTokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Controllers
{
    [ODataRouting]
    [Authorize]
    public class VacanciesController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthTokenService _authTokenService;
        private readonly IMapper _mapper;
        private readonly IAddressResolutionService _addressResolutionService;

        public VacanciesController(ApplicationDbContext db, IAuthTokenService authTokenService, IMapper mapper, IAddressResolutionService addressResolutionService)
        {
            _db = db;
            _mapper = mapper;
            _authTokenService = authTokenService;
            _addressResolutionService = addressResolutionService;
        }

        [HttpGet]
        public async Task<ActionResult<IQueryable<IActionResult>>> Get(ODataQueryOptions<Vacancy> query)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
            {
                return Forbidden();
            }

            ApplicationUser user = await _db.ApplicationUsers
                .Where(u => u.Id == userId && u.IsActive)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                return Forbidden();
            }

            IQueryable<Vacancy> vacancies = _db.Vacancies
                .Include(x => x.States)
                .Include(v => v.PostalAddress)
                .Include(v => v.Creation)
                .Include(v => v.LastModification);

            IQueryable<long> clientIdQuery =
                from clientUser in _db.ClientUsers
                where clientUser.ApplicationUserId == user.Id
                from clientUserSetting in _db.ClientUserPermissions
                where clientUserSetting.ClientUserId == clientUser.Id
                select clientUserSetting.ClientId;

            long clientId = await clientIdQuery.FirstOrDefaultAsync();

            if (clientId != default)
            {
                return EnvelopeResult.Ok((IEnumerable<VacancyResponseDto>) _mapper.Map<List<VacancyResponseDto>>(vacancies.Where(v => v.ClientId == clientId)));
            }

            vacancies =
                from supplierUser in _db.SupplierUsers
                where supplierUser.ApplicationUserId == user.Id
                from supplierUserSetting in _db.SupplierUserPermissions
                where supplierUserSetting.SupplierUserId == supplierUser.Id
                from supplier in _db.Suppliers
                where supplier.Id == supplierUserSetting.SupplierId
                from clientSupplier in _db.ClientSuppliers
                where clientSupplier.SupplierId == supplier.Id
                from client in _db.Clients
                where client.Id == clientSupplier.ClientId
                from vacancy in _db.Vacancies
                    .Include(v => v.PostalAddress)
                    .Include(v => v.Creation)
                    .Include(v => v.LastModification)
                    .Include(x => x.States)
                where vacancy.ClientId == client.Id
                select vacancy;

            IQueryable vacancyResult = query.ApplyTo(vacancies);

            return EnvelopeResult.Ok((IEnumerable<VacancyResponseDto>) _mapper.Map<List<VacancyResponseDto>>(vacancyResult));
        }

        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            Vacancy vacancy = await _db.Vacancies
                .Include(v => v.PostalAddress)
                .Include(v => v.Creation)
                .Include(v => v.States)
                .SingleOrDefaultAsync(x => x.Id == key);

            // TODO: only return result if the requesting user has the necessary permission

            if (vacancy is null)
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<Vacancy>(key));

            return EnvelopeResult.Ok(_mapper.Map<VacancyResponseDto>(vacancy));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VacancyRequestDto vacancyRequest)
        {
            Vacancy vacancy = _mapper.Map<Vacancy>(vacancyRequest);

            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Error(HttpStatusCode.BadRequest, Constants.Errors.UserNotFound);

            if (vacancy.PostalAddress is not null)
            {
                long addressId = await _addressResolutionService.GetOrAddPostalAddress(vacancy.PostalAddress);

                vacancy.PostalAddress = null;
                vacancy.PostalAddressId = addressId;
            }

            vacancy.Creation = new RecordCreation
            {
                CreatedById = userId.Value,
                CreatedOn = DateTime.Now
            };

            await _db.Vacancies.AddAsync(vacancy);
            await _db.SaveChangesAsync();
            vacancy = await _db.Vacancies
                .Include(v => v.PostalAddress)
                .Include(v => v.Creation)
                .Include(v => v.LastModification)
                .Include(x => x.States)
                .FirstOrDefaultAsync(v => v.Id == vacancy.Id);

            return EnvelopeResult.Created($"/api/v1/vacancies/{vacancy.Id}", _mapper.Map<VacancyResponseDto>(vacancy));
        }

        [HttpPut]
        public async Task<ActionResult> Put(long key, [FromBody] VacancyRequestDto vacancyRequest)
        {
            Vacancy vacancy = _mapper.Map<Vacancy>(vacancyRequest);
            vacancy.Id = key;

            Vacancy storedVacancy = await _db.Vacancies
                .AsNoTracking()
                .Include(v => v.PostalAddress)
                .SingleOrDefaultAsync(x => x.Id == key);

            vacancy.CreationId = storedVacancy.CreationId;
            vacancy.LastModificationId = storedVacancy.LastModificationId;

            long? userId = _authTokenService.ExtractUserId(HttpContext);
            if (!userId.HasValue)
            {
                return Forbidden();
            }

            vacancy.LastModification = new RecordModification
            {
                Id = storedVacancy.LastModificationId ?? 0,
                ModifiedById = userId,
                ModifiedOn = DateTime.Now
            };

            if (vacancy.PostalAddress is not null && !vacancy.PostalAddress.Equals(storedVacancy.PostalAddress))
            {
                long existingAddress = await _db.PostalAddresses
                    .AsNoTracking()
                    .Where(p =>
                        p.CountryCodeISO == vacancy.PostalAddress.CountryCodeISO /////
                        && p.PostalZipCode == vacancy.PostalAddress.PostalZipCode ////
                        && p.PostalCity == vacancy.PostalAddress.PostalCity //////////
                        && p.PostalStreet == vacancy.PostalAddress.PostalStreet //////
                        && p.PostalName == vacancy.PostalAddress.PostalName)
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync();

                if (existingAddress != 0)
                {
                    vacancy.PostalAddress = null;
                    vacancy.PostalAddressId = existingAddress;
                }
                else
                {
                    _db.PostalAddresses.Update(vacancy.PostalAddress);
                }
            }

            _db.Vacancies.Update(vacancy);
            await _db.SaveChangesAsync();

            vacancy = await _db.Vacancies
                .Include(v => v.PostalAddress)
                .Include(v => v.Creation)
                .Include(v => v.LastModification)
                .Include(x => x.States)
                .FirstOrDefaultAsync(v => v.Id == vacancy.Id);

            return EnvelopeResult.Updated(_mapper.Map<VacancyResponseDto>(vacancy));
        }
    }
}