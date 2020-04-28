using System;
using System.Threading.Tasks;
using Mega.WhatsAppApi.Infraestrutura.Objetos;
using Mega.WhatsAppApi.Infraestrutura.Persistencia;
using Raven.Client.Documents;

namespace Mega.WhatsAppApi.Infraestrutura.Servicos
{
    public class ServicoDeLoggingMega : IDisposable
    {
        private IDocumentStore DocumentStore  { get; set; }

        public ServicoDeLoggingMega()
        {
            DocumentStore = PersistenciaRavenDb.GetDocumentStore();
        }
        
        public void SalveLog(Log log)
        {
            using(var ravenSession = DocumentStore.OpenSession())
            {
                ravenSession.Store(log);
                ravenSession.SaveChanges();
            }
        }

        public void SalveErro(Erro erro)
        {
            using(var ravenSession = DocumentStore.OpenSession())
            {
                ravenSession.Store(erro);
                ravenSession.SaveChanges();
            }
        }

        public void Dispose()
        {
            DocumentStore.Dispose();
        }
    }
}