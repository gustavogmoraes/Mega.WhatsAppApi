using Mega.WhatsAppApi.Infrastructure.Objects;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Mega.WhatsAppApi.Api.Extensions
{
    /// <summary>
    /// Extensions for HttpContext class.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Gets the client session.
        /// </summary>
        /// <returns>The session.</returns>
        public static Session GetClientSession(this HttpContext httpContext) => 
            JsonConvert.DeserializeObject<Session>(httpContext.Session.GetString("clientSession"));
    }
}