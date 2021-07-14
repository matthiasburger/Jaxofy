using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Assignment;
using DasTeamRevolution.Models.Dto.Order;
using DasTeamRevolution.Services.AuthTokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;
using IronSphere.Extensions;

namespace DasTeamRevolution.Controllers
{
    /// <summary>
    /// <see cref="Order"/> endpoints.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController : ApiBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthTokenService _authTokenService;
        private readonly IMapper _mapper;

        public OrdersController(ApplicationDbContext db, IAuthTokenService authTokenService, IMapper mapper)
        {
            _db = db;
            _authTokenService = authTokenService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResponseDto>>> Get(ODataQueryOptions<Order> query)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();
            SupplierUser supplierUser = await _db.SupplierUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            IQueryable<Order> orderQuery;

            if (clientUser is not null)
            {
                orderQuery =
                    from clientUserSetting in _db.ClientUserPermissions
                    where clientUserSetting.ClientUserId == clientUser.Id
                    from client in _db.Clients
                    where client.Id == clientUserSetting.ClientId
                    from order in _db.Orders
                        .Include(o => o.Creation)
                        .Include(o => o.LastModification)
                        .Include(o => o.Assignments)
                    where order.ClientId == client.Id
                    select order;
            }
            else if (supplierUser is not null)
            {
                orderQuery =
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
                        .Include(o => o.Assignments)
                    where order.ClientId == client.Id
                    select order;
            }
            else
            {
                return Forbidden();
            }

            IEnumerable<Order> orders = (IEnumerable<Order>) query.ApplyTo(orderQuery);

            return EnvelopeResult.Ok(_mapper.Map<IEnumerable<OrderResponseDto>>(orders));
        }

        [HttpGet]
        [Route("get-orders-by-employee/{employeeId}")]
        public async Task<IActionResult> GetOrderByEmployee(long employeeId)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();
            SupplierUser supplierUser = await _db.SupplierUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            IQueryable<Order> orderQuery;

            if (clientUser is not null)
            {
                orderQuery =
                    from clientUserSetting in _db.ClientUserPermissions
                    where clientUserSetting.ClientUserId == clientUser.Id
                    from client in _db.Clients
                    where client.Id == clientUserSetting.ClientId
                    from order in _db.Orders
                        .Include(o => o.Creation)
                        .Include(o => o.LastModification)
                        .Include(o => o.Assignments)
                    where order.ClientId == client.Id
                    from assignment in _db.Assignments
                    where assignment.OrderId == order.Id
                    from employee in _db.Employees
                    where employee.Assignments.Contains(assignment)
                    select order;
            }
            else if (supplierUser is not null)
            {
                orderQuery =
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
                        .Include(o => o.Assignments)
                    where order.ClientId == client.Id
                    from assignment in _db.Assignments
                    where assignment.OrderId == order.Id
                    from employee in _db.Employees
                    where employee.Assignments.Contains(assignment)
                    select order;
            }
            else
            {
                return Forbidden();
            }

            return EnvelopeResult.Ok(_mapper.Map<IEnumerable<OrderResponseDto>>(await orderQuery.ToListAsync()));
        }

        [HttpGet]
        [Route("{key}")]
        public async Task<IActionResult> GetById(long key)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();
            SupplierUser supplierUser = await _db.SupplierUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            IQueryable<Order> orderQuery;

            if (clientUser is not null)
            {
                orderQuery =
                    from clientUserSetting in _db.ClientUserPermissions
                    where clientUserSetting.ClientUserId == clientUser.Id
                    from client in _db.Clients
                    where client.Id == clientUserSetting.ClientId
                    from order in _db.Orders
                        .Include(o => o.Creation)
                        .Include(o => o.LastModification)
                        .Include(o => o.Assignments)
                    where order.ClientId == client.Id && order.Id == key
                    select order;
            }
            else if (supplierUser is not null)
            {
                orderQuery =
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
                        .Include(o => o.Assignments)
                    where order.ClientId == client.Id && order.Id == key
                    select order;
            }
            else
            {
                return Forbidden();
            }

            Order foundOrder = await orderQuery.SingleOrDefaultAsync();

            return foundOrder switch
            {
                null => Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<Order>(key)),
                _ => EnvelopeResult.Ok(_mapper.Map<OrderResponseDto>(foundOrder))
            };
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequestDto orderRequest)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers
                .Where(u => u.ApplicationUserId == userId)
                .FirstOrDefaultAsync();

            if (clientUser is null)
                return Forbidden();

            IQueryable<Client> clientQuery =
                from clientUserSetting in _db.ClientUserPermissions
                where clientUserSetting.ClientUserId == clientUser.Id
                from client in _db.Clients
                where client.Id == clientUserSetting.ClientId
                select client;

            if (!await clientQuery.AnyAsync(c => c.Id == orderRequest.ClientId))
                return BadRequest();

            Order order = new()
            {
                ClientId = orderRequest.ClientId,
                LastModification = null,
                Creation = new RecordCreation(userId.Value)
            };

            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();

            if (!orderRequest.LinkAssignments.IsNullOrEmpty())
            {
                foreach (Assignment assignment in await _db.Assignments.Where(a => orderRequest.LinkAssignments.Contains(a.Id)).ToListAsync())
                {
                    assignment.OrderId = order.Id;
                }

                await _db.SaveChangesAsync();
                order = await _db.Orders.SingleOrDefaultAsync(o => o.Id == order.Id);
            }

            if (!orderRequest.CreateAssignments.IsNullOrEmpty())
            {
                foreach (AssignmentRequestDto assignmentDto in orderRequest.CreateAssignments)
                {
                    Assignment assignment = _mapper.Map<Assignment>(assignmentDto);
                    assignment.OrderId = order.Id;
                    await _db.Assignments.AddAsync(assignment);
                }

                await _db.SaveChangesAsync();
                order = await _db.Orders.SingleOrDefaultAsync(o => o.Id == order.Id);
            }

            if (!orderRequest.CreateFromProposals.IsNullOrEmpty())
            {
                foreach (Proposal proposal in await _db.Proposals.Where(p => orderRequest.CreateFromProposals.Contains(p.Id)).ToListAsync())
                {
                    await _db.Assignments.AddAsync(new Assignment
                    {
                        OrderId = order.Id
                    });
                }

                await _db.SaveChangesAsync();
                order = await _db.Orders.SingleOrDefaultAsync(o => o.Id == order.Id);
            }

            return EnvelopeResult.Created($"/api/v1/orders/{order.Id}", order);
        }
    }
}