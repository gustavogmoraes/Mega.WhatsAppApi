using System;
using Microsoft.AspNetCore.Hosting;

namespace Mega.WhatsAppApi.Api.Extensions
{
    /// <summary>
    /// Web host builder extensions.
    /// Made specifically to deploy this app as a container registry on Heroku.
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Specifies a port to use.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IWebHostBuilder UsePort(this IWebHostBuilder builder)
        {
            var port = Environment.GetEnvironmentVariable("PORT");
            if (string.IsNullOrEmpty(port))
            {
                return builder;
            }

            return builder.UseUrls($"http://+:{port}");
        }
    }
}