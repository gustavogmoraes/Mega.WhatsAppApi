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

namespace Mega.WhatsAppApi.Infraestrutura.Utils
{
    public static class Extensions 
    {
        public static StringContent GetStringContent(this object obj)
        {
            var jsonContent = JsonConvert.SerializeObject(obj);

            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return contentString;
        }

        public static dynamic GetStringContentResult(this string resultString)
        { 
            return (dynamic)JsonConvert.DeserializeObject<ExpandoObject>(resultString, new ExpandoObjectConverter());
        }

        public static SecureString ToSecureString(this string str)
        {
            var secureString = new SecureString();
            str.ToList().ForEach(x => secureString.AppendChar(x));

            return secureString;
        }
    }
}