using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.PostalAddress;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Services.AddressResolution
{
    public class AddressResolutionService : IAddressResolutionService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;

        public AddressResolutionService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<long> GetOrAddPostalAddress(PostalAddressRequestDto dto)
        {
            PostalAddress existingAddress = await _db.PostalAddresses
                .AsNoTracking()
                .Where(p =>
                    p.CountryCodeISO == dto.CountryCodeISO /////
                    && p.PostalZipCode == dto.PostalZipCode ////
                    && p.PostalCity == dto.PostalCity //////////
                    && p.PostalStreet == dto.PostalStreet //////
                    && p.PostalName == dto.PostalName)
                .FirstOrDefaultAsync();

            if (existingAddress is not null)
                return existingAddress.Id;

            existingAddress = _mapper.Map<PostalAddress>(dto);
            await _db.AddAsync(existingAddress);
            await _db.SaveChangesAsync();

            return existingAddress.Id;
        }
    }
}