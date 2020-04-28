using Mega.WhatsAppApi.Api.Filters;
using Mega.WhatsAppApi.Dominio.Objetos;
using Mega.WhatsAppApi.Infraestrutura.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Mega.WhatsAppApi.Api.Swagger.Examples;
using Mega.WhatsAppApi.Infraestrutura.Objetos;
using Mega.WhatsAppApi.Api.Extensions;

namespace Mega.WhatsAppApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// Controller de Mensagem
    /// </summary>
    /// 
    public class MensagemController : Controller
    {
        [HttpPost("EnvieMensagem")]
        //[SwaggerRequestExample(typeof(Mensagem), typeof(MensagemExample))]
        /// <summary>
        /// Envia mensagem.
        /// </summary>
        /// <param name="mensagem"></param>
        /// <returns></returns>
        public ActionResult<ResultadoRequest> EnvieMensagem([FromBody]Mensagem mensagem)
        {
            using var servicoDeMensagem = new ServicoDeMensagem(HttpContext.GetClientSession().Cliente);
            return servicoDeMensagem.EnvieMensagem(mensagem);
        }
    }
}
