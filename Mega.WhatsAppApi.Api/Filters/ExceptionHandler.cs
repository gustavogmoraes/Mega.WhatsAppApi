using System.Linq;
using System.Threading.Tasks;
using Mega.WhatsAppApi.Api.Extensions;
using Mega.WhatsAppApi.Api.Results;
using Mega.WhatsAppApi.Domain.Objects;
using Mega.WhatsAppApi.Infrastructure.Objects;
using Mega.WhatsAppApi.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mega.WhatsAppApi.Api.Filters
{
    /// <summary>
    /// Exception handler filter class.
    /// </summary>
    public class ExceptionHandler : IExceptionFilter
    {
        /// <summary>
        /// Gets called on exception.
        /// </summary>
        /// <param name="context">Context.</param>
        public void OnException(ExceptionContext context)
        {
            StoreErrorAsync(new Error
            {
                Route = context.ActionDescriptor.RouteValues.ToDictionary(x => x.Key, x => x.Value),
                Exception = context.Exception,
                Session = context.HttpContext.GetClientSession()
            });

            context.Result = new InternalServerErrorObjectResult(new ResultadoRequest 
            {
                Mensagem = "Ocorreu um erro desconhecido na API!\nO desenvolvedor já foi contatado com os logs e informações e já está trabalhando para resolver.\nTente refazer a requisição."
            });
        }

        private void StoreErrorAsync(Error erro)
        {
            Task.Run(() =>
            {
                using(var loggingService = new LoggingService())
                {
                    loggingService.StoreError(erro);
                }
            });
        }
    }
}