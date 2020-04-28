using System;
using Mega.WhatsAppApi.Dominio.Objetos;

namespace Mega.WhatsAppApi.Infraestrutura.Servicos
{
    public class ConversorDeMensagens : IDisposable
    {
        public dynamic ConvertaMensagem(Mensagem mensagem) => mensagem.TipoDeMensagem.ToLowerInvariant() switch
        {
            "texto" => ConvertaMensagemTipoTexto(mensagem),
            "arquivo" => ConvertaMensagemTipoArquivo(mensagem),
            _ => throw new ArgumentException(message: "Valor de enumerador inválido", paramName: nameof(mensagem.TipoDeMensagem))
        };

        private dynamic ConvertaMensagemTipoTexto(Mensagem mensagem) => new
        {
            to_number = ConvertaNumero(mensagem.Telefone),
            message = mensagem.Texto,
            type = "text"
        };

        private dynamic ConvertaMensagemTipoArquivo(Mensagem mensagem) => new
        {
            to_number = ConvertaNumero(mensagem.Telefone),
            text = mensagem.Texto,
            message = mensagem.LinkArquivo,
            type = "media"
        };

        private string ConvertaNumero(string numeroRecebido)
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

        public void Dispose()
        {
            
        }
    }
}