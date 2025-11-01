using System;
using System.Collections.Generic;
using System.Text;

namespace Mega.WhatsAppApi.Domain.Objects
{
    /// <summary>
    /// Classe de inconsistência para uma validação.
    /// </summary>
    public class Inconsistencia
    {
        /// <summary>
        /// O nome do objeto validado.
        /// </summary>
        /// <value></value>
        public string ObjetoValidado { get; set; }

        /// <summary>
        /// O nome da propriedade validada.
        /// </summary>
        /// <value></value>
        public string PropriedadeValidade { get; set; }

        /// <summary>
        /// A mensagem de validação.
        /// </summary>
        /// <value></value>
        public string Mensagem { get; set; }
    }
}
