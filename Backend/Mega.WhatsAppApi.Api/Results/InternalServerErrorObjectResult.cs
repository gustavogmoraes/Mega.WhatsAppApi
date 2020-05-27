using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mega.WhatsAppApi.Api.Results
{
    /// <summary>
    /// Internal server error object result.
    /// </summary>
    public class InternalServerErrorObjectResult : ObjectResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <returns></returns>
        public InternalServerErrorObjectResult() : this(null)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}