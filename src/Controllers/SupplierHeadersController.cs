using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.SupplierHeader;
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
    public class SupplierHeadersController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IAuthTokenService _authTokenService;

        public SupplierHeadersController(ApplicationDbContext db, IMapper mapper, IAuthTokenService authTokenService)
        {
            _db = db;
            _mapper = mapper;
            _authTokenService = authTokenService;
        }

        [HttpGet]
        public IActionResult Get(ODataQueryOptions<SupplierHeader> query)
        {
            IQueryable supplierHeaders = query.ApplyTo(_db.SupplierHeaders.AsQueryable());
            return EnvelopeResult.Ok((IEnumerable<SupplierHeaderResponseDto>)_mapper.Map<List<SupplierHeaderResponseDto>>(supplierHeaders));
        }

        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            SupplierHeader supplierHeader = await _db.SupplierHeaders.FirstOrDefaultAsync(x => x.Id == key);
            return EnvelopeResult.Ok(_mapper.Map<SupplierHeaderResponseDto>(supplierHeader));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SupplierHeaderRequestDto supplierHeaderRequest)
        {
            SupplierHeader supplierHeader = _mapper.Map<SupplierHeader>(supplierHeaderRequest);
            
            long? userId = _authTokenService.ExtractUserId(HttpContext);
            if (!userId.HasValue)
            {
                return Forbidden();
            }

            // TODO: check user permission here!
            
            supplierHeader.Creation = new RecordCreation
            {
                CreatedById = userId.Value,
                CreatedOn = DateTime.Now
            };

            await _db.SupplierHeaders.AddAsync(supplierHeader);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Created($"/api/v1/supplierheaders/{supplierHeader.Id}", supplierHeader);
        }

        [HttpPut]
        public async Task<ActionResult> Put(long key, [FromBody] SupplierHeaderRequestDto supplierHeaderRequest)
        {
            SupplierHeader supplierHeader = _mapper.Map<SupplierHeader>(supplierHeaderRequest);
            supplierHeader.Id = key;
            
            SupplierHeader storedSupplierHeader = await _db.SupplierHeaders.AsNoTracking().SingleOrDefaultAsync(x => x.Id == key);

            supplierHeader.CreationId = storedSupplierHeader.CreationId;
            supplierHeader.LastModificationId = storedSupplierHeader.LastModificationId;
            
            long? userId = _authTokenService.ExtractUserId(HttpContext);
            if (!userId.HasValue)
            {
                return Forbidden();
            }

            // TODO: check user permission here!
            
            supplierHeader.LastModification = new RecordModification
            {
                Id = storedSupplierHeader.LastModificationId ?? 0,
                ModifiedById = userId,
                ModifiedOn = DateTime.Now
            };

            _db.SupplierHeaders.Update(supplierHeader);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(supplierHeader);
        }
    }
}