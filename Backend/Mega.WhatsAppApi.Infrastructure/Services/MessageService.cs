using System;
using Mega.WhatsAppApi.Domain.Objects;
using System.Linq;
using Mega.WhatsAppApi.Infrastructure.Objects;
using Mega.WhatsAppApi.Infrastructure.Validators;
using Mega.WhatsAppApi.Infrastructure.Converters;

namespace Mega.WhatsAppApi.Infrastructure.Services
{
    /// <summary>
    /// Message service class.
    /// </summary>
    public class MessageService : IDisposable 
    {
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

            using var converter = new MessageConverter();
            using var maytApiService = new MaytApiService(Client);

            var resultMaytApi = maytApiService.SendMessage(converter.ConvertMessage(message));

            return new ResultadoRequest
            {
                Mensagem = "Mensagem enviada com sucesso!"
            };
        }

        /// <summary>
        /// Disposes the class;
        /// </summary>
        public void Dispose()
        {
        }
    }
}
