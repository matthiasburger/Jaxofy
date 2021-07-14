using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.PoolEmployee;
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
    public class PoolEmployeesController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthTokenService _authTokenService;
        private readonly IMapper _mapper;

        public PoolEmployeesController(ApplicationDbContext db, IAuthTokenService authTokenService, IMapper mapper)
        {
            _db = db;
            _authTokenService = authTokenService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(ODataQueryOptions<PoolEmployee> query)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            SupplierUser supplierUser = await _db.SupplierUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            if (supplierUser is null)
                return Forbidden();

            IQueryable<PoolEmployee> poolEmployees =
                from supplierUserSetting in _db.SupplierUserPermissions
                where supplierUserSetting.SupplierUserId == supplierUser.Id
                from supplier in _db.Suppliers
                where supplier.Id == supplierUserSetting.SupplierId
                from poolEmployee in _db.PoolEmployees
                where poolEmployee.SupplierId == supplier.Id
                select poolEmployee;

            IQueryable poolEmployeeQuery = query.ApplyTo(poolEmployees);

            return EnvelopeResult.Ok((IEnumerable<PoolEmployeeResponseDto>)_mapper.Map<List<PoolEmployeeResponseDto>>(poolEmployeeQuery));
        }

        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            SupplierUser supplierUser = await _db.SupplierUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            if (supplierUser is null)
                return Forbidden();

            IQueryable<PoolEmployee> poolEmployeeQuery =
                from supplierUserSetting in _db.SupplierUserPermissions
                where supplierUserSetting.SupplierUserId == supplierUser.Id
                from supplier in _db.Suppliers
                where supplier.Id == supplierUserSetting.SupplierId
                from _poolEmployee in _db.PoolEmployees
                where _poolEmployee.SupplierId == supplier.Id && _poolEmployee.Id == key
                select _poolEmployee;

            PoolEmployee poolEmployee = await poolEmployeeQuery.SingleOrDefaultAsync();

            if (poolEmployee is null)
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<PoolEmployee>(key));

            return EnvelopeResult.Ok(_mapper.Map<PoolEmployeeResponseDto>(poolEmployee));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PoolEmployeeRequestDto poolEmployeeRequest)
        {
            PoolEmployee poolEmployee = _mapper.Map<PoolEmployee>(poolEmployeeRequest);

            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            SupplierUser supplierUser = await _db.SupplierUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            if (supplierUser is null)
                return Forbidden();

            poolEmployee.Creation = new RecordCreation
            {
                CreatedById = userId,
                CreatedOn = DateTime.Now
            };

            await _db.PoolEmployees.AddAsync(poolEmployee);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Created($"/api/v1/poolemployees/{poolEmployee.Id}", poolEmployee);
        }

        [HttpPut]
        public async Task<IActionResult> Put(long key, [FromBody] PoolEmployeeRequestDto poolEmployeeRequest)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            SupplierUser supplierUser = await _db.SupplierUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            if (supplierUser is null)
                return Forbidden();

            IQueryable<PoolEmployee> storedPoolEmployeeQuery =
                from supplierUserSetting in _db.SupplierUserPermissions
                where supplierUserSetting.SupplierUserId == supplierUser.Id
                from supplier in _db.Suppliers
                where supplier.Id == supplierUserSetting.SupplierId
                from _poolEmployee in _db.PoolEmployees
                where _poolEmployee.Id == key && _poolEmployee.SupplierId == supplier.Id
                select _poolEmployee;
            
            PoolEmployee poolEmployee = await storedPoolEmployeeQuery.SingleOrDefaultAsync();

            if (poolEmployee is null)
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<PoolEmployee>(key));

            // TODO: pass a reason value into the endpoint's parameter DTO and specify it here inside the modification record.
            
            poolEmployee.LastModification = new RecordModification
            {
                Id = poolEmployee.LastModificationId ?? 0,
                ModifiedById = userId.Value,
                ModifiedOn = DateTime.Now
            };

            poolEmployee.Email = poolEmployeeRequest.Email;
            poolEmployee.Phone = poolEmployeeRequest.Phone;
            poolEmployee.Mobile = poolEmployeeRequest.Mobile;
            poolEmployee.LastName = poolEmployeeRequest.LastName;
            poolEmployee.FirstName = poolEmployeeRequest.FirstName;
            poolEmployee.SupplierId = poolEmployeeRequest.SupplierId;
            poolEmployee.DateOfBirth = poolEmployeeRequest.DateOfBirth;
            poolEmployee.IsActive = poolEmployeeRequest.IsActive;
            poolEmployee.Gender = poolEmployeeRequest.Gender;

            _db.PoolEmployees.Update(poolEmployee);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(poolEmployee);
        }
    }
}