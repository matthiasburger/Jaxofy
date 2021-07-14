using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.EmployeeDocument;
using DasTeamRevolution.Services.AuthTokenService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Controllers
{
    /// <summary>
    /// Controller for uploading employee documents.<para> </para>
    /// <a href="https://stackoverflow.com/a/51388106">Angular-to-C# backend IFormFile &lt;&gt; File interop (StackOverflow link)</a>
    /// </summary>
    /// <seealso cref="EmployeeDocument"/>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeeDocumentsController : ApiBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthTokenService _authTokenService;

        public EmployeeDocumentsController(ApplicationDbContext db, IAuthTokenService authTokenService)
        {
            _db = db;
            _authTokenService = authTokenService;
        }

        /// <summary>
        /// Gets all the <see cref="EmployeeDocument"/>s that belong to a given <see cref="Employee"/> and the requesting <see cref="Client"/>.
        /// </summary>
        /// <param name="dto"><see cref="ListEmployeeDocumentsRequestDto"/> for specifying the <see cref="Employee"/>.<see cref="Employee.Id"/></param>
        /// <returns><see cref="EmployeeDocument"/></returns>
        [HttpGet, Authorize, Route("")]
        public async Task<IActionResult> GetAll([FromQuery] ListEmployeeDocumentsRequestDto dto)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers
                .Include(u => u.ApplicationUser)
                .Where(u => u.ApplicationUserId == userId && u.ApplicationUser.IsActive)
                .FirstOrDefaultAsync();

            if (clientUser is null)
                return Forbidden();

            IQueryable<Tuple<long, string>> documentsQuery =
                from clientUserSetting in _db.ClientUserPermissions
                where clientUserSetting.ClientUserId == clientUser.Id
                from client in _db.Clients
                where client.Id == clientUserSetting.ClientId
                from employee in _db.Employees
                where employee.ClientId == client.Id && employee.Id == dto.EmployeeId
                from employeeDocument in _db.EmployeeDocuments
                    .Include(d => d.Creation)
                    .Include(d => d.LastModification)
                where employeeDocument.EmployeeId == employee.Id
                select new Tuple<long, string>(employeeDocument.Id, employeeDocument.DocumentName);

            return Ok(1, new[]
            {
                new ListEmployeeDocumentsResponseDto
                {
                    DocumentIdNameTuples = await documentsQuery.ToListAsync()
                }
            });
        }

        /// <summary>
        /// Gets a <see cref="EmployeeDocument"/> from the db using a specific <paramref name="id"/>.<para> </para>
        /// IMPORTANT: 
        /// The request fails if the requesting user is not an authorized <see cref="ClientUser"/> or the document's <see cref="EmployeeDocument.Employee"/> does not belong to the requesting client.
        /// </summary>
        /// <param name="id">Document id (<see cref="EmployeeDocument"/>)</param>
        [HttpGet, Authorize, Route("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers
                .Include(u => u.ApplicationUser)
                .Where(u => u.ApplicationUserId == userId && u.ApplicationUser.IsActive)
                .FirstOrDefaultAsync();

            if (clientUser is null)
                return Forbidden();

            IQueryable<EmployeeDocument> documentQuery =
                from clientUserSetting in _db.ClientUserPermissions
                where clientUserSetting.ClientUserId == clientUser.Id
                from client in _db.Clients
                where client.Id == clientUserSetting.ClientId
                from employee in _db.Employees
                where employee.ClientId == client.Id
                from employeeDocument in _db.EmployeeDocuments
                    .Include(d => d.Creation)
                    .Include(d => d.LastModification)
                where employeeDocument.EmployeeId == employee.Id && employeeDocument.Id == id
                select employeeDocument;

            EmployeeDocument doc = await documentQuery.FirstOrDefaultAsync();

            return doc is not null ? Ok(1, new[] { doc }) : Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<EmployeeDocument>(id));
        }

        /// <summary>
        /// Endpoint for uploading a <see cref="EmployeeDocument"/>.<para> </para>
        /// IMPORTANT:
        /// The request will fail if the provided <paramref name="dto"/>'s <see cref="EmployeeDocumentUploadRequestDto.EmployeeId"/>
        /// does not belong to the requesting <see cref="Client"/>'s employees.
        /// </summary>
        /// <param name="dto">Upload request DTO containing the related <see cref="Employee"/>'s ID + the document file.</param>
        [HttpPost, Authorize, Route("upload")]
        public async Task<IActionResult> Upload([FromBody] EmployeeDocumentUploadRequestDto dto)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers
                .Include(u => u.ApplicationUser)
                .Where(u => u.ApplicationUserId == userId && u.ApplicationUser.IsActive)
                .FirstOrDefaultAsync();

            if (clientUser is null)
                return Forbidden();

            IQueryable<Employee> employeeQuery =
                from clientUserSetting in _db.ClientUserPermissions
                where clientUserSetting.ClientUserId == clientUser.Id
                from client in _db.Clients
                where client.Id == clientUserSetting.ClientId
                from _employee in _db.Employees
                where _employee.ClientId == client.Id && _employee.Id == dto.EmployeeId
                select _employee;

            Employee employee = await employeeQuery.FirstOrDefaultAsync();

            if (employee is null)
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<Employee>(dto.EmployeeId));

            DateTime utcNow = DateTime.UtcNow;

            // await using MemoryStream ms = new();
            // await dto.Document.CopyToAsync(ms);

            EmployeeDocument doc = new()
            {
                Employee = employee,
                EmployeeId = employee.Id,
                DocumentBytes = dto.Document.Content,
                DocumentName = dto.Document.FileName,
                Creation = new RecordCreation { CreatedById = clientUser.ApplicationUserId, CreatedOn = utcNow }
            };

            await _db.EmployeeDocuments.AddAsync(doc);

            bool success = await _db.SaveChangesAsync() > 0;

            return success ? Created($"/api/v1/employeedocuments/{doc.Id}", 1, new[] { new EmployeeDocumentUploadResponseDto { DocumentId = doc.Id } }) : InternalServerError();
        }
    }
}