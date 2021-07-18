using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Id3;
using Jaxofy.Controllers.Base;
using Jaxofy.Data;
using Jaxofy.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Configuration;

namespace Jaxofy.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TracksController : ApiBaseController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public TracksController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpGet, Route("")]
        public ActionResult Get(ODataQueryOptions<Track> query)
        {
            IQueryable tracks = query.ApplyTo(_dbContext.Tracks);
            return Ok(tracks);
        }

        [HttpPost, Route("")]
        public async Task<ActionResult> Insert([FromForm] IFormFile formFile)
        {
            Guid songGuid = Guid.NewGuid();

            string filePath = Path.Combine(_configuration["BasePath"], songGuid.ToString());

            await using MemoryStream memS = new ();
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
                ArtistName = "keiner",
                FileName = songGuid.ToString(),
                SongGuid = songGuid,
                CreatedDate = DateTime.Now,
                FileSize = data.LongLength,
                LastUpdated = null,
                Duration = lengthInMilliseconds
            };

            await _dbContext.Tracks.AddAsync(track);
            await _dbContext.SaveChangesAsync();

            return Ok(track);
        }
    }
}