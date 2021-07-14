using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Jaxofy.Controllers.Base;
using Jaxofy.Data;
using Jaxofy.Data.Models;
using Jaxofy.Extensions;
using Jaxofy.Services.HttpEncoder;
using Jaxofy.Services.TrackService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;

namespace Jaxofy.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlayController : ApiBaseController
    {
        private readonly ApplicationDbContext _databaseContext;
        private readonly ITrackService _trackService;
        private readonly IHttpEncoder _httpEncoder;
        private readonly IConfiguration _configuration;

        public PlayController(ApplicationDbContext context, ITrackService trackService, IHttpEncoder httpEncoder,
            IConfiguration configuration)
        {
            _databaseContext = context;
            _trackService = trackService;
            _httpEncoder = httpEncoder;
            _configuration = configuration;
        }

        [HttpGet, Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> StreamTrack()
        {
            return await StreamTrack(Guid.NewGuid()).ConfigureAwait(false);
        }
        
        

        [HttpGet, Route("{trackGuid}")]
        [AllowAnonymous]
        public async Task<IActionResult> StreamTrack(Guid trackGuid)
        {
            return await StreamTrack(trackGuid, null).ConfigureAwait(false);
        }

        [NonAction]
        protected async Task<IActionResult> StreamTrack(Guid id, ApplicationUser currentUser = null)
        {
            var user = currentUser;
            OperationResult<Track> track = _trackService.StreamCheckAndInfo(user, id);
            
            var info = await _trackService.TrackStreamInfoAsync(id,
                _trackService.DetermineByteStartFromHeaders(Request.Headers),
                _trackService.DetermineByteEndFromHeaders(Request.Headers, track.Data.GetFileSize(_configuration)),
                user).ConfigureAwait(false);

            Response.Headers.Add("Content-Disposition", info.Data.ContentDisposition);
            Response.Headers.Add("X-Content-Duration", info.Data.ContentDuration);
            Response.Headers.Add("Content-Duration", info.Data.ContentDuration);
            if (!info.Data.IsFullRequest)
            {
                Response.Headers.Add("Accept-Ranges", info.Data.AcceptRanges);
                Response.Headers.Add("Content-Range", info.Data.ContentRange);
            }

            Response.Headers.Add("Content-Length", info.Data.ContentLength);
            Response.ContentType = info.Data.ContentType;
            Response.StatusCode =
                info.Data.IsFullRequest ? (int) HttpStatusCode.OK : (int) HttpStatusCode.PartialContent;
            Response.Headers.Add("Last-Modified", info.Data.LastModified);
            if (!string.IsNullOrEmpty(info.Data.Etag))
            {
                Response.Headers.Add("ETag", info.Data.Etag);
            }

            Response.Headers.Add("Cache-Control", info.Data.CacheControl);
            if (!string.IsNullOrEmpty(info.Data.Pragma))
            {
                Response.Headers.Add("Pragma", info.Data.Pragma);
            }

            Response.Headers.Add("Expires", info.Data.Expires);

            await Response.Body.WriteAsync(info.Data.Bytes, 0, info.Data.Bytes.Length).ConfigureAwait(false);

            return new EmptyResult();
        }

        public async Task<OperationResult<TrackStreamInfo>> TrackStreamInfoAsync(Guid id, long beginBytes,
            long endBytes, ApplicationUser roadieUser)
        {
            Track track = _databaseContext.Tracks.FirstOrDefault(x => x.RoadieId == id);

            string trackPath = null;
            try
            {
                trackPath = track.PathToTrack(_configuration);
            }
            catch (Exception ex)
            {
                return new OperationResult<TrackStreamInfo>(ex);
            }

            var trackFileInfo = new FileInfo(trackPath);


            var contentDurationTimeSpan = TimeSpan.FromMilliseconds(track.Duration ?? 0);
            var info = new TrackStreamInfo
            {
                FileName = _httpEncoder?.UrlEncode(track.FileName).ToContentDispositionFriendly(),
                ContentDisposition =
                    $"attachment; filename=\"{_httpEncoder?.UrlEncode(track.FileName).ToContentDispositionFriendly()}\"",
                ContentDuration = contentDurationTimeSpan.TotalSeconds.ToString()
            };
            var contentLength = endBytes - beginBytes + 1;
            info.Track = new DataToken
            {
                Text = track.Title,
                Value = track.RoadieId.ToString()
            };
            info.BeginBytes = beginBytes;
            info.EndBytes = endBytes;
            info.ContentRange = $"bytes {beginBytes}-{endBytes}/{contentLength}";
            info.ContentLength = contentLength.ToString();
            info.IsFullRequest = beginBytes == 0 && endBytes == trackFileInfo.Length - 1;
            info.IsEndRangeRequest = beginBytes > 0 && endBytes != trackFileInfo.Length - 1;
            info.LastModified = (track.LastUpdated ?? track.CreatedDate).ToString("R");

            info.CacheControl = "no-store, must-revalidate, no-cache, max-age=0";
            info.Pragma = "no-cache";
            info.Expires = "Mon, 01 Jan 1990 00:00:00 GMT";

            var bytesToRead = (int) (endBytes - beginBytes) + 1;
            var trackBytes = new byte[bytesToRead];
            using (var fs = trackFileInfo.OpenRead())
            {
                try
                {
                    fs.Seek(beginBytes, SeekOrigin.Begin);
                    var r = fs.Read(trackBytes, 0, bytesToRead);
                }
                catch (Exception ex)
                {
                    return new OperationResult<TrackStreamInfo>(ex);
                }
            }

            info.Bytes = trackBytes;
            return new OperationResult<TrackStreamInfo>
            {
                IsSuccess = true,
                Data = info
            };
        }
    }

    public class DataToken
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public sealed class TrackStreamInfo
    {
        public string AcceptRanges => "bytes";
        public long BeginBytes { get; set; }
        public byte[] Bytes { get; set; }
        public string CacheControl { get; set; }
        public string ContentDisposition { get; set; }
        public string ContentDuration { get; set; }
        public string ContentLength { get; set; }
        public string ContentRange { get; set; }
        public string ContentType => MimeTypeHelper.Mp3MimeType;
        public long EndBytes { get; set; }
        public string Etag { get; set; }
        public string Expires { get; set; }
        public string FileName { get; set; }
        public bool IsEndRangeRequest { get; set; }
        public bool IsFullRequest { get; set; }
        public string LastModified { get; set; }
        public string Pragma { get; set; }
        public DataToken Track { get; set; }

        public override string ToString()
        {
            return $"TrackId [{Track}], ContentRange [{ContentRange}], Begin [{BeginBytes}], End [{EndBytes}]";
        }
    }

    public static class MimeTypeHelper
    {
        public static string Mp3Extension = ".mp3";

        public static readonly Dictionary<string, string> AudioMimeTypes =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
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

        public static readonly Dictionary<string, string> ImageMimeTypes =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".tbn", "image/jpeg"},
                {".png", "image/png"},
                {".gif", "image/gif"},
                {".tiff", "image/tiff"},
                {".webp", "image/webp"},
                {".ico", "image/vnd.microsoft.icon"},
                {".svg", "image/svg+xml"},
                {".svgz", "image/svg+xml"}
            };


        public static string Mp3MimeType => AudioMimeTypes[Mp3Extension];

        public static bool IsFileAudioType(string fileName) => IsFileAudioType(new FileInfo(fileName));

        public static bool IsFileAudioType(FileInfo file)
        {
            if (file?.Exists != true)
            {
                return false;
            }

            var ext = file.Extension;
            return AudioMimeTypes.TryGetValue(ext, out _);
        }

        public static bool IsFileImageType(string fileName) => IsFileImageType(new FileInfo(fileName));

        public static bool IsFileImageType(FileInfo file)
        {
            if (file?.Exists != true)
            {
                return false;
            }

            var ext = file.Extension;
            return ImageMimeTypes.TryGetValue(ext, out _);
        }
    }
}