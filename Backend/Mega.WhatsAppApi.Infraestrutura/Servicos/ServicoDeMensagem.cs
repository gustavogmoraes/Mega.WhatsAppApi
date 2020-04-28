using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Mega.WhatsAppApi.Dominio.Objetos;
using Mega.WhatsAppApi.Infraestrutura.Utils;
using Mega.WhatsAppApi.Infraestrutura.Validadores;
using System.Linq;
using Mega.WhatsAppApi.Infraestrutura.Objetos;

namespace Mega.WhatsAppApi.Infraestrutura.Servicos
{
    public class ServicoDeMensagem : IDisposable 
    {
        private Cliente Cliente { get; set; }
        public ServicoDeMensagem(Cliente cliente)
        {
            Cliente = cliente;
        }

        public ResultadoRequest EnvieMensagem(Mensagem mensagem)
        {
            // Validation block
            using var validador = new ValidadorDeMensagens();
            var resultadoValidacao = validador.ValideMensagem(mensagem);
            if(resultadoValidacao.Any())
            {
                return new ResultadoRequest
                {
                    Mensagem = "Foi encontrada uma inconsistência na mensagem, avalie a lista de inconsistências para mais informações!",
                    Inconsistencias = resultadoValidacao
                };
            }
            //
            using var conversor = new ConversorDeMensagens();
            using var servicoMaytApi = new ServicoMaytApi(Cliente);

            var resultMaytApi = servicoMaytApi.SendMessage(conversor.ConvertaMensagem(mensagem));

            return new ResultadoRequest
            {
                Mensagem = "Mensagem enviada com sucesso!"
            };
        }

        public void Dispose()
        {
        }
    }
}
