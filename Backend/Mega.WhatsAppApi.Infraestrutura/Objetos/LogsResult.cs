using System;

namespace Mega.WhatsAppApi.Infraestrutura.Objetos
{
    public class LogsResult
    {
        public int id { get; set; }
        public string pid { get; set; }
        public string phone	{ get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string type { get; set; } // enum { incoming, outgoing, error }
        public dynamic data { get; set; }
    }
}