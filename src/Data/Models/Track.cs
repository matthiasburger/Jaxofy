using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [JsonProperty("songGuid")]
        public Guid SongGuid { get; set; }
        
        [Column("LastUpdated")]
        public DateTime? LastUpdated { get; set; }
        
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        
        [Column("FileSize")]
        public long FileSize { get; set; }

        [NotMapped]
        [JsonProperty("url")]
        public string Url => Path.Combine("play", SongGuid.ToString());
        
        public ICollection<TrackArtist> TrackArtists { get; set; } = new List<TrackArtist>();
    }

    public class TrackArtist
    {
        public Artist Artist { get; set; }

        public Track Track { get; set; }
        
        public Guid ArtistId { get; set; }

        public long TrackId { get; set; }
    }

    public class Artist : IEntity<Guid>
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public long ArtistId { get; set; }

        public string Name { get; set; }

        public ICollection<TrackArtist> TrackArtists { get; set; } = new List<TrackArtist>();
    }
}