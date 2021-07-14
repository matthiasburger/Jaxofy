using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DasTeamRevolution.Models;
using DasTeamRevolution.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DasTeamRevolution.Controllers.Base
{
    public class ODataBaseController : ODataController
    {
        public EnvelopeResult EnvelopeResult { get; init; }
        
        public ODataBaseController()
        {
            EnvelopeResult = new EnvelopeResult(this);
        }
        
        [NonAction]
        public StatusCodeResult Forbidden()
            => StatusCode((int) HttpStatusCode.Forbidden);

        [NonAction]
        public StatusCodeResult InternalServerError()
            => StatusCode((int) HttpStatusCode.InternalServerError);

        [NonAction]
        protected OkObjectResult Ok<T>(IEnumerable<T> items) where T : class
        {
            if (items is null)
                return _okArray(Array.Empty<T>());

            return _okArray(items);
        }
        
        [NonAction]
        protected OkObjectResult Ok<T>(IList<T> items) where T : class
        {
            if (items is null)
                return _okArray(Array.Empty<T>());

            return _okArray(items);
        }

        [NonAction]
        protected OkObjectResult Ok<T>(List<T> items) where T : class
        {
            if (items is null)
                return _okArray(Array.Empty<T>());

            return _okArray(items);
        }

        [NonAction]
        protected OkObjectResult Ok<T>(T item) where T : class
            => _okArray(new[] { item });

        private OkObjectResult _okArray<T>(IEnumerable<T> items)
        {
            Type itemsType = items.GetType();

            Type itemType = itemsType.IsArray
                ? itemsType.GetElementType()
                : itemsType.GetGenericArguments()[0];

            IEnumerable<T> enumerable = items as T[] ?? items.ToArray();
            
            return base.Ok(new ResponseBodyDto
            {
                Data = new ResponseBodyDataDto
                {
                    Type = itemType?.Name,
                    Items = enumerable.Cast<object>(),
                    Count = enumerable.LongCount()
                }
            });
        }

        [NonAction]
        protected IActionResult Error(int httpCode, Error error)
        {
            if (httpCode is < 400 or >= 600)
            {
                throw new ArgumentException($"The passed {nameof(httpCode)} argument \"{httpCode}\" is NOT a valid HTTP Status code from the error code range!", nameof(httpCode));
            }

            return StatusCode(httpCode, new ResponseBodyDto { Error = error });
        }
        
        protected IActionResult Error(HttpStatusCode httpCode, Error error)
        {
            return Error((int) httpCode, error);
        }
        
        [NonAction]
        protected IActionResult Error(HttpStatusCode httpCode, string error)
        {
            if ((int) httpCode is < 400 or >= 600)
            {
                throw new ArgumentException(
                    $"The passed {nameof(httpCode)} argument \"{httpCode}\" is NOT a valid HTTP Status code from the error code range!",
                    nameof(httpCode));
            }

            return StatusCode((int) httpCode, new ResponseBodyDto { Error = new Error { Message = error } });
        }
    }
}