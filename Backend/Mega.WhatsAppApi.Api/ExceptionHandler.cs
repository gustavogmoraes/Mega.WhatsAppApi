using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Mega.WhatsAppApi.Api.Results;
using Mega.WhatsAppApi.Dominio.Objetos;
using Mega.WhatsAppApi.Infraestrutura.Objetos;
using Mega.WhatsAppApi.Infraestrutura.Servicos;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mega.WhatsAppApi.Api
{
    public class ExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            SalveErroAsync(new Erro
            {
                Route = context.ActionDescriptor.RouteValues.ToDictionary(x => x.Key, x => x.Value),
                Exception = context.Exception
            });

            context.Result = new InternalServerErrorObjectResult(new ResultadoRequest 
            {
                Mensagem = "Ocorreu um erro desconhecido na API!\nO desenvolvedor já foi contatado com os logs e informações e já está trabalhando para resolver.\nTente refazer a requisição."
            });
        }

        private void SalveErroAsync(Erro erro)
        {
            Task.Run(() =>
            {
                using(var servicoDeLogging = new ServicoDeLoggingMega())
                {
                    servicoDeLogging.SalveErro(erro);
                }
            });
        }
    }
}