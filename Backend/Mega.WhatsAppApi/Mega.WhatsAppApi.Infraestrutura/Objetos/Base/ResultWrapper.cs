using System.Collections.Generic;
using System.Linq;

namespace Mega.WhatsAppApi.Infraestrutura.Objetos.Base
{
    public class ResultWrapper<T>
    {
        public string Message { get; set; }

        public IList<Error> Errors { get; set; }

        public bool IsValid { get; set; }

        public T Result { get; set; }

        public ResultWrapper(T result, string outerMessage, IList<Error> errors = null)
        {
            Result = result;
            Message = outerMessage;
            Errors = errors ?? new List<Error>();
            IsValid = !Errors.Any();
        }
    }
}
