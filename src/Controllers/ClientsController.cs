using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Client;
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
    public class ClientsController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IAuthTokenService _authTokenService;
        private readonly IAddressResolutionService _addressResolutionService;

        public ClientsController(ApplicationDbContext db, IMapper mapper, IAuthTokenService authTokenService, IAddressResolutionService addressResolutionService)
        {
            _db = db;
            _mapper = mapper;
            _authTokenService = authTokenService;
            _addressResolutionService = addressResolutionService;
        }

        [HttpGet]
        public ActionResult<List<ClientResponseDto>> Get(ODataQueryOptions<Client> query)
        {
            IQueryable clients = query.ApplyTo(_db.Clients
                .Include(v => v.PostalAddress)
                .Include(v => v.Creation)
                .Include(v => v.LastModification));

            return EnvelopeResult.Ok((IEnumerable<ClientResponseDto>)_mapper.Map<List<ClientResponseDto>>(clients));
        }

        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            Client client = await _db.Clients
                .Include(v => v.PostalAddress)
                .Include(v => v.Creation)
                .Include(v => v.LastModification)
                .SingleOrDefaultAsync(x => x.Id == key);

            if (client is null)
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<Client>(key));

            return EnvelopeResult.Ok(_mapper.Map<ClientResponseDto>(client));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ClientRequestDto clientRequest)
        {
            Client client = _mapper.Map<Client>(clientRequest);

            long? userId = _authTokenService.ExtractUserId(HttpContext);
            if (!userId.HasValue)
            {
                return Forbidden();
            }

            client.Creation = new RecordCreation
            {
                CreatedById = userId.Value,
                CreatedOn = DateTime.Now
            };

            if (client.PostalAddress is not null)
            {
                long addressId = await _addressResolutionService.GetOrAddPostalAddress(client.PostalAddress);
                
                client.PostalAddress = null;
                client.PostalAddressId = addressId;
            }

            await _db.Clients.AddAsync(client);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Created($"/api/v1/clients/{client.Id}", _mapper.Map<ClientResponseDto>(client));
        }

        [HttpPut]
        public async Task<ActionResult> Put(long key, [FromBody] ClientRequestDto clientRequest)
        {
            Client client = _mapper.Map<Client>(clientRequest);
            client.Id = key;

            Client storedClient = await _db.Clients
                .AsNoTracking()
                .Include(c => c.PostalAddress)
                .SingleOrDefaultAsync(x => x.Id == key);

            client.CreationId = storedClient.CreationId;
            client.LastModificationId = storedClient.LastModificationId;

            long? userId = _authTokenService.ExtractUserId(HttpContext);
            if (!userId.HasValue)
            {
                return Forbidden();
            }

            if (client.PostalAddress is not null && !client.PostalAddress.Equals(storedClient.PostalAddress))
            {
                long existingAddress = await _db.PostalAddresses
                    .AsNoTracking()
                    .Where(p =>
                        p.CountryCodeISO == client.PostalAddress.CountryCodeISO /////
                        && p.PostalZipCode == client.PostalAddress.PostalZipCode ////
                        && p.PostalCity == client.PostalAddress.PostalCity //////////
                        && p.PostalStreet == client.PostalAddress.PostalStreet //////
                        && p.PostalName == client.PostalAddress.PostalName)
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync();

                if (existingAddress != 0)
                {
                    client.PostalAddress = null;
                    client.PostalAddressId = existingAddress;
                }
                else
                {
                    _db.PostalAddresses.Update(client.PostalAddress);
                }
            }

            client.LastModification = new RecordModification
            {
                Id = storedClient.LastModificationId ?? 0,
                ModifiedById = userId,
                ModifiedOn = DateTime.Now
            };

            _db.Clients.Update(client);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(_mapper.Map<ClientResponseDto>(client));
        }
    }
}