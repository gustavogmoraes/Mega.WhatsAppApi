using System;
using System.Reflection;
using Mega.WhatsAppApi.Infrastructure.Objects.Base;

namespace Mega.WhatsAppApi.Infrastructure.Objects
{
    /// <summary>
    /// Class for log.
    /// </summary>
    public class Log : Entity
    {
        /// <summary>
        /// Date time when the log was stored. GMT -3 based.
        /// </summary>
        /// <value>The date time.</value>
        public DateTime Horario { get; set; } 
        
        /// <summary>
        /// Kind of operation (Request or Response).
        /// </summary>
        /// <value>The kind of operation.</value>
        public string TipoDeOperacao {get; set;}

        /// <summary>
        /// Called endpoint.
        /// </summary>
        /// <value>The endpoint.</value>
        public string Endpoint {get; set;}

        /// <summary>
        /// Log dynamic data.
        /// </summary>
        /// <value>Log data.</value>
        public object Dados {get; set;}

        /// <summary>
        /// Stores the session.
        /// </summary>
        /// <value>The session.</value>
        public Session Session { get; set; }
    }
}