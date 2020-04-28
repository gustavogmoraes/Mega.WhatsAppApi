using System.Collections.Generic;
using System;
namespace Mega.WhatsAppApi.Infraestrutura.Objetos
{
    public class Erro
    {
        public string Id { get; set; }

        public Dictionary<string, string> Route { get; set; }

        public Exception Exception { get; set; }
    }
}