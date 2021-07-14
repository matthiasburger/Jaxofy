using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using Jaxofy.Controllers;
using Jaxofy.Data.Models;
using Jaxofy.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace Jaxofy.Services.TrackService
{
    [Serializable]
    public class AppException : Exception
    {
        public AppException(string message) : base(message)
        {
        }
    }

    public class TrackService : ITrackService
    {
        private readonly IHttpEncoder _httpEncoder;

        public TrackService( IHttpEncoder httpEncoder)
        {
            _httpEncoder = httpEncoder;
        }

        public OperationResult<Track> StreamCheckAndInfo(ApplicationUser user, Guid id)
        {
            // var track = _dbContext.Tracks.FirstOrDefault(x => x.RoadieId == id);

            var track = TrackList.GetRandomTrack(id);
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

        public async Task<OperationResult<TrackStreamInfo>> TrackStreamInfoAsync(Guid trackId, long beginBytes,
            long endBytes,
            ApplicationUser roadieUser)
        {
            var track = TrackList.GetRandomTrack(trackId);

            string trackPath = null;
            try
            {
                trackPath = track.PathToTrack();
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

    public class Track
    {
        public string PathToTrack()
        {
            // todo: add configuration
            return @"C:\Users\Matthias Burger\Downloads\DankbarkeitFinal13.05.21.wav";
        }

        public double? Duration { get; set; }
        public string FileName { get; set; }
        public Guid RoadieId { get; set; }
        public string Title { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime CreatedDate { get; set; }
        public long FileSize { get; set; }
    }

    [Serializable]
    public class OperationResult<T>
    {
        private List<Exception> _errors;

        private List<string> _messages;

        [XmlIgnore]
        public Dictionary<string, object> AdditionalClientData { get; set; } = new Dictionary<string, object>();

        [JsonIgnore]
        [XmlIgnore]
        public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();

        /// <summary>
        ///     Client friendly exceptions
        /// </summary>
        [JsonPropertyName("errors")]
        public IEnumerable<AppException> AppExceptions
        {
            get
            {
                if (Errors?.Any() != true)
                {
                    return null;
                }

                return Errors.Select(x => new AppException(x.Message));
            }
        }

        public T Data { get; set; }

        /// <summary>
        ///     Server side visible exceptions
        /// </summary>
        [JsonIgnore]
        public IEnumerable<Exception> Errors { get; set; }

        [JsonIgnore] public bool IsAccessDeniedResult { get; set; }

        [JsonIgnore] public bool IsNotFoundResult { get; set; }

        public bool IsSuccess { get; set; }

        public IEnumerable<string> Messages => _messages;

        public long OperationTime { get; set; }

        public OperationResult()
        {
        }

        public OperationResult(IEnumerable<string> messages = null)
        {
            if (messages?.Any() == true)
            {
                AdditionalData = new Dictionary<string, object>();
                messages.ToList().ForEach(AddMessage);
            }
        }

        public OperationResult(string message = null)
        {
            AdditionalData = new Dictionary<string, object>();
            AddMessage(message);
        }

        public OperationResult(Exception error = null)
        {
            AddError(error);
        }

        public OperationResult(bool isNotFoundResult, IEnumerable<string> messages = null)
        {
            IsNotFoundResult = isNotFoundResult;
            if (messages?.Any() == true)
            {
                AdditionalData = new Dictionary<string, object>();
                messages.ToList().ForEach(AddMessage);
            }
        }

        public OperationResult(bool isNotFoundResult, string message)
        {
            IsNotFoundResult = isNotFoundResult;
            AddMessage(message);
        }

        public OperationResult(string message = null, Exception error = null)
        {
            AddMessage(message);
            AddError(error);
        }

        public void AddError(Exception exception)
        {
            if (exception != null)
            {
                (_errors ?? (_errors = new List<Exception>())).Add(exception);
            }
        }

        public void AddMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                (_messages ?? (_messages = new List<string>())).Add(message);
            }
        }
    }

    public static class TrackList
    {
        public static Track GetRandomTrack(Guid id)
        {
            return new()
            {
                RoadieId = id,
                Title = "test",
                Duration = 2.3,
                CreatedDate = DateTime.Now.AddDays(-2),
                FileName = "test",
                FileSize = 45_400_000,
                LastUpdated = DateTime.Now.AddMinutes(-20)
            };
        }
    }

    public interface ITrackService
    {
        OperationResult<Track> StreamCheckAndInfo(ApplicationUser user, Guid id);
        long DetermineByteStartFromHeaders(IHeaderDictionary requestHeaders);
        long DetermineByteEndFromHeaders(IHeaderDictionary requestHeaders, long fileSize);

        Task<OperationResult<TrackStreamInfo>> TrackStreamInfoAsync(Guid id, long determineByteStartFromHeaders,
            long determineByteEndFromHeaders, ApplicationUser user);
    }

    public interface IHttpEncoder
    {
        string HtmlEncode(string s);

        string UrlDecode(string s);

        string UrlEncode(string s);

        string UrlEncodeBase64(byte[] input);

        string UrlEncodeBase64(string input);
    }

    public class HttpEncoder : IHttpEncoder
    {
        public string HtmlEncode(string s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        public string UrlDecode(string s)
        {
            return HttpUtility.UrlDecode(s);
        }

        public string UrlEncode(string s)
        {
            return HttpUtility.UrlEncode(s);
        }

        public string UrlEncodeBase64(byte[] input)
        {
            return WebEncoders.Base64UrlEncode(input);
        }

        public string UrlEncodeBase64(string input)
        {
            return WebEncoders.Base64UrlEncode(Encoding.ASCII.GetBytes(input));
        }
    }
}