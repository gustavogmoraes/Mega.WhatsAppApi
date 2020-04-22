using System;
using Mega.WhatsAppApi.Dominio.Enumeradores;

namespace Mega.WhatsAppApi.Dominio.Objetos
{
    public class Mensagem
    {
        public string Telefone { get; set; }

        public string Texto { get; set; }

        public EnumTipoDeMensagem TipoDeMensagem { get; set; }
    }
}
