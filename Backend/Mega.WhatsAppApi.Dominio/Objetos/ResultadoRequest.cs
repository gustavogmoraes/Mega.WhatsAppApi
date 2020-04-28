using System;
using System.Collections.Generic;

namespace Mega.WhatsAppApi.Dominio.Objetos
{
    [Serializable]
    public class ResultadoRequest
    {
        public string Mensagem { get; set; }

        public List<Inconsistencia> Inconsistencias { get; set; }
    }
}