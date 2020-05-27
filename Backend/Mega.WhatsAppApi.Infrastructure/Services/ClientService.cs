using System;
using System.Linq;
using Mega.WhatsAppApi.Infrastructure.Objects;
using Mega.WhatsAppApi.Infrastructure.Persistence;

namespace Mega.WhatsAppApi.Infrastructure.Services
{
    /// <summary>
    /// Client service class.
    /// </summary>
    public class ClientService : IDisposable
    {
        /// <summary>
        /// Queries a client by the given token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Client QueryClient(string token)
        {
            using(var ravenSession = Stores.MegaWhatsAppApi.OpenSession())
            {
                return ravenSession
                    .Query<Client>()
                    .FirstOrDefault(cliente => cliente.Token == token);
            }
        }

        /// <summary>
        /// Creates and stores a new client with the given name.
        /// </summary>
        /// <param name="clientName"></param>
        public void CreateClient(string clientName)
        {
            using(var ravenSession = Stores.MegaWhatsAppApi.OpenSession())
            {
                ravenSession.Store(new Client 
                {
                    Nome = clientName,
                    Token = Guid.NewGuid().ToString()
                });

                ravenSession.SaveChanges();
            }
        }

        /// <summary>
        /// Disposes this class.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}