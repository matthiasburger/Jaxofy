using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace DasTeamRevolution.Models.Dto
{
    /// <summary>
    /// Every request should return this, even if no items are included in it. <para> </para>
    /// If <see cref="ResponseBodyDto.Items"/> is not <c>null</c> or empty, it means that the request was successful. <para> </para>
    /// Failed requests should return the correct HTTP status code, but (if applicable) still return one or more errors inside <see cref="Constants.Errors"/>.
    /// </summary>
    public class ResponseBodyDto
    {
        /// <summary>
        /// Response data payload.
        /// </summary>
        public ResponseBodyDataDto Data { get; set; }

        /// <summary>
        /// If the request failed, the error should be written into this field for the client to handle.
        /// </summary>
        public Error Error { get; set; }

        /// <summary>
        /// If the request failed, the error should be written into this field for the client to handle.
        /// </summary>
        public List<Error> ValidationError { get; set; }

        /// <summary>
        /// Parameterless ctor.
        /// </summary>
        public ResponseBodyDto()
        {
            //nop
        }

        /// <summary>
        /// ActionContext ctor for Validation-ErrorHandling 
        /// </summary>
        public ResponseBodyDto(ActionContext actionContext)
        {
            List<string> validationErrors = actionContext.ModelState
                .SelectMany(modelError =>
                    modelError.Value.Errors.Select(x => x.ErrorMessage)
                ).ToList();
            
            (ValidationError ??= new List<Error>()).AddRange(validationErrors.Select(c=>new Error(-1, c)));
            Error = new Error((int)HttpStatusCode.BadRequest,
                $"{validationErrors.Count} validation-errors. Check 'ValidationError' section");
        }
    }
}