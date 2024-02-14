using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace IntroSE.Kanban.BackendTests
{
    public class Response
    {
        public string ErrorMessage { get; set; }
        public bool ErrorOccured { get => ErrorMessage != null; }
        public string ReturnValue { get; set; } // Return Value of the function, formatted as Json string
        public Response() { }
        public Response(string errorMessage) { ErrorMessage = errorMessage; }
        public Response(string errorMessage, object returnValue)
        {
            ErrorMessage = errorMessage;
            ReturnValue = JsonConvert.SerializeObject(returnValue);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static Response FromJson(string jsonResponse)
        {
            return JsonConvert.DeserializeObject<Response>(jsonResponse);
        }

        public T DeserializeReturnValue<T>()
        {
            return JsonConvert.DeserializeObject<T>(ReturnValue);
        }

    }
}
