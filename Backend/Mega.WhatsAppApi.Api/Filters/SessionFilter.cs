using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Mega.WhatsAppApi.Infrastructure.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Mega.WhatsAppApi.Domain.Objects;
using Mega.WhatsAppApi.Api.Extensions;
using Mega.WhatsAppApi.Infrastructure.Utils;
using Mega.WhatsAppApi.Infrastructure.Services;

namespace Mega.WhatsAppApi.Api.Filters
{
    /// <summary>
    /// Session filter class.
    /// </summary>
    public class SessionFilter : IActionFilter
    {
        /// <summary>
        /// Runs on action executing.
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            var commonLog = new Log
            {
                Horario = DateTime.UtcNow,
                TipoDeOperacao = "Request",
                Endpoint = $"{controllerActionDescriptor.ControllerName}.{controllerActionDescriptor.ActionName}",
                Dados = new 
                {
                    Headers = context.HttpContext.Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToList()),
                    Body = context.ActionArguments.ToDictionary(x => x.Key, x => x.Value)
                },
            };

            var tokenCliente = context.HttpContext.Request.Headers["TokenCliente"].ToString();
            if (string.IsNullOrEmpty(tokenCliente))
            {
                StoreLogAsync(commonLog);

                context.Result = new ForbidResult();
                return;
            }

            if (!ValidateTokenAndStartSession(tokenCliente, context.HttpContext))
            {
                commonLog.Session = context.HttpContext.GetClientSession();

                StoreLogAsync(commonLog);
                context.Result = new UnauthorizedResult();

                return;
            }

            commonLog.Session = context.HttpContext.GetClientSession();
            StoreLogAsync(commonLog);
        }

        /// <summary>
        /// Runs on action executed.
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            StoreLogAsync(new Log
            {
                Horario = DateTime.UtcNow.ToBraziliaDateTime(),
                Session = context.HttpContext.GetClientSession(),
                TipoDeOperacao = "Response",
                Endpoint = $"{controllerActionDescriptor.ControllerName}.{controllerActionDescriptor.ActionName}",
                Dados = new 
                {  
                    Headers = context?.HttpContext?.Response?.Headers?.ToDictionary(x => x.Key, x => x.Value.ToList()),
                    Body = (ResultadoRequest)((dynamic)context?.Result)?.Value
                }
            });
            EndSession(context.HttpContext);
        }

        private static void StartSession(Client client, HttpContext httpContext)
        {
            httpContext.Session.SetString("clientSession", JsonConvert.SerializeObject(new Session
            {
                Id = Guid.NewGuid().ToString(),
                StartTime = DateTime.UtcNow.ToBraziliaDateTime(),
                Client = client,
            }));
        }

        private static void EndSession(HttpContext httpContext)
        {
            httpContext.Session.Remove("clientSession");
        }

        private bool ValidateTokenAndStartSession(string token, HttpContext httpContext)
        {
            using var clientService = new ClientService();

            var client = clientService.QueryClient(token);
            if (client == null)
            {
                return false;
            }

            StartSession(client, httpContext);
            return true;
        }

        private void WriteResponseUsuarioNaoAutorizadoOuNaoExiste(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            httpContext.Response.WriteAsync("Usuário não autorizado ou inexistente").Wait();
        }

        private void WriteResponseRequisaoSemToken(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            httpContext.Response.WriteAsync("Requisiçao realizada sem token de acesso").Wait();
        }

        private void StoreLogAsync(Log log)
        {
            Task.Run(() =>
            {
                using(var servicoDeLoggin = new LoggingService())
                {
                    servicoDeLoggin.StoreLog(log);
                }
            });
        }
    }
}
