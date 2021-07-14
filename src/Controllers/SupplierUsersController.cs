using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.SupplierUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Controllers
{
    [ODataRouting]
    [Authorize]
    public class SupplierUsersController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public SupplierUsersController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get(ODataQueryOptions<SupplierUser> query)
        {
            IQueryable supplierUsers = query.ApplyTo(_db.SupplierUsers.AsQueryable());
            return EnvelopeResult.Ok((IEnumerable<SupplierUserResponseDto>)_mapper.Map<List<SupplierUserResponseDto>>(supplierUsers));
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            SupplierUser supplierUser = await _db.SupplierUsers.FirstOrDefaultAsync(x => x.Id == key);
            return EnvelopeResult.Ok(_mapper.Map<SupplierUserResponseDto>(supplierUser));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SupplierUserRequestDto supplierUserRequest)
        {
            SupplierUser supplierUser = _mapper.Map<SupplierUser>(supplierUserRequest);

            await _db.SupplierUsers.AddAsync(supplierUser);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Created($"/api/v1/supplierusers/{supplierUser.Id}", supplierUser);
        }

        [HttpPut]
        public async Task<ActionResult> Put(long key, [FromBody] SupplierUserRequestDto supplierUserRequest)
        {
            SupplierUser supplierUser = _mapper.Map<SupplierUser>(supplierUserRequest);
            supplierUser.Id = key;

            _db.SupplierUsers.Update(supplierUser);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(supplierUser);
        }
    }
}