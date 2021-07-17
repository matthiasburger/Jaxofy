using System;
using System.Collections.Generic;

namespace Jaxofy.Services.TrackService.Models
{
    public static class MimeTypeHelper
    {
        private const string Mp3Extension = ".mp3";

        private static readonly Dictionary<string, string> AudioMimeTypes =
            new(StringComparer.OrdinalIgnoreCase)
            {
                {Mp3Extension, "audio/mpeg"},
                {".m4a", "audio/mp4"},
                {".aac", "audio/mp4"},
                {".webma", "audio/webm"},
                {".wav", "audio/wav"},
                {".wma", "audio/x-ms-wma"},
                {".ogg", "audio/ogg"},
                {".oga", "audio/ogg"},
                {".opus", "audio/ogg"},
                {".ac3", "audio/ac3"},
                {".dsf", "audio/dsf"},
                {".m4b", "audio/m4b"},
                {".xsp", "audio/xsp"},
                {".dsp", "audio/dsp"}
            };

        public static string Mp3MimeType => AudioMimeTypes[Mp3Extension];
    }
}