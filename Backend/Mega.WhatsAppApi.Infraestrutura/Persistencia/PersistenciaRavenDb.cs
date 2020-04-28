using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Mega.WhatsAppApi.Infraestrutura.Utils;
using Raven.Client.Documents;

namespace Mega.WhatsAppApi.Infraestrutura.Persistencia
{
    public static class PersistenciaRavenDb
    {
        private const string UrlCloud = @"https://a.free.gsoftware.ravendb.cloud/";
        private const string DatabaseName = "Mega.WhatsAppApi";
        private const string CertificateFileName = "free.gsoftware.client.certificate.with.password.pfx";
        private const string CertificatePassword = "8FF4A485E3D110558EF44DAA5347761E";

        public static IDocumentStore GetDocumentStore()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var certificatePath = Path.Combine(baseDirectory, CertificateFileName);
            
            var ds = new DocumentStore
            {
                Urls = new[] { UrlCloud },
                Database = DatabaseName,
                Certificate = new X509Certificate2(certificatePath, CertificatePassword.ToSecureString())
            };

            ds.Initialize();
            return ds;
        }
    }
}
