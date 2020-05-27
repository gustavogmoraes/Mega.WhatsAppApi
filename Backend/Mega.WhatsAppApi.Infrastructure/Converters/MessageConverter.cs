using System;
using Mega.WhatsAppApi.Domain.Objects;

namespace Mega.WhatsAppApi.Infrastructure.Converters
{
    /// <summary>
    /// Message converter class.
    /// </summary>
    public class MessageConverter : IDisposable
    {
        /// <summary>
        /// Converts the Api dto to system object.
        /// </summary>
        /// <returns></returns>
        public dynamic ConvertMessage(Mensagem mensagem) => mensagem.TipoDeMensagem.ToLowerInvariant() switch
        {
            "texto" => ConvertTextMessage(mensagem),
            "arquivo" => ConvertFileMessage(mensagem),
            _ => throw new ArgumentException(message: "Valor de enumerador inválido", paramName: nameof(mensagem.TipoDeMensagem))
        };

        private dynamic ConvertTextMessage(Mensagem mensagem) => new
        {
            to_number = ConvertNumber(mensagem.Telefone),
            message = mensagem.Texto,
            type = "text"
        };

        private dynamic ConvertFileMessage(Mensagem mensagem) => new
        {
            to_number = ConvertNumber(mensagem.Telefone),
            text = mensagem.Texto,
            message = mensagem.LinkArquivo,
            type = "media"
        };

        private string ConvertNumber(string numeroRecebido)
        {
            var retorno = new string(numeroRecebido);
            if (!retorno.Contains("+55"))
            {
                retorno = "+55" + retorno;
            }

            if (retorno.Length > 13)
            {
                retorno = retorno.Remove(5, 1);
            }

            return retorno;
        }

        /// <summary>
        /// Disposes.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}