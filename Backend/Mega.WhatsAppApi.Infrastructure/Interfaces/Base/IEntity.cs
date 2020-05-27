using System;
using System.Collections.Generic;
using System.Text;

namespace Mega.WhatsAppApi.Infrastructure.Interfaces.Base
{
    /// <summary>
    /// Entity interface.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// The unique identifier.
        /// </summary>
        /// <value></value>
        string Id { get; set; }
    }
}
