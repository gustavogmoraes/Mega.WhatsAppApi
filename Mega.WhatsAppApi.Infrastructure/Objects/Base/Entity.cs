using System;
using Mega.WhatsAppApi.Infrastructure.Interfaces.Base;

namespace Mega.WhatsAppApi.Infrastructure.Objects.Base
{
    /// <summary>
    /// Base class for id'ed entities.
    /// </summary>
    public class Entity : IEntity
    {
        /// <summary>
        /// Object unique Id.
        /// </summary>
        /// <value>The Id.</value>
        public string Id { get; set; }
    }
}
