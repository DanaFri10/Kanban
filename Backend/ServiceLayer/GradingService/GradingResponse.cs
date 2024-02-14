using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class GradingResponse<T>
    {
        public string ErrorMessage { get; set; }
        public T ReturnValue { get; set; }

        public GradingResponse() 
        {
            ErrorMessage = null;
        }

        public GradingResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public GradingResponse(string errorMessage, T returnValue)
        {
            ErrorMessage = errorMessage;
            ReturnValue = returnValue;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, this.GetType(), new JsonSerializerOptions { WriteIndented = true ,DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull});
        }

        public static GradingResponse<T> FromJson(string jsonResponse)
        {
            return JsonSerializer.Deserialize<GradingResponse<T>>(jsonResponse);
        }

    }
}
