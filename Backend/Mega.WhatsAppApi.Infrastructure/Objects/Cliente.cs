using System;
using Mega.WhatsAppApi.Infrastructure.Objects.Base;

namespace Mega.WhatsAppApi.Infrastructure.Objects
{
    /// <summary>
    /// The application client.
    /// </summary>
    public class Client : Entity
    {
        /// <summary>
        /// Client name.
        /// </summary>
        /// <value>The client name.</value>
        public string Nome { get; set; }

        /// <summary>
        /// Client unique token used for requests (Guid format).
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        /// Product Id for MaytApi.
        /// </summary>
        /// <value>The product Id.</value>
        [Obsolete]
        public string ProductId { get; set; }

        /// <summary>
        /// Phone Id for MaytApi.
        /// </summary>
        /// <value>The phone id.</value>
        [Obsolete]
        public string PhoneId { get; set; }

        /// <summary>
        /// Api key for MaytApi.
        /// </summary>
        /// <value>The api key.</value>
        [Obsolete]
        public string MaytApiKey { get; set; }
    }
}