using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jaxofy.Controllers.Base;
using Jaxofy.Data;
using Jaxofy.Data.Models;
using Jaxofy.Extensions;
using Jaxofy.Services.HttpEncoder;
using Jaxofy.Services.TrackService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Jaxofy.Services.TrackService
{
    public class TrackService : ITrackService
    {
        private readonly IHttpEncoder _httpEncoder;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public TrackService(IHttpEncoder httpEncoder, IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _httpEncoder = httpEncoder;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        /// <summary>
        /// checks if track and stream is available
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OperationResult<Track>> StreamCheckAndInfoAsync(Guid id)
        {
            Track track = await _dbContext.Tracks.FirstOrDefaultAsync(x => x.SongGuid == id);

            if (track == null)
                return new (true, $"Track Not Found [{id}]");

            return new ()
            {
                Data = track,
                IsSuccess = true
            };
        }

        /// <summary>
        /// Gets start of Range-Header
        /// </summary>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        [Obsolete("will be replaced soon by DetermineByteRangeFromHeaders()")]
        public long DetermineByteStartFromHeaders(IHeaderDictionary requestHeaders)
        {
            if (requestHeaders?.Any(x => x.Key == "Range") != true)
                return 0;

            StringValues rangeHeader = requestHeaders["Range"];
            string rangeBegin = rangeHeader.FirstOrDefault();

            if (string.IsNullOrEmpty(rangeBegin))
                return 0;

            //bytes=0-
            rangeBegin = rangeBegin.Replace("bytes=", string.Empty);
            string[] parts = rangeBegin.Split('-');
            rangeBegin = parts[0];
            if (!string.IsNullOrEmpty(rangeBegin) && long.TryParse(rangeBegin, out long result))
                return result;

            return 0;
        }

        /// <summary>
        /// Gets end of Range-Header
        /// </summary>
        /// <param name="requestHeaders"></param>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        [Obsolete("will be replaced soon by DetermineByteRangeFromHeaders()")]
        public long DetermineByteEndFromHeaders(IHeaderDictionary requestHeaders, long fileSize)
        {
            long defaultFileLength = fileSize - 1;
            if (requestHeaders?.Any(x => x.Key == "Range") != true)
                return defaultFileLength;

            StringValues rangeHeader = requestHeaders["Range"];
            string rangeEnd = null;
            string rangeBegin = rangeHeader.FirstOrDefault();
            if (string.IsNullOrEmpty(rangeBegin))
                return defaultFileLength;

            //bytes=0-
            rangeBegin = rangeBegin.Replace("bytes=", string.Empty);
            string[] parts = rangeBegin.Split('-');
            rangeBegin = parts[0];
            if (parts.Length > 1)
            {
                rangeEnd = parts[1];
            }

            long? result = null;

            if (!string.IsNullOrEmpty(rangeEnd))
            {
                result = long.TryParse(rangeEnd, out long outValue) ? (int?) outValue : null;
            }

            return result ?? defaultFileLength;
        }

        /// <summary>
        /// Gets complete info and track streaming-ready
        /// </summary>
        /// <param name="id"></param>
        /// <param name="beginBytes"></param>
        /// <param name="endBytes"></param>
        /// <returns></returns>
        public async Task<OperationResult<TrackStreamInfo>> TrackStreamInfoAsync(Guid id, long beginBytes, long endBytes)
        {
            Track track = await _dbContext.Tracks.FirstOrDefaultAsync(x => x.SongGuid == id);

            string trackPath = track.PathToTrack(_configuration);
            FileInfo trackFileInfo = new(trackPath);

            TimeSpan contentDurationTimeSpan = TimeSpan.FromMilliseconds(track.Duration ?? 0);
            long contentLength = endBytes - beginBytes + 1;

            TrackStreamInfo info = new()
            {
                FileName = _httpEncoder?.UrlEncode(track.FileName).ToContentDispositionFriendly(),
                ContentDisposition =
                    $"attachment; filename=\"{_httpEncoder?.UrlEncode(track.FileName).ToContentDispositionFriendly()}\"",
                ContentDuration = contentDurationTimeSpan.TotalSeconds.ToString(CultureInfo.InvariantCulture),
                Track = new DataToken {Text = track.Title, Value = track.SongGuid.ToString()},
                BeginBytes = beginBytes,
                EndBytes = endBytes,
                ContentRange = $"bytes {beginBytes}-{endBytes}/{contentLength}",
                ContentLength = contentLength.ToString(),
                IsFullRequest = beginBytes == 0 && endBytes == trackFileInfo.Length - 1,
                IsEndRangeRequest = beginBytes > 0 && endBytes != trackFileInfo.Length - 1,
                LastModified = (track.LastUpdated ?? track.CreatedDate).ToString("R"),
                CacheControl = "no-store, must-revalidate, no-cache, max-age=0",
                Pragma = "no-cache",
                Expires = "Mon, 01 Jan 1990 00:00:00 GMT"
            };

            int bytesToRead = (int) (endBytes - beginBytes) + 1;
            byte[] trackBytes = new byte[bytesToRead];
            await using (FileStream fs = trackFileInfo.OpenRead())
            {
                try
                {
                    fs.Seek(beginBytes, SeekOrigin.Begin);
                    int _ = fs.Read(trackBytes, 0, bytesToRead);
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

        public (long start, long end) DetermineByteRangeFromHeaders(IHeaderDictionary requestHeaders, long fileSize)
        {
            long defaultFileLength = fileSize - 1;

            if (requestHeaders?.Any(x => x.Key == "Range") != true)
                return (0, defaultFileLength);

            StringValues rangeHeader = requestHeaders["Range"];

            string rangeValue = rangeHeader.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(rangeValue))
                return (0, defaultFileLength);

            rangeValue = rangeValue.Replace("bytes=", string.Empty);
            string[] parts = rangeValue.Split('-');

            if (!long.TryParse(parts[0], out long start))
                start = 0;

            if (parts.Length <= 1 || !long.TryParse(parts[1], out long end))
                end = defaultFileLength;

            return (start, end);
        }
    }
}