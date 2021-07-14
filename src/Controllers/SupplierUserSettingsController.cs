using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.SupplierUserSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Controllers
{
    [ODataRouting]
    [Authorize]
    public class SupplierUserSettingsController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public SupplierUserSettingsController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IQueryable<IActionResult>> Get(ODataQueryOptions<SupplierUserSetting> query)
        {
            IQueryable supplierUserSettings = query.ApplyTo(_db.SupplierUserPermissions.AsQueryable());
            return EnvelopeResult.Ok((IEnumerable<SupplierUserSettingsResponseDto>)_mapper.Map<List<SupplierUserSettingsResponseDto>>(supplierUserSettings));
        }

        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            SupplierUserSetting supplierUserSetting =
                await _db.SupplierUserPermissions.FirstOrDefaultAsync(x => x.Id == key);

            return EnvelopeResult.Ok(_mapper.Map<SupplierUserSettingsResponseDto>(supplierUserSetting));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SupplierUserSettingsRequestDto supplierUserPermissionRequest)
        {
            SupplierUserSetting supplierUserPermission =
                _mapper.Map<SupplierUserSetting>(supplierUserPermissionRequest);

            await _db.SupplierUserPermissions.AddAsync(supplierUserPermission);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Created($"/api/v1/supplierusersettings/{supplierUserPermission.Id}", supplierUserPermission);
        }

        [HttpPut]
        public async Task<ActionResult> Put(long key,
            [FromBody] SupplierUserSettingsRequestDto supplierUserPermissionRequest)
        {
            SupplierUserSetting supplierUserPermission =
                _mapper.Map<SupplierUserSetting>(supplierUserPermissionRequest);
            supplierUserPermission.Id = key;

            _db.SupplierUserPermissions.Update(supplierUserPermission);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(supplierUserPermission);
        }
    }
}