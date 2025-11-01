using System;
using System.Collections.Generic;
using Mega.WhatsAppApi.Domain.Objects;

namespace Mega.WhatsAppApi.Infrastructure.Validators
{
    /// <summary>
    /// Message validator class.
    /// </summary>
    public class MessageValidator : IDisposable
    {
        /// <summary>
        /// Validates a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<Inconsistencia> ValidateMessage(Mensagem message)
        {
            return new List<Inconsistencia>();
        }

        /// <summary>
        /// Disposes.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}