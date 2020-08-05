using System;
using System.Collections.Generic;
using Mega.WhatsAppApi.Domain.Objects;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Mega.SmsAutomator.Objects;
using Mega.WhatsAppApi.Infrastructure.Objects;
using Mega.WhatsAppApi.Infrastructure.Validators;
using Mega.WhatsAppApi.Infrastructure.Converters;
using Mega.WhatsAppApi.Infrastructure.Persistence;
using Mega.WhatsAppApi.Infrastructure.Utils;

namespace Mega.WhatsAppApi.Infrastructure.Services
{
    /// <summary>
    /// Message service class.
    /// </summary>
    public class MessageService : IDisposable 
    {
        private const string JsonMediaType = "    ";
        private const string SendMessageEndpoint = "sendMessage";
        
        private Client Client { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="client"></param>
        public MessageService(Client client)
        {
            Client = client;
        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResultadoRequest SendMessage(Mensagem message)
        {
            // Validation block
            using var validator = new MessageValidator();

            var validationResult = validator.ValidateMessage(message);
            if(validationResult.Any())
            {
                return new ResultadoRequest
                {
                    Mensagem = "Foi encontrada uma inconsistência na mensagem, avalie a lista de inconsistências para mais informações!",
                    Inconsistencias = validationResult
                };
            }
            //

             
            // using var maytApiService = new MaytApiService(Client);
            //
            // var resultMaytApi = maytApiService.SendMessage(converter.ConvertMessage(message));
            
            using var converter = new MessageConverter();
            var convertedMessage = converter.ConvertMessageToMegaAuto(message);

            using (var session = Stores.MegaWhatsAppApi.OpenSession())
            {
                session.Store(new ToBeSent
                {
                    EntryTime = DateTime.UtcNow.ToBraziliaDateTime(),
                    Message = new Message
                    {
                        Number = (string)convertedMessage.Number,
                        Text = (string)convertedMessage.Text
                    }
                });
                
                session.SaveChanges();
            }
            
            // var obj = ((object) convertedMessage).GetStringContent();
            //  using var client = GetMegaAutomatorClient();
            //
            //  var result = client.PostAsync("SendMessage", obj)
            //      .Result.Content
            //      .ReadAsStringAsync().Result
            //      .GetStringContentResult();

            return new ResultadoRequest
            {
                Mensagem = "Mensagem enfileirada com sucesso!"
            };
        }
        
        private HttpClient GetMegaAutomatorClient()
        {
            var client = new HttpClient { BaseAddress = new Uri($@"{Client.MegaAutomatorUrl}/api/Message/") };
            client.DefaultRequestHeaders.Add("ClientToken",new [] { "adminrjdta" }); //new [] { Client.Token });
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType));

            return client;
        }

        /// <summary>
        /// Disposes the class;
        /// </summary>
        public void Dispose()
        {
        }

        public List<ToBeSent> GetSmsToBeSent()
        {
            using(var session = Stores.MegaWhatsAppApi.OpenSession())
            {
                return session.Query<ToBeSent>()
                    .OrderBy(x =>  x.EntryTime)
                    .Take(50)
                    .ToList();
            }
        }

        public void AddSentMessages(List<ToBeSent> sentMessages)
        {    
            using(var session = Stores.MegaWhatsAppApi.OpenSession())
            {
                sentMessages.ForEach(x => session.Delete(x.Id)); 
                session.Advanced.DocumentStore.MassInsert(sentMessages.Select(x => new Sent
                {
                    Message = x.Message,
                    TimeSent = DateTime.UtcNow.ToBraziliaDateTime() 
                }).ToList(), processLoopOnDatabase: true);
                
                session.SaveChanges();
            }
        }
    }
}
