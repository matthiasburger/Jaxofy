using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Services.AuthTokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Controllers
{
    /// <summary>
    /// Controller for getting single documents off of a proposal.<para> </para>
    /// <a href="https://stackoverflow.com/a/51388106">Angular-to-C# backend IFormFile &lt;&gt; File interop (StackOverflow link)</a>
    /// </summary>
    /// <seealso cref="ProposalDocument"/>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProposalDocumentsController : ApiBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthTokenService _authTokenService;

        public ProposalDocumentsController(ApplicationDbContext db, IAuthTokenService authTokenService)
        {
            _db = db;
            _authTokenService = authTokenService;
        }

        /// <summary>
        /// Gets a <see cref="ProposalDocument"/> from the db using a specific <paramref name="id"/>.<para> </para>
        /// IMPORTANT: 
        /// The request fails if the document's <see cref="Proposal"/> does not belong to the requesting client/supplier.
        /// </summary>
        /// <param name="id">Document id (<see cref="ProposalDocument"/>)</param>
        [HttpGet, Authorize, Route("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Forbidden();

            ClientUser clientUser = await _db.ClientUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();
            SupplierUser supplierUser = await _db.SupplierUsers.Where(u => u.ApplicationUserId == userId).FirstOrDefaultAsync();

            IQueryable<ProposalDocument> docQuery;

            if (clientUser is not null)
            {
                docQuery =
                    from clientUserSetting in _db.ClientUserPermissions
                    where clientUserSetting.ClientUserId == clientUser.Id
                    from client in _db.Clients
                    where client.Id == clientUserSetting.ClientId
                    from vacancy in _db.Vacancies
                    where vacancy.ClientId == client.Id
                    from proposal in _db.Proposals
                    where proposal.VacancyId == vacancy.Id
                    from document in _db.ProposalDocuments
                    where document.ProposalId == proposal.Id && document.Id == id
                    select document;
            }
            else if (supplierUser is not null)
            {
                docQuery =
                    from supplierUserSetting in _db.SupplierUserPermissions
                    where supplierUserSetting.SupplierUserId == supplierUser.Id
                    from supplier in _db.Suppliers
                    where supplier.Id == supplierUserSetting.SupplierId
                    from _sup in _db.SupplierUserPermissions
                    where _sup.SupplierId == supplier.Id
                    from _su in _db.SupplierUsers
                    where _su.Id == _sup.SupplierUserId
                    from proposal in _db.Proposals
                    where proposal.SupplierUserId == _su.Id
                    from document in _db.ProposalDocuments
                    where document.ProposalId == proposal.Id && document.Id == id
                    select document;
            }
            else
            {
                return Forbidden();
            }

            ProposalDocument doc = await docQuery.FirstOrDefaultAsync();

            return doc is not null ? Ok(1, new[] { doc }) : Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<ProposalDocument>(id));
        }
    }
}