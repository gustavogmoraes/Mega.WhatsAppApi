using System;
using System.Collections.Generic;
using System.Text;
using Mega.WhatsAppApi.Dominio.Objetos;

namespace Mega.WhatsAppApi.Infraestrutura.Servicos
{
    public class ServicoDeMensagem : IDisposable
    {
        public dynamic EnvieMensagem(Mensagem mensagem)
        {
            return new { };
        }

        public void Dispose()
        {
        }
    }
}
