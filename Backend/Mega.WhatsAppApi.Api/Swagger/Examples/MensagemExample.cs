using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mega.WhatsAppApi.Dominio.Objetos;
using Swashbuckle.AspNetCore.Filters;

namespace Mega.WhatsAppApi.Api.Swagger.Examples
{
    public class MensagemExample : IExamplesProvider<Mensagem>
    {
        public Mensagem GetExamples()
        {
            return new Mensagem
            {
                Telefone = "6296977961",
                Texto = "Seu pedido está pronto, acesse pelo link: https://github.com/gustavogmoraes",
                TipoDeMensagem = "Texto"
            };
        }
    }
}
