using System.Collections.Generic;
using System;
using Mega.WhatsAppApi.Infrastructure.Objects.Base;

namespace Mega.WhatsAppApi.Infrastructure.Objects
{
    /// <summary>
    /// Error class.
    /// </summary>
    public class Error : Entity
    {
        /// <summary>
        /// Stores the route.
        /// </summary>
        /// <value>The route.</value>
        public Dictionary<string, string> Route { get; set; }

        /// <summary>
        /// Stores the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Api session for log and error tracing.
        /// </summary>
        /// <value></value>
        public Session Session { get; set; }
    }
}