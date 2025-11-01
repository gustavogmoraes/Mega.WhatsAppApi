using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Mega.WhatsAppApi.Infrastructure.Objects;
using Mega.WhatsAppApi.Infrastructure.Utils;

namespace Mega.WhatsAppApi.Infrastructure.Services
{
    /// <summary>
    /// MaytApi service class.
    /// </summary>
    [Obsolete]
    public class MaytApiService : IDisposable
    {
        private const string JsonMediaType = "application/json";
        private const string SendMessageEndpoint = "sendMessage";
        private const string LogsEndpoint = "Logs";

        private Client Client { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public MaytApiService(Client client)
        {
            Client = client;
        }

        /// <summary>
        /// Sends a message via maytApi.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [Obsolete]
        public dynamic SendMessage(dynamic message)
        {
            using var maytApiClient = GetMaytApiClient();

            return maytApiClient.PostAsync(SendMessageEndpoint, ((object)message).GetStringContent()).Result.Content
                                .ReadAsStringAsync().Result
                                .GetStringContentResult();
        }

        /// <summary>
        /// Gets maytApi logs.
        /// </summary>
        /// <returns></returns>
        public dynamic GetLogs()
        {
            using var maytApiClient = GetMaytApiClient();

            return maytApiClient.GetStringAsync(LogsEndpoint).Result
                                .GetStringContentResult();
        }

        /// <summary>
        /// Disposes.
        /// </summary>
        public void Dispose()
        {
            
        }

        private HttpClient GetMaytApiClient()
        {
            var client = new HttpClient { BaseAddress = new Uri($@"https://api.maytapi.com/api/{Client.ProductId}/{Client.PhoneId}/") };
            client.DefaultRequestHeaders.Add("x-maytapi-key", new [] { Client.MaytApiKey });
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType));

            return client;
        }
    }
}
