using System.Threading.Tasks;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.PostalAddress;

namespace DasTeamRevolution.Services.AddressResolution
{
    /// <summary>
    /// Address resolution service for either fetching an already existing PostalAddress form the DB or inserting it on the fly.
    /// </summary>
    public interface IAddressResolutionService
    {
        /// <summary>
        /// Checks if a given <see cref="PostalAddress"/> exists in the DB with the passed <paramref name="dto"/>'s fields (e.g. name, address, ZIP code, etc...). <para> </para>
        /// If the address exists, its primary key is returned. If not, the address is created and the created row's primary key is returned. Either way, you're getting an ID back.
        /// </summary>
        /// <param name="dto">Address data to lookup or create.</param>
        /// <returns>The found or inserted row's ID.</returns>
        Task<long> GetOrAddPostalAddress(PostalAddressRequestDto dto);
    }
}