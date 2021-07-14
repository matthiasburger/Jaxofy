using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.ClientUserSetting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Controllers
{
    [ODataRouting]
    [Authorize]
    public class ClientUserSettingsController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ClientUserSettingsController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(ODataQueryOptions<ClientUserSetting> query)
        {
            IQueryable clientUserSettings = query.ApplyTo(
                _db.ClientUserPermissions.AsQueryable()
            );

            return EnvelopeResult.Ok((IEnumerable<ClientUserSettingResponseDto>)_mapper.Map<List<ClientUserSettingResponseDto>>(clientUserSettings));
        }

        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            ClientUserSetting clientUserSetting = 
                await _db.ClientUserPermissions.FirstOrDefaultAsync(x => x.Id == key);

            return EnvelopeResult.Ok(_mapper.Map<ClientUserSettingResponseDto>(clientUserSetting));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ClientUserSettingRequestDto clientUserPermissionRequest)
        {
            ClientUserSetting clientUserPermission = _mapper.Map<ClientUserSetting>(clientUserPermissionRequest);

            await _db.ClientUserPermissions.AddAsync(clientUserPermission);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Created($"/api/v1/clientusersettings/{clientUserPermission.Id}", clientUserPermission);
        }

        [HttpPut]
        public async Task<ActionResult> Put(long key,
            [FromBody] ClientUserSettingRequestDto clientUserPermissionRequest)
        {
            ClientUserSetting clientUserPermission = _mapper.Map<ClientUserSetting>(clientUserPermissionRequest);
            clientUserPermission.Id = key;

            _db.ClientUserPermissions.Update(clientUserPermission);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(clientUserPermission);
        }
    }
}