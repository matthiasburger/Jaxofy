using System;
using System.Threading.Tasks;
using Jaxofy.Controllers;
using Jaxofy.Controllers.Base;
using Jaxofy.Data.Models;
using Jaxofy.Services.TrackService.Models;
using Microsoft.AspNetCore.Http;

namespace Jaxofy.Services.TrackService
{
    public interface ITrackService
    {
        Task<OperationResult<Track>> StreamCheckAndInfoAsync(Guid id);
        
        [Obsolete("will be replaced soon by DetermineByteRangeFromHeaders()")]
        long DetermineByteStartFromHeaders(IHeaderDictionary requestHeaders);
        [Obsolete("will be replaced soon by DetermineByteRangeFromHeaders()")]
        long DetermineByteEndFromHeaders(IHeaderDictionary requestHeaders, long fileSize);

        Task<OperationResult<TrackStreamInfo>> TrackStreamInfoAsync(Guid id, long determineByteStartFromHeaders,
            long determineByteEndFromHeaders);

        (long start, long end) DetermineByteRangeFromHeaders(IHeaderDictionary requestHeaders, long fileSize);
    }

    
}