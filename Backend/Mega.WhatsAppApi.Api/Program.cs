using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mega.WhatsAppApi.Api.Extensions;
using Mega.WhatsAppApi.Infraestrutura.Servicos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mega.WhatsAppApi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var servico = new ServicoDeCliente();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .UsePort()
                    .UseStartup<Startup>());
    }
}
