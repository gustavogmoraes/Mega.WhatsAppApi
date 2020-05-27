using System.Linq;
using System.Security;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System;
using System.IO;

namespace Mega.WhatsAppApi.Infrastructure.Utils
{
    /// <summary>
    /// Extensions class.
    /// </summary>
    public static class Extensions 
    {
        /// <summary>
        /// Gets a string content to send a rest request.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static StringContent GetStringContent(this object obj)
        {
            var jsonContent = JsonConvert.SerializeObject(obj);

            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return contentString;
        }

        /// <summary>
        /// Gets a dynamic object from a string content result.
        /// </summary>
        /// <param name="resultString"></param>
        /// <returns></returns>
        public static dynamic GetStringContentResult(this string resultString)
        { 
            return (dynamic)JsonConvert.DeserializeObject<ExpandoObject>(resultString, new ExpandoObjectConverter());
        }

        /// <summary>
        /// Returns a secure string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static SecureString ToSecureString(this string str)
        {
            var secureString = new SecureString();
            str.ToList().ForEach(x => secureString.AppendChar(x));

            return secureString;
        }

        /// <summary>
        /// Check if enviroment is development.
        /// </summary>
        /// <returns></returns>
        public static bool EnvironmentIsDevelopment()
        {
            return Environment.UserName == "Gustavo Moraes";
        }

        /// <summary>
        /// Converts datetime from Utc to Brazilia time aka E. South America.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>The converted datetime.</returns>
        public static DateTime ToBraziliaDateTime(this DateTime dateTime) 
        { 
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
        }
    }
}