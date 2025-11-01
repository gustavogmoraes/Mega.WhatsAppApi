using System;
using System.Collections.Generic;

namespace Mega.WhatsAppApi.Domain.Objects
{
    /// <summary>
    /// Envolpe de retorno para uma request.
    /// </summary>
    [Serializable]
    public class ResultadoRequest
    {
        /// <summary>
        /// Mensagem de retorno.
        /// </summary>
        /// <value></value>
        public string Mensagem { get; set; }

        /// <summary>
        /// Lista de inconsistências, caso existam.
        /// </summary>
        /// <value></value>
        public List<Inconsistencia> Inconsistencias { get; set; }
    }
}