using System;

namespace Mega.WhatsAppApi.Infraestrutura.Objetos
{
    public class Sessao
    {
        public string Id { get; set; }

        public  DateTime? HorarioDeInicio { get; set; }

        public  Cliente Cliente { get; set; }
    }
}