using System;

namespace Mega.WhatsAppApi.Dominio.Objetos
{
    public class Mensagem
    {
        /// <summary>
        /// O número de telefone pra onde a mensagem vai ser enviada. 
        /// Sempre deve ser passado o código de área antes do número, já o digito 9 é opcional. Não utilize nenhum caractere especial, somente o número.
        /// </summary>
        /// <value>
        /// Retorna a string de telefone.
        /// </value>
        public string Telefone { get; set; }

        /// <summary>
        /// O texto a ser enviado na mensagem,
        /// </summary>
        public string Texto { get; set; }

        /// <summary>
        /// Tipo de mensagem, enum { "Texto", "Arquivo" }
        /// </summary>
        /// <value></value>
        public string TipoDeMensagem { get; set; }
    }
}
