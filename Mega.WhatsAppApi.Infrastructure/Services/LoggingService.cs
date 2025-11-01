using System;
using Mega.WhatsAppApi.Infrastructure.Objects;
using Mega.WhatsAppApi.Infrastructure.Persistence;

namespace Mega.WhatsAppApi.Infrastructure.Services
{
    /// <summary>
    /// Responsible for saving logs on execution.
    /// </summary>
    public class LoggingService : IDisposable
    {
        /// <summary>
        /// Stores the log.
        /// </summary>
        /// <param name="log"></param>
        public void StoreLog(Log log)
        {
            using(var ravenSession = Stores.MegaWhatsAppApi.OpenSession())
            {
                ravenSession.Store(log);
                ravenSession.SaveChanges();
            }
        }

        /// <summary>
        /// Stores the error.
        /// </summary>
        /// <param name="error"></param>
        public void StoreError(Error error)
        {
            using(var ravenSession = Stores.MegaWhatsAppApi.OpenSession())
            {
                ravenSession.Store(error);
                ravenSession.SaveChanges();
            }
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}