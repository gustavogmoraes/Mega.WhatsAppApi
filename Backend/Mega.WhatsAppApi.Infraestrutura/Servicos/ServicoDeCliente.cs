using System.IO;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using Mega.WhatsAppApi.Infraestrutura.Objetos;
using Mega.WhatsAppApi.Infraestrutura.Persistencia;
using Mega.WhatsAppApi.Infraestrutura.Utils;
using Raven.Client.Documents;

namespace Mega.WhatsAppApi.Infraestrutura.Servicos
{
    public class ServicoDeCliente : IDisposable
    {
        
        private IDocumentStore DocumentStore  { get; set; }

        public ServicoDeCliente()
        {
            DocumentStore = PersistenciaRavenDb.GetDocumentStore();
        }

        public void Dispose()
        {
            DocumentStore.Dispose();
        }
    
        public void CrieCliente(string nomeDoCliente)
        {
            using(var ravenSession = DocumentStore.OpenSession())
            {
                ravenSession.Store(new Cliente 
                {
                    Nome = nomeDoCliente,
                    Token = Guid.NewGuid().ToString()
                });

                ravenSession.SaveChanges();
            }
        }

        public Cliente ConsulteCliente (string token)
        {
            using(var ravenSession = DocumentStore.OpenSession())
            {
                return ravenSession
                    .Query<Cliente>()
                    .FirstOrDefault(cliente => cliente.Token == token);
            }
        }
    }
}