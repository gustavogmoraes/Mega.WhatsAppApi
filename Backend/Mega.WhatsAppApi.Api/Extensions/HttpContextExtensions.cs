using Mega.WhatsAppApi.Infraestrutura.Objetos;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Mega.WhatsAppApi.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static Sessao GetClientSession(this HttpContext httpContext)
        {
            return JsonConvert.DeserializeObject<Sessao>(httpContext.Session.GetString("clientSession"));
        }
    }
}