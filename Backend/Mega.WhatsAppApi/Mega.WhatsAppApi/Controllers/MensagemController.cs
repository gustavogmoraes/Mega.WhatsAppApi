using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mega.WhatsAppApi.Dominio.Objetos;
using Mega.WhatsAppApi.Infraestrutura.Objetos.Base;
using Mega.WhatsAppApi.Infraestrutura.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Mega.WhatsAppApi.Api.Controllers
{
    [Route("api/[controller]")]
    public class MensagemController : Controller
    {
        [HttpPost]
        public ActionResult<dynamic> Post([FromBody]Mensagem mensagem)
        {
            using var servicoDeMensagem = new ServicoDeMensagem();

            return servicoDeMensagem.EnvieMensagem(mensagem);
        }
    }
}
