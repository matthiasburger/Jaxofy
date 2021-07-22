using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Id3;

using Jaxofy.Controllers.Base;
using Jaxofy.Data.Models;
using Jaxofy.Data.Repositories;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace Jaxofy.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TracksController : ApiBaseController
    {
        private readonly ITrackRepository _trackRepository;
        private readonly IConfiguration _configuration;

        public TracksController(ITrackRepository trackRepository, IConfiguration configuration)
        {
            _trackRepository = trackRepository;
            _configuration = configuration;
        }

        [HttpGet, Route("")]
        public ActionResult Get(ODataQueryOptions<Track> query)
        {
            IQueryable tracks = query.ApplyTo(_trackRepository.GetQueryable());
            return Ok(tracks);
        }

        [HttpGet, Route("{id:long}")]
        public async Task<ActionResult> Get(long id)
        {
            Track track = await _trackRepository.SingleOrDefaultNoTracking(x => x.Id == id);
            return Ok(track);
        }

        [HttpPost, Route("")]
        public async Task<ActionResult> Insert([FromForm] IFormFile formFile)
        {
            Guid songGuid = Guid.NewGuid();

            string filePath = Path.Combine(_configuration["BasePath"], songGuid.ToString());

            await using MemoryStream memS = new();
            await formFile.CopyToAsync(memS);

            byte[] data = memS.ToArray();

            await System.IO.File.WriteAllBytesAsync(filePath, data);

            int lengthInMilliseconds = 0;

            Id3Tag tag;
            using (Mp3 mp3 = new(filePath))
            {
                tag = mp3.GetTag(Id3TagFamily.Version2X) ?? mp3.GetTag(Id3TagFamily.Version1X);
                if (tag != null)
                    lengthInMilliseconds = tag.Length.Value.Milliseconds;
            }

            Track track = new()
            {
                Title = tag?.Title ?? formFile.FileName,
                ArtistName = @"keiner",
                FileName = songGuid.ToString(),
                SongGuid = songGuid,
                CreatedDate = DateTime.Now,
                FileSize = data.LongLength,
                LastUpdated = null,
                Duration = lengthInMilliseconds
            };

            (bool success, EntityEntry<Track> entity) = await _trackRepository.Add(track);

            if (!success)
                return InternalServerError();

            return EnvelopeResult.Created(
                Url.Action("Get", "Tracks", new {id = entity.Entity.Id}), 
                entity.Entity);
        }
    }
}