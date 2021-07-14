using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Supplier;
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
    public class SuppliersController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IAuthTokenService _authTokenService;
        private readonly IAddressResolutionService _addressResolutionService;

        public SuppliersController(ApplicationDbContext db, IMapper mapper, IAuthTokenService authTokenService, IAddressResolutionService addressResolutionService)
        {
            _db = db;
            _mapper = mapper;
            _authTokenService = authTokenService;
            _addressResolutionService = addressResolutionService;
        }

        [EnableQuery]
        [HttpGet]
        public ActionResult<IQueryable<IActionResult>> Get(ODataQueryOptions<Supplier> query)
        {
            IQueryable suppliers = query.ApplyTo(_db.Suppliers.AsQueryable());
            return EnvelopeResult.Ok((IEnumerable<SupplierResponseDto>)_mapper.Map<List<SupplierResponseDto>>(suppliers));
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            Supplier supplier = await _db.Suppliers.FirstOrDefaultAsync(x => x.Id == key);
            return EnvelopeResult.Ok(_mapper.Map<SupplierResponseDto>(supplier));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SupplierRequestDto supplierRequest)
        {
            Supplier supplier = _mapper.Map<Supplier>(supplierRequest);

            long? userId = _authTokenService.ExtractUserId(HttpContext);
            if (!userId.HasValue)
            {
                return Forbidden();
            }

            supplier.Creation = new RecordCreation
            {
                CreatedById = userId.Value,
                CreatedOn = DateTime.Now
            };

            if (supplier.PostalAddress is not null)
            {
                long addressId = await _addressResolutionService.GetOrAddPostalAddress(supplier.PostalAddress);
                
                supplier.PostalAddress = null;
                supplier.PostalAddressId = addressId;
            }

            await _db.Suppliers.AddAsync(supplier);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Created($"/api/v1/suppliers/{supplier.Id}", supplier);
        }

        [HttpPut]
        public async Task<IActionResult> Put(long key, [FromBody] SupplierRequestDto supplierRequest)
        {
            Supplier supplier = _mapper.Map<Supplier>(supplierRequest);
            supplier.Id = key;

            Supplier storedSupplier = await _db.Suppliers
                .AsNoTracking()
                .Include(s => s.PostalAddress)
                .SingleOrDefaultAsync(x => x.Id == key);

            if (storedSupplier is null)
            {
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<Supplier>(key));
            }

            supplier.CreationId = storedSupplier.CreationId;
            supplier.LastModificationId = storedSupplier.LastModificationId;
            supplier.PostalAddressId = storedSupplier.PostalAddressId;
            
            long? userId = _authTokenService.ExtractUserId(HttpContext);
            if (!userId.HasValue)
            {
                return Forbidden();
            }

            supplier.LastModification = new RecordModification
            {
                Id = storedSupplier.LastModificationId ?? 0,
                ModifiedById = userId,
                ModifiedOn = DateTime.Now
            };

            if (supplier.PostalAddress is not null && !supplier.PostalAddress.Equals(storedSupplier.PostalAddress))
            {
                long existingAddress = await _db.PostalAddresses
                    .AsNoTracking()
                    .Where(p =>
                        p.CountryCodeISO == supplier.PostalAddress.CountryCodeISO /////
                        && p.PostalZipCode == supplier.PostalAddress.PostalZipCode ////
                        && p.PostalCity == supplier.PostalAddress.PostalCity //////////
                        && p.PostalStreet == supplier.PostalAddress.PostalStreet //////
                        && p.PostalName == supplier.PostalAddress.PostalName)
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync();

                if (existingAddress != 0)
                {
                    supplier.PostalAddress = null;
                    supplier.PostalAddressId = existingAddress;
                }
                else
                {
                    _db.PostalAddresses.Update(supplier.PostalAddress);
                }
            }

            _db.Suppliers.Update(supplier);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(supplier);
        }
    }
}