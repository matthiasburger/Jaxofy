using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.ClientSupplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Controllers
{
    [ODataRouting]
    [Authorize]
    public class ClientSuppliersController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ClientSuppliersController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get(ODataQueryOptions<ClientSupplier> query)
        {
            IQueryable clientSuppliers = query.ApplyTo(
                _db.ClientSuppliers.AsQueryable()
            );
            
            return EnvelopeResult.Ok((IEnumerable<ClientSupplierResponseDto>)_mapper.Map<List<ClientSupplierResponseDto>>(clientSuppliers));
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            ClientSupplier clientSupplier = await _db.ClientSuppliers
                .SingleOrDefaultAsync(x => x.Id == key);

            if (clientSupplier is null)
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<Client>(key));

            return EnvelopeResult.Ok(_mapper.Map<ClientSupplierResponseDto>(clientSupplier));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ClientSupplierRequestDto clientSupplierRequest)
        {
            ClientSupplier clientSupplier = _mapper.Map<ClientSupplier>(clientSupplierRequest);
            
            await _db.ClientSuppliers.AddAsync(clientSupplier);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Created($"/api/v1/clientsuppliers/{clientSupplier.Id}", clientSupplier);
        }

        [HttpPut]
        public async Task<ActionResult> Put(long key, [FromBody] ClientSupplierRequestDto clientSupplierRequest)
        {
            ClientSupplier clientSupplier = _mapper.Map<ClientSupplier>(clientSupplierRequest);
            clientSupplier.Id = key;

            _db.ClientSuppliers.Update(clientSupplier);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(clientSupplier);
        }
    }
}