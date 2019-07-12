using Newtonsoft.Json;
using PayslipProcessorDraft.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Models
{
    /// <summary>
    /// generally refer to errors that violate business logic rule
    /// </summary>
    public class ErrorResponse : ResponseBase
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        public ErrorResponse()
        {
            Type = ErrorTypes.General;
        }

        public ErrorResponse(string errorMessage, string errorType = ErrorTypes.General, string message = null)
        {
            Type = errorType;
            Message = message ?? errorMessage;
            if (ErrorMessages == null)
            {
                ErrorMessages = new List<string>();
            }
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ErrorMessages.Add(errorMessage);
            }
            IsSuccessful = false;
        }
    }
}
