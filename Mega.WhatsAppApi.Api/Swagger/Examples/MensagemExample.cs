using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mega.WhatsAppApi.Domain.Objects;
using Swashbuckle.AspNetCore.Filters;

namespace Mega.WhatsAppApi.Api.Swagger.Examples
{
    /// <summary>
    /// Message example for Swagger api documentation.
    /// </summary>
    public class MensagemExample : IExamplesProvider<Mensagem>
    {
        /// <summary>
        /// Returns examples.
        /// </summary>
        /// <returns></returns>
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
