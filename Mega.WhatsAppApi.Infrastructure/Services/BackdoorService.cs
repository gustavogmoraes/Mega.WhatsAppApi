using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Mega.WhatsAppApi.Infrastructure.Persistence;
using Mega.WhatsAppApi.Infrastructure.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Mega.WhatsAppApi.Infrastructure.Services
{
    public class BackdoorService
    {
        public int GetNumberOfDocumentsOnCollection(string collectionName)
        {
            using var session = Stores.MegaWhatsAppApi.OpenSession();
            return session.Query<object>(collectionName: collectionName).Count();
        }
        
        public dynamic Query(string query)
        {
            using var session = Stores.MegaWhatsAppApi.OpenSession();
            var queryresult = session.Advanced.RawQuery<dynamic>(query).ToList();

            return queryresult.Select(x => ((object)x).CastToExpando()).ToList();
        }
    }
}