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
        
        long DetermineByteStartFromHeaders(IHeaderDictionary requestHeaders);
        long DetermineByteEndFromHeaders(IHeaderDictionary requestHeaders, long fileSize);

        Task<OperationResult<TrackStreamInfo>> TrackStreamInfoAsync(Guid id, long determineByteStartFromHeaders,
            long determineByteEndFromHeaders);

        (long start, long end) DetermineByteRangeFromHeaders(IHeaderDictionary requestHeaders, long fileSize);
    }

    
}