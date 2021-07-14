using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Employee;
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
    public class EmployeesController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthTokenService _authTokenService;
        private readonly IMapper _mapper;
        private readonly IAddressResolutionService _addressResolutionService;

        public EmployeesController(ApplicationDbContext db, IAuthTokenService authTokenService, IMapper mapper,
            IAddressResolutionService addressResolutionService)
        {
            _db = db;
            _mapper = mapper;
            _addressResolutionService = addressResolutionService;
            _authTokenService = authTokenService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(ODataQueryOptions<Employee> query)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser =
                await _db.ClientUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            if (clientUser is null)
                return Forbidden();

            IQueryable<Employee> employees =
                from clientUserSetting in _db.ClientUserPermissions
                where clientUserSetting.ClientUserId == clientUser.Id
                from client in _db.Clients
                where client.Id == clientUserSetting.ClientId
                from employee in _db.Employees
                    .Include(e => e.PostalAddress)
                    .Include(e => e.Creation)
                    .Include(e => e.LastModification)
                    .Include(e => e.PoolEmployee)
                    .ThenInclude(e => e.Supplier)
                    .ThenInclude(e => e.Header)
                    .Include(e => e.Assignments)
                where employee.ClientId == client.Id
                select employee;

            IQueryable employeeQuery = query.ApplyTo(employees);

            return EnvelopeResult.Ok(
                (IEnumerable<EmployeeResponseDto>) _mapper.Map<List<EmployeeResponseDto>>(employeeQuery));
        }

        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser =
                await _db.ClientUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            if (clientUser is null)
                return Forbidden();

            Employee employeeItem = await (
                from clientUserSetting in _db.ClientUserPermissions
                where clientUserSetting.ClientUserId == clientUser.Id
                from client in _db.Clients
                where client.Id == clientUserSetting.ClientId
                from employee in _db.Employees
                    .Include(e => e.PostalAddress)
                    .Include(e => e.Creation)
                    .Include(e => e.LastModification)
                    .Include(e => e.PoolEmployee)
                    .ThenInclude(e => e.Supplier)
                    .ThenInclude(e => e.Header)
                where employee.Id == key && employee.ClientId == client.Id
                select employee
            ).FirstOrDefaultAsync();

            if (employeeItem is null)
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<Employee>(key));

            return EnvelopeResult.Ok(_mapper.Map<EmployeeResponseDto>(employeeItem));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EmployeeRequestDto employeeRequest)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser =
                await _db.ClientUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            if (clientUser is null)
                return Forbidden();

            IQueryable<long> supplierQuery =
                from clientUserSetting in _db.ClientUserPermissions
                where clientUserSetting.ClientUserId == clientUser.Id
                from clientSupplier in _db.ClientSuppliers
                where clientSupplier.ClientId == clientUserSetting.ClientId
                from supplier in _db.Suppliers
                where supplier.Id == employeeRequest.SupplierId && supplier.Id == clientSupplier.SupplierId
                select supplier.Id;

            if (!await supplierQuery.AnyAsync())
            {
                // Supplier is not related to requesting client or does not exist!
                return BadRequest();
            }

            Employee employee = _mapper.Map<Employee>(employeeRequest);

            // Custom client-created employees MUST absolutely have their PoolEmployeeId set to null!
            employee.PoolEmployeeId = null;

            employee.Creation = new RecordCreation
            {
                CreatedById = userId,
                CreatedOn = DateTime.Now
            };

            employee.PostalAddress = null;
            employee.PostalAddressId = employeeRequest.PostalAddress is not null
                ? await _addressResolutionService.GetOrAddPostalAddress(employeeRequest.PostalAddress)
                : null;

            await _db.Employees.AddAsync(employee);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Created($"/api/v1/employees/{employee.Id}", employee);
        }

        [HttpPut]
        public async Task<IActionResult> Put(long key, [FromBody] EmployeeRequestDto employeeRequest)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser =
                await _db.ClientUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            if (clientUser is null)
                return Forbidden();

            IQueryable<long> supplierQuery =
                from clientUserSetting in _db.ClientUserPermissions
                where clientUserSetting.ClientUserId == clientUser.Id
                from clientSupplier in _db.ClientSuppliers
                where clientSupplier.ClientId == clientUserSetting.ClientId
                from supplier in _db.Suppliers
                where supplier.Id == employeeRequest.SupplierId && supplier.Id == clientSupplier.SupplierId
                select supplier.Id;

            if (!await supplierQuery.AnyAsync())
            {
                // Supplier is not related to requesting client or does not exist!
                return BadRequest();
            }

            Employee employee = _mapper.Map<Employee>(employeeRequest);
            employee.Id = key;

            Employee storedEmployee = await _db.Employees.AsNoTracking().SingleOrDefaultAsync(x => x.Id == key);

            if (storedEmployee is null)
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<Employee>(key));

            employee.PostalAddress = null;
            employee.PostalAddressId = storedEmployee.PostalAddressId;
            employee.Creation = null;
            employee.CreationId = storedEmployee.CreationId;
            employee.LastModificationId = storedEmployee.LastModificationId;

            if (employeeRequest.PostalAddress is not null)
                employee.PostalAddressId =
                    await _addressResolutionService.GetOrAddPostalAddress(employeeRequest.PostalAddress);

            // TODO: pass a reason value into the endpoint's parameter DTO and specify it here inside the modification record.
            employee.LastModification = new RecordModification
            {
                Id = storedEmployee.LastModificationId ?? 0,
                ModifiedById = userId.Value,
                ModifiedOn = DateTime.Now
            };

            _db.Employees.Update(employee);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(employee);
        }
    }
}