using System.Security;
using System.Collections.Generic;
using System;
using Raven.Client.Documents;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Mega.WhatsAppApi.Infrastructure.Utils;

namespace Mega.WhatsAppApi.Infrastructure.Persistence
{
    /// <summary>
    /// Class responsible for managing all that is persisted on databases.
    /// </summary>
    public static class Stores
    {
        private const string UrlCloud = @"https://a.free.gsoftware.ravendb.cloud/";
        private const string CertificateFileName = "free.gsoftware.client.certificate.with.password.pfx";
        private const string CertificatePassword = "8FF4A485E3D110558EF44DAA5347761E";
        private static string MainDatabase = Utils.Extensions.EnvironmentIsDevelopment() ? "Mega.WhatsAppApi.Dev" : "Mega.WhatsAppApi";
        
        /// <summary>
        /// Gets main document store.
        /// </summary>
        /// <returns>Return this document store.</returns>
        public static IDocumentStore MegaWhatsAppApi => DocumentStores[MainDatabase] ?? CreateNewDocumentStore(MainDatabase);

        /// <summary>
        /// The document stores.
        /// </summary>
        /// <value>Returns the document stores dictionaries.</value>
        private static Dictionary<string, IDocumentStore> DocumentStores { get; set; }

        #region Constructor

        static Stores() => DocumentStores = new Dictionary<string, IDocumentStore>();
        
        #endregion

        #region Private Methods

        private static IDocumentStore CreateNewDocumentStore(string databaseName)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var certificatePath = Path.Combine(baseDirectory, CertificateFileName);
            
            var documentStore = new DocumentStore
            {
                Urls = new[] { UrlCloud },
                Database = databaseName,
                Certificate = new X509Certificate2(certificatePath, CertificatePassword.ToSecureString())
            };

            documentStore.Initialize();

            DocumentStores.Add(databaseName, documentStore);
            return documentStore;
        }

        #endregion
    }
}
