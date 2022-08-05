using System;
using System.Net;
using System.Threading.Tasks;
using Jaxofy.Controllers.Base;
using Jaxofy.Data.Models;
using Jaxofy.Exceptions;
using Jaxofy.Services.TrackService;
using Jaxofy.Services.TrackService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jaxofy.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlayController : ApiBaseController
    {
        private readonly ITrackService _trackService;
        private readonly IConfiguration _configuration;

        public PlayController(ITrackService trackService, IConfiguration configuration)
        {
            _trackService = trackService;
            _configuration = configuration;
        }

        [HttpGet, Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> StreamTrack()
        {
            return await StreamTrack(Guid.NewGuid()).ConfigureAwait(false);
        }
        
        [HttpGet, Route("{trackGuid:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> StreamTrack(Guid trackGuid)
        {
            return await _streamTrack(trackGuid).ConfigureAwait(false);
        }

        [NonAction]
        private async Task<IActionResult> _streamTrack(Guid id)
        {
            OperationResult<Track> track = await _trackService.StreamCheckAndInfoAsync(id);

            long byteStart = _trackService.DetermineByteStartFromHeaders(Request.Headers);
            long byteEnd = _trackService.DetermineByteEndFromHeaders(Request.Headers, track.Data.FileSize);

            (long start, long end) = _trackService.DetermineByteRangeFromHeaders(Request.Headers, track.Data.FileSize);

            if (start != byteStart)
                throw new AssertionException($"start {start} <> byteStart {byteStart}");
            if (end != byteEnd)
                throw new AssertionException($"start {end} <> byteStart {byteEnd}");
            
            OperationResult<TrackStreamInfo> info = await _trackService.TrackStreamInfoAsync(id, byteStart, byteEnd)
                .ConfigureAwait(false);

            Response.Headers.Add("Content-Disposition", info.Data.ContentDisposition);
            Response.Headers.Add("X-Content-Duration", info.Data.ContentDuration);
            Response.Headers.Add("Content-Duration", info.Data.ContentDuration);
            Response.Headers.Add("Content-Length", info.Data.ContentLength);
            Response.ContentType = info.Data.ContentType;
            
            if (!info.Data.IsFullRequest)
            {
                Response.Headers.Add("Accept-Ranges", info.Data.AcceptRanges);
                Response.Headers.Add("Content-Range", info.Data.ContentRange);
                Response.StatusCode = (int) HttpStatusCode.PartialContent;
            }
            else
            {
                Response.StatusCode = (int) HttpStatusCode.OK;
            }

            Response.Headers.Add("Last-Modified", info.Data.LastModified);
            if (!string.IsNullOrEmpty(info.Data.Etag))
                Response.Headers.Add("ETag", info.Data.Etag);

            Response.Headers.Add("Cache-Control", info.Data.CacheControl);
            
            if (!string.IsNullOrEmpty(info.Data.Pragma))
                Response.Headers.Add("Pragma", info.Data.Pragma);

            Response.Headers.Add("Expires", info.Data.Expires);

            // todo: remove if everything works fine
            // await Response.Body.WriteAsync(info.Data.Bytes, 0, info.Data.Bytes.Length).ConfigureAwait(false);

            await Response.Body
                .WriteAsync(info.Data.Bytes.AsMemory(0, info.Data.Bytes.Length))
                .ConfigureAwait(false);
           
            return new EmptyResult();
        }
    }
}