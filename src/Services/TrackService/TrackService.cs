using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jaxofy.Controllers;
using Jaxofy.Controllers.Base;
using Jaxofy.Data;
using Jaxofy.Data.Models;
using Jaxofy.Extensions;
using Jaxofy.Services.HttpEncoder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Jaxofy.Services.TrackService
{
    public class TrackService : ITrackService
    {
        private readonly IHttpEncoder _httpEncoder;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public TrackService( IHttpEncoder httpEncoder, IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _httpEncoder = httpEncoder;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public OperationResult<Track> StreamCheckAndInfo(ApplicationUser user, Guid id)
        {
            Track track = _dbContext.Tracks.FirstOrDefault(x => x.RoadieId == id);
            if (track == null)
            {
                return new OperationResult<Track>(true, $"Track Not Found [{id}]");
            }

            return new OperationResult<Track>
            {
                Data = track,
                IsSuccess = true
            };
        }

        public long DetermineByteStartFromHeaders(IHeaderDictionary requestHeaders)
        {
            if (requestHeaders?.Any(x => x.Key == "Range") != true)
            {
                return 0;
            }

            long result = 0;
            var rangeHeader = requestHeaders["Range"];
            var rangeBegin = rangeHeader.FirstOrDefault();
            if (!string.IsNullOrEmpty(rangeBegin))
            {
                //bytes=0-
                rangeBegin = rangeBegin.Replace("bytes=", string.Empty);
                var parts = rangeBegin.Split('-');
                rangeBegin = parts[0];
                if (!string.IsNullOrEmpty(rangeBegin))
                {
                    long.TryParse(rangeBegin, out result);
                }
            }

            return result;
        }

        public long DetermineByteEndFromHeaders(IHeaderDictionary requestHeaders, long fileSize)
        {
            var defaultFileLength = fileSize - 1;
            if (requestHeaders?.Any(x => x.Key == "Range") != true)
            {
                return defaultFileLength;
            }

            long? result = null;
            var rangeHeader = requestHeaders["Range"];
            string rangeEnd = null;
            var rangeBegin = rangeHeader.FirstOrDefault();
            if (!string.IsNullOrEmpty(rangeBegin))
            {
                //bytes=0-
                rangeBegin = rangeBegin.Replace("bytes=", string.Empty);
                var parts = rangeBegin.Split('-');
                rangeBegin = parts[0];
                if (parts.Length > 1)
                {
                    rangeEnd = parts[1];
                }

                if (!string.IsNullOrEmpty(rangeEnd))
                {
                    result = long.TryParse(rangeEnd, out var outValue) ? (int?) outValue : null;
                }
            }

            return result ?? defaultFileLength;
        }

        public async Task<OperationResult<TrackStreamInfo>> TrackStreamInfoAsync(Guid id, long beginBytes,
            long endBytes,
            ApplicationUser roadieUser)
        {
            Track track = _dbContext.Tracks.FirstOrDefault(x => x.RoadieId == id);

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
}