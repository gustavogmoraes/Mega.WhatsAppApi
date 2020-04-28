using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Mega.WhatsAppApi.Infraestrutura.Objetos;
using Mega.WhatsAppApi.Infraestrutura.Utils;
using Newtonsoft.Json;

namespace Mega.WhatsAppApi.Infraestrutura.Servicos
{
    public class ServicoMaytApi : IDisposable
    {
        private Cliente Client { get; set; }
        public ServicoMaytApi(Cliente client)
        {
            Client = client;
        }

        public dynamic SendMessage(dynamic message)
        {
            using var maytApiClient = GetMaytApiClient();

            return maytApiClient.PostAsync(SendMessageEndpoint, ((object)message).GetStringContent()).Result.Content
                                .ReadAsStringAsync().Result
                                .GetStringContentResult();
        }

        public dynamic Logs()
        {
            using var maytApiClient = GetMaytApiClient();

            return maytApiClient.GetStringAsync(LogsEndpoint).Result
                                .GetStringContentResult();
        }

        public void Dispose()
        {
            
        }

        private const string JsonMediaType = "application/json";
        private const string SendMessageEndpoint = "sendMessage";
        private const string LogsEndpoint = "Logs";

        private HttpClient GetMaytApiClient()
        {
            var client = new HttpClient { BaseAddress = new Uri($@"https://api.maytapi.com/api/{Client.ProductId}/{Client.PhoneId}/") };
            client.DefaultRequestHeaders.Add("x-maytapi-key", new [] { Client.MaytApiKey });
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType));

            return client;
        }
    }
}
