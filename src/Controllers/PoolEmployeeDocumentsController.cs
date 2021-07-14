using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.PoolEmployeeDocument;
using DasTeamRevolution.Services.AuthTokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Controllers
{
    /// <summary>
    /// Controller for uploading pool employee documents.<para> </para>
    /// <a href="https://stackoverflow.com/a/51388106">Angular-to-C# backend IFormFile &lt;&gt; File interop (StackOverflow link)</a>
    /// </summary>
    /// <seealso cref="PoolEmployeeDocument"/>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PoolEmployeeDocumentsController : ApiBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthTokenService _authTokenService;

        public PoolEmployeeDocumentsController(ApplicationDbContext db, IAuthTokenService authTokenService)
        {
            _db = db;
            _authTokenService = authTokenService;
        }

        [HttpGet, Authorize, Route("")]
        public async Task<IActionResult> GetAll([FromQuery] ListPoolEmployeeDocumentsRequestDto dto)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            SupplierUser supplierUser = await _db.SupplierUsers
                .Include(u => u.ApplicationUser)
                .Where(u => u.ApplicationUserId == userId && u.ApplicationUser.IsActive)
                .FirstOrDefaultAsync();

            if (supplierUser is null)
                return Forbidden();

            IQueryable<Tuple<long, string>> documentsQuery =
                from supplierUserSetting in _db.SupplierUserPermissions
                where supplierUserSetting.SupplierUserId == supplierUser.Id
                from supplier in _db.Suppliers
                where supplier.Id == supplierUserSetting.SupplierId
                from poolEmployee in _db.PoolEmployees
                where poolEmployee.SupplierId == supplier.Id
                from poolEmployeeDocument in _db.PoolEmployeeDocuments
                    .Include(d => d.Creation)
                    .Include(d => d.LastModification)
                where poolEmployeeDocument.PoolEmployeeId == dto.PoolEmployeeId
                select new Tuple<long, string>(poolEmployeeDocument.Id, poolEmployeeDocument.DocumentName);

            return Ok(1, new[]
            {
                new ListPoolEmployeeDocumentsResponseDto
                {
                    DocumentIdNameTuples = await documentsQuery.ToListAsync()
                }
            });
        }

        /// <summary>
        /// Gets a <see cref="PoolEmployeeDocument"/> from the db using a specific <paramref name="id"/>.<para> </para>
        /// IMPORTANT: 
        /// The request fails if the requesting user is not an authorized <see cref="SupplierUser"/> or the document's <see cref="PoolEmployeeDocument.PoolEmployee"/> does not belong to the requesting supplier.
        /// </summary>
        /// <param name="id">Document id (<see cref="PoolEmployeeDocument"/>)</param>
        [HttpGet, Authorize, Route("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            SupplierUser supplierUser = await _db.SupplierUsers
                .Include(u => u.ApplicationUser)
                .Where(u => u.ApplicationUserId == userId && u.ApplicationUser.IsActive)
                .FirstOrDefaultAsync();

            if (supplierUser is null)
                return Forbidden();

            IQueryable<PoolEmployeeDocument> documentQuery =
                from supplierUserSetting in _db.SupplierUserPermissions
                where supplierUserSetting.SupplierUserId == supplierUser.Id
                from supplier in _db.Suppliers
                where supplier.Id == supplierUserSetting.SupplierId
                from poolEmployee in _db.PoolEmployees
                where poolEmployee.SupplierId == supplier.Id
                from poolEmployeeDocument in _db.PoolEmployeeDocuments
                    .Include(d => d.Creation)
                    .Include(d => d.LastModification)
                where poolEmployeeDocument.Id == id && poolEmployeeDocument.PoolEmployeeId == poolEmployee.Id
                select poolEmployeeDocument;

            PoolEmployeeDocument doc = await documentQuery.FirstOrDefaultAsync();

            return doc is not null ? Ok(1, new[] { doc }) : Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<PoolEmployeeDocument>(id));
        }

        /// <summary>
        /// Endpoint for uploading a <see cref="PoolEmployeeDocument"/>.<para> </para>
        /// IMPORTANT:
        /// The request will fail if the provided <paramref name="dto"/>'s <see cref="PoolEmployeeDocumentUploadRequestDto.PoolEmployeeId"/>
        /// does not belong to the requesting <see cref="Supplier"/>'s pool of employees.
        /// </summary>
        /// <param name="dto">Upload request DTO containing the related <see cref="PoolEmployee"/>'s ID + the document file.</param>
        [HttpPost, Authorize, Route("upload")]
        public async Task<IActionResult> Upload([FromBody] PoolEmployeeDocumentUploadRequestDto dto)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            SupplierUser supplierUser = await _db.SupplierUsers
                .Include(u => u.ApplicationUser)
                .Where(u => u.ApplicationUserId == userId && u.ApplicationUser.IsActive)
                .FirstOrDefaultAsync();

            if (supplierUser is null)
                return Forbidden();

            IQueryable<PoolEmployee> poolEmployeeQuery =
                from supplierUserSetting in _db.SupplierUserPermissions
                where supplierUserSetting.SupplierUserId == supplierUser.Id
                from supplier in _db.Suppliers
                where supplier.Id == supplierUserSetting.SupplierId
                from poolEmployee in _db.PoolEmployees
                where poolEmployee.Id == dto.PoolEmployeeId && poolEmployee.SupplierId == supplier.Id
                select poolEmployee;

            PoolEmployee employee = await poolEmployeeQuery.FirstOrDefaultAsync();

            if (employee is null)
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<PoolEmployee>(dto.PoolEmployeeId));

            DateTime utcNow = DateTime.UtcNow;

            // await using MemoryStream ms = new();
            // await dto.Document.CopyToAsync(ms);

            PoolEmployeeDocument doc = new()
            {
                PoolEmployee = employee,
                PoolEmployeeId = employee.Id,
                DocumentBytes = dto.Document.Content,
                DocumentName = dto.Document.FileName,
                Creation = new RecordCreation { CreatedBy = supplierUser.ApplicationUser, CreatedOn = utcNow }
            };

            await _db.PoolEmployeeDocuments.AddAsync(doc);

            bool success = await _db.SaveChangesAsync() > 0;

            return success ? Created($"/api/v1/poolemployeedocuments/{doc.Id}", 1, new[] { new PoolEmployeeDocumentUploadResponseDto { DocumentId = doc.Id } }) : InternalServerError();
        }
    }
}