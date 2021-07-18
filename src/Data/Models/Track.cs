using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Jaxofy.Data.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Jaxofy.Data.Models
{
    public partial class Track
    {
        public string PathToTrack(IConfiguration configuration) 
            => Path.Combine(configuration["BasePath"], FileName);
    }
    
    [Table("Track")]
    [Index(nameof(SongGuid), IsUnique = true)]
    public partial class Track : Entity
    {
        [JsonProperty("title")]
        [Column("Title", Order = 1)]
        public string Title { get; set; }
        
        [Column("FileName", Order = 2)]
        public string FileName { get; set; }

        [Column("Duration", Order = 3)]
        public double? Duration { get; set; }
        
        [Column("SongGuid")]
        public Guid SongGuid { get; set; }
        
        [Column("LastUpdated")]
        public DateTime? LastUpdated { get; set; }
        
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        
        [Column("FileSize")]
        public long FileSize { get; set; }
        
        // TODO: remove and change to n-m-relation with ICollection<Artist>
        [Column("ArtistName")]
        [JsonProperty("artistName")]
        public string ArtistName { get; set; }

        [NotMapped]
        [JsonProperty("url")]
        public string Url => Path.Combine("http://localhost:8000/api/v1/play", SongGuid.ToString());
    }
}