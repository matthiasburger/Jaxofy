using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Assignment;
using DasTeamRevolution.Services.AuthTokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AssignmentsController : ApiBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IAuthTokenService _authTokenService;

        public AssignmentsController(ApplicationDbContext db, IMapper mapper, IAuthTokenService authTokenService)
        {
            _db = db;
            _mapper = mapper;
            _authTokenService = authTokenService;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Get(ODataQueryOptions<Assignment> query)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser =
                await _db.ClientUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();
            SupplierUser supplierUser =
                await _db.SupplierUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            IQueryable<Assignment> assignmentQuery;

            if (clientUser is not null)
            {
                assignmentQuery =
                    from clientUserSetting in _db.ClientUserPermissions
                    where clientUserSetting.ClientUserId == clientUser.Id
                    from client in _db.Clients
                    where client.Id == clientUserSetting.ClientId
                    from order in _db.Orders
                        .Include(o => o.Creation)
                        .Include(o => o.LastModification)
                    where order.ClientId == client.Id
                    from assignment in _db.Assignments
                        .Include(x => x.Employee)
                        .Include(x => x.Order)
                        .ThenInclude(x => x.Client)
                        .ThenInclude(x => x.Group)
                    where assignment.OrderId == order.Id
                    select assignment;
            }
            else if (supplierUser is not null)
            {
                assignmentQuery =
                    from supplierUserSetting in _db.SupplierUserPermissions
                    where supplierUserSetting.SupplierUserId == supplierUser.Id
                    from supplier in _db.Suppliers
                    where supplier.Id == supplierUserSetting.SupplierId
                    from clientSupplier in _db.ClientSuppliers
                    where clientSupplier.SupplierId == supplier.Id
                    from client in _db.Clients
                    where client.Id == clientSupplier.ClientId
                    from order in _db.Orders
                        .Include(o => o.Creation)
                        .Include(o => o.LastModification)
                    where order.ClientId == client.Id
                    from assignment in _db.Assignments
                        .Include(x => x.Employee)
                        .Include(x => x.Order)
                        .ThenInclude(x => x.Client)
                        .ThenInclude(x => x.Group)
                    where assignment.OrderId == order.Id
                    select assignment;
            }
            else
            {
                return Forbidden();
            }

            IQueryable result = query.ApplyTo(assignmentQuery);

            return EnvelopeResult.Ok(
                (IEnumerable<AssignmentResponseDto>) _mapper.Map<List<AssignmentResponseDto>>(result));
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(long key)
        {
            Assignment assignment =
                await _db.Assignments
                    .Include(x => x.Employee)
                    .Include(x => x.Order)
                    .ThenInclude(x => x.Client)
                    .ThenInclude(x => x.Group)
                    .Include(x => x.Order)
                    .ThenInclude(x => x.Client)
                    .ThenInclude(x => x.Group)
                    .FirstOrDefaultAsync(x => x.Id == key);

            return EnvelopeResult.Ok(_mapper.Map<AssignmentResponseDto>(assignment));
        }

        [HttpGet, Route("get-assignments-by-poolemployee/{id}")]
        public async Task<IActionResult> GetAssignmentsByPoolEmployeeId(long id)
        {
            List<Assignment> assignments = await (
                from assignment in _db.Assignments
                join employee in _db.Employees 
                    on assignment.EmployeeId equals employee.Id
                join poolEmployee in _db.PoolEmployees 
                    on employee.PoolEmployeeId equals poolEmployee.Id
                where poolEmployee.Id == id
                select assignment
            ).ToListAsync();

            return EnvelopeResult.Ok(_mapper.Map<List<AssignmentResponseDto>>(assignments));
        }

        [HttpGet, Route("get-assignments-by-order/{id}")]
        public async Task<IActionResult> GetAssignmentsByOrderId(long id)
        {
            List<Assignment> assignments = await (from assignment in _db.Assignments
                where assignment.OrderId == id
                select assignment).ToListAsync();

            return EnvelopeResult.Ok(_mapper.Map<List<AssignmentResponseDto>>(assignments));
        }

        [HttpGet]
        [Route("get-assignments-by-employee/{id}")]
        public async Task<IActionResult> GetAssignmentByEmployee(long id)
        {
            IQueryable<Assignment> assignments = from assignment in _db.Assignments
                where assignment.EmployeeId == id
                select assignment;

            return EnvelopeResult.Ok(_mapper.Map<List<AssignmentResponseDto>>(await assignments.ToListAsync()));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AssignmentRequestDto assignmentRequest)
        {
            Assignment assignment =
                _mapper.Map<Assignment>(assignmentRequest);

            await _db.Assignments.AddAsync(assignment);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Created($"/api/v1/Assignments/{assignment.Id}", assignment);
        }

        [HttpPut("{key}")]
        public async Task<ActionResult> Put(long key,
            [FromBody] AssignmentRequestDto assignmentRequest)
        {
            Assignment assignment =
                _mapper.Map<Assignment>(assignmentRequest);
            assignment.Id = key;

            _db.Assignments.Update(assignment);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(assignment);
        }
    }
}