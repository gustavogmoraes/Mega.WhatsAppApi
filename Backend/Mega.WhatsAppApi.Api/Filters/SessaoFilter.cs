using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Mega.WhatsAppApi.Infraestrutura.Objetos;
using Mega.WhatsAppApi.Infraestrutura.Servicos;
using Mega.WhatsAppApi.Infraestrutura.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using Mega.WhatsAppApi.Dominio.Objetos;
using Mega.WhatsAppApi.Api.Extensions;

namespace Mega.WhatsAppApi.Api.Filters
{
    public class SessaoFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            var logComum = new Log
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
                SalveLogAssync(logComum);

                context.Result = new ForbidResult();
                return;
            }

            if (!ValideTokenEInicieSessao(tokenCliente, context.HttpContext))
            {
                logComum.Sessao = context.HttpContext.GetClientSession();

                SalveLogAssync(logComum);
                context.Result = new UnauthorizedResult();

                return;
            }

            logComum.Sessao = context.HttpContext.GetClientSession();
            SalveLogAssync(logComum);
        }

        private static void InicieSessao(Cliente cliente, HttpContext httpContext)
        {
            httpContext.Session.SetString("clientSession", JsonConvert.SerializeObject(new Sessao
            {
                Id = Guid.NewGuid().ToString(),
                HorarioDeInicio = DateTime.UtcNow,
                Cliente = cliente,
            }));
        }

        private static void FinalizeSessao(HttpContext httpContext)
        {
            httpContext.Session.Remove("clientSession");
        }

        private bool ValideTokenEInicieSessao(string token, HttpContext httpContext)
        {
            using var servicoDeCliente = new ServicoDeCliente();

            var cliente = servicoDeCliente.ConsulteCliente(token);
            if (cliente == null)
            {
                return false;
            }

            InicieSessao(cliente, httpContext);
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

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            SalveLogAssync(new Log
            {
                Horario = DateTime.UtcNow,
                Sessao = context.HttpContext.GetClientSession(),
                TipoDeOperacao = "Response",
                Endpoint = $"{controllerActionDescriptor.ControllerName}.{controllerActionDescriptor.ActionName}",
                Dados = new 
                {  
                    Headers = context?.HttpContext?.Response?.Headers?.ToDictionary(x => x.Key, x => x.Value.ToList()),
                    Body = (ResultadoRequest)((dynamic)context?.Result)?.Value
                }
            });
            FinalizeSessao(context.HttpContext);
        }

        private void SalveLogAssync(Log log)
        {
            Task.Run(() =>
            {
                using(var servicoDeLoggin = new ServicoDeLoggingMega())
                {
                    servicoDeLoggin.SalveLog(log);
                }
            });
        }
    }
}
