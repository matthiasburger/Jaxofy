using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.JobProfile;
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
    public class JobProfilesController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IAuthTokenService _authTokenService;

        public JobProfilesController(ApplicationDbContext db, IMapper mapper, IAuthTokenService authTokenService)
        {
            _db = db;
            _mapper = mapper;
            _authTokenService = authTokenService;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get(ODataQueryOptions<JobProfile> query)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers
                .Where(u => u.ApplicationUserId == userId)
                .FirstOrDefaultAsync();

            SupplierUser supplierUser = await _db.SupplierUsers
                .Where(u => u.ApplicationUserId == userId)
                .FirstOrDefaultAsync();

            if (clientUser is null && supplierUser is null)
                return Forbidden();

            IQueryable<JobProfile> jobProfileQuery;

            if (clientUser is not null)
            {
                jobProfileQuery =
                    from clientUserSetting in _db.ClientUserPermissions
                    where clientUserSetting.ClientUserId == clientUser.Id
                    from client in _db.Clients
                    where client.Id == clientUserSetting.ClientId
                    from clientSupplier in _db.ClientSuppliers
                    where clientSupplier.ClientId == client.Id
                    from jobProfile in _db.JobProfiles
                    where jobProfile.ClientSupplierId == clientSupplier.Id
                    select jobProfile;
            }
            else
            {
                jobProfileQuery =
                    from supplierUserSetting in _db.SupplierUserPermissions
                    where supplierUserSetting.SupplierUserId == supplierUser.Id
                    from supplier in _db.Suppliers
                    where supplier.Id == supplierUserSetting.SupplierId
                    from clientSupplier in _db.ClientSuppliers
                    where clientSupplier.SupplierId == supplier.Id
                    from jobProfile in _db.JobProfiles
                    where jobProfile.ClientSupplierId == clientSupplier.Id
                    select jobProfile;
            }

            IQueryable jobProfiles = query.ApplyTo(jobProfileQuery);

            return EnvelopeResult.Ok((IEnumerable<JobProfileResponseDto>) _mapper.Map<List<JobProfileResponseDto>>(jobProfiles));
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers
                .Where(u => u.ApplicationUserId == userId)
                .FirstOrDefaultAsync();

            SupplierUser supplierUser = await _db.SupplierUsers
                .Where(u => u.ApplicationUserId == userId)
                .FirstOrDefaultAsync();

            if (clientUser is null && supplierUser is null)
                return Forbidden();

            IQueryable<JobProfile> jobProfileQuery;

            if (clientUser is not null)
            {
                jobProfileQuery =
                    from clientUserSetting in _db.ClientUserPermissions
                    where clientUserSetting.ClientUserId == clientUser.Id
                    from client in _db.Clients
                    where client.Id == clientUserSetting.ClientId
                    from clientSupplier in _db.ClientSuppliers
                    where clientSupplier.ClientId == client.Id
                    from jobProfile in _db.JobProfiles
                    where jobProfile.ClientSupplierId == clientSupplier.Id && jobProfile.Id == key
                    select jobProfile;
            }
            else
            {
                jobProfileQuery =
                    from supplierUserSetting in _db.SupplierUserPermissions
                    where supplierUserSetting.SupplierUserId == supplierUser.Id
                    from supplier in _db.Suppliers
                    where supplier.Id == supplierUserSetting.SupplierId
                    from clientSupplier in _db.ClientSuppliers
                    where clientSupplier.SupplierId == supplier.Id
                    from jobProfile in _db.JobProfiles
                    where jobProfile.ClientSupplierId == clientSupplier.Id && jobProfile.Id == key
                    select jobProfile;
            }

            JobProfile foundJobProfile = await jobProfileQuery.FirstOrDefaultAsync();

            if (foundJobProfile is null)
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<JobProfile>(key));

            return EnvelopeResult.Ok(_mapper.Map<JobProfileResponseDto>(foundJobProfile));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] JobProfileRequestDto jobProfileRequest)
        {
            // TODO: handle permission

            JobProfile jobProfile = _mapper.Map<JobProfile>(jobProfileRequest);

            await _db.JobProfiles.AddAsync(jobProfile);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Created($"/api/v1/JobProfiles/{jobProfile.Id}", jobProfile);
        }

        [HttpPut]
        public async Task<ActionResult> Put(long key, [FromBody] JobProfileRequestDto jobProfileRequest)
        {
            // TODO: handle permission

            JobProfile jobProfile = _mapper.Map<JobProfile>(jobProfileRequest);
            jobProfile.Id = key;

            _db.JobProfiles.Update(jobProfile);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(jobProfile);
        }
    }
}