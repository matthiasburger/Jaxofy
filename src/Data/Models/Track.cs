using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Jaxofy.Data.Models.Base;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Jaxofy.Data.Models
{
    [Table("Track")]
    public class Track : Entity
    {
        public string PathToTrack(IConfiguration configuration) 
            => Path.Combine(configuration["BasePath"], FileName);

        public double? Duration { get; set; }
        public string FileName { get; set; }
        public Guid RoadieId { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime CreatedDate { get; set; }
        public long FileSize { get; set; }

        public long GetFileSize(IConfiguration configuration)
        {
            return FileSize > 0 ? FileSize : new FileInfo(PathToTrack(configuration)).Length;
        }
        
        // TODO: remove and change to n-m-relation with ICollection<Artist>
        [JsonProperty("artistName")]
        public string ArtistName { get; set; }

        [NotMapped]
        [JsonProperty("url")]
        public string Url => Path.Combine("http://localhost:8000/api/v1/play", RoadieId.ToString());
    }
}