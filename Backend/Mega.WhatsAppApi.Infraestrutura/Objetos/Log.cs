using System;
using System.Reflection;

namespace Mega.WhatsAppApi.Infraestrutura.Objetos
{
    public class Log
    {
        public string Id { get; set; }
        public DateTime Horario { get; set; } 
        public string TipoDeOperacao {get; set;}
        public string Endpoint {get; set;}
        public object Dados {get; set;}
        public Sessao Sessao { get; set; }
    }
}