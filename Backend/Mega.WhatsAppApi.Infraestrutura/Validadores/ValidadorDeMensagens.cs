using System;
using System.Collections.Generic;
using Mega.WhatsAppApi.Dominio.Objetos;

namespace Mega.WhatsAppApi.Infraestrutura.Validadores
{
    public class ValidadorDeMensagens : IDisposable
    {
        public List<Inconsistencia> ValideMensagem(Mensagem mensagem)
        {
            return new List<Inconsistencia>();
        }

        private void ValideSeNumeroEstahNoWhatsApp(string numero)
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}