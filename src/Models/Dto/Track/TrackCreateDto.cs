using Microsoft.AspNetCore.Http;

namespace Jaxofy.Models.Dto.Track
{
    public class TrackCreateDto
    {
        public IFormFile FormFile { get; set; }
        public byte[] Data { get; set; }
        public string Title { get; set; }
        public string ArtistName { get; set; }
    }
}