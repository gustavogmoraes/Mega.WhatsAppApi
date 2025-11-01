using System.Collections.Generic;
using Mega.SmsAutomator.Objects;
using Mega.WhatsAppApi.Api.Filters;
using Mega.WhatsAppApi.Domain.Objects;
using Mega.WhatsAppApi.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Mega.WhatsAppApi.Api.Swagger.Examples;
using Mega.WhatsAppApi.Infrastructure.Objects;
using Mega.WhatsAppApi.Api.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("ObtenhaSmsParaEnviar")]
        public ActionResult<List<ToBeSent>> ObtenhaSmsParaEnviar()
        {
            return new MessageService(HttpContext.GetClientSession().Client).GetSmsToBeSent();
        }
        
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("CadastreMensagensEnviadas")]
        public void CadastreMensagensEnviadas([FromBody]List<ToBeSent> sentMessages)
        {
            new MessageService(HttpContext.GetClientSession().Client).AddSentMessages(sentMessages);
        }
        
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("GetNumberOfDocumentsOnCollection")]
        public ActionResult<int> GetNumberOfDocumentsOnCollection(string collectionName)
        {
            return new BackdoorService().GetNumberOfDocumentsOnCollection(collectionName);
        }
        
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("Query")]
        public ActionResult<dynamic> Query(string query)
        {
            return new BackdoorService().Query(query);
        }
    }
}
