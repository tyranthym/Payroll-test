using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Models
{
    public interface IResponseBase
    {
        string Message { get; set; }               //response message
        List<string> ErrorMessages { get; set; }   //response errors if there is any
        bool IsSuccessful { get; set; }
    }

    public class ResponseBase : IResponseBase
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("errors")]
        public List<string> ErrorMessages { get; set; } = new List<string>();
        [JsonProperty("success")]
        public bool IsSuccessful { get; set; }
    }
}
