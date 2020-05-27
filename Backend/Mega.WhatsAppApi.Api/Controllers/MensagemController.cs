using Mega.WhatsAppApi.Api.Filters;
using Mega.WhatsAppApi.Domain.Objects;
using Mega.WhatsAppApi.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Mega.WhatsAppApi.Api.Swagger.Examples;
using Mega.WhatsAppApi.Infrastructure.Objects;
using Mega.WhatsAppApi.Api.Extensions;

namespace Mega.WhatsAppApi.Api.Controllers
{
    /// <summary>
    /// Controller de Mensagem
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MensagemController : Controller
    {
        /// <summary>
        /// Envia uma mensagem.
        /// </summary>
        /// <param name="mensagem"></param>
        /// <returns></returns>
        [HttpPost("EnvieMensagem")]
        public ActionResult<ResultadoRequest> EnvieMensagem([FromBody]Mensagem mensagem)
        {
            using var messageService = new MessageService(HttpContext.GetClientSession().Client);
            return messageService.SendMessage(mensagem);
        }
    }
}
