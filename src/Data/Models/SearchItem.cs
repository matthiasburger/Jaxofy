using System;
using Jaxofy.Data.Models.Base;

namespace Jaxofy.Data.Models;

public class SearchItem : Entity
{
    public string Content { get; set; }

    public long? TrackId { get; set; }

    public Track Track { get; set; }

    public Guid? ArtistId { get; set; }

    public Artist Artist { get; set; }
    
    public long? SongCollectionId { get; set; }

    public SongCollection SongCollection { get; set; }
}