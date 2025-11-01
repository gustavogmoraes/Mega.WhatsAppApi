using System;
using Mega.WhatsAppApi.Infrastructure.Objects.Base;

namespace Mega.WhatsAppApi.Infrastructure.Objects
{
    /// <summary>
    /// Custom session class.
    /// </summary>
    public class Session : Entity
    {
        /// <summary>
        /// Starting session time.
        /// </summary>
        /// <value>The time the session was started.</value>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Client who started session.
        /// </summary>
        /// <value>The client.</value>
        public Client Client { get; set; }
    }
}