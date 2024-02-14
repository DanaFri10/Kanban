using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace IntroSE.Kanban.Frontend.Models
{
    public class Response<T>
    {
        public string ErrorMessage { get; set; }
        public bool ErrorOccured { get => ErrorMessage != null; }
        public string ReturnValue { get; set; } // Return Value of the function, formatted as Json string
        public Response() 
        {
            ErrorMessage = null;
            ReturnValue = null;
        }
        public Response(string errorMessage) { ErrorMessage = errorMessage; }
        public Response(T returnValue)
        {
            ErrorMessage = null;
            //ErrorMessage = errorMessage;
            ReturnValue = JsonConvert.SerializeObject(returnValue);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static Response<T> FromJson(string jsonResponse)
        {
            return JsonConvert.DeserializeObject<Response<T>>(jsonResponse);
        }

        public T DeserializeReturnValue()
        {
            return JsonConvert.DeserializeObject<T>(ReturnValue);
        }

    }
}
