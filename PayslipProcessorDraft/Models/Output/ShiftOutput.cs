using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Models
{
    public class ShiftOutput
    {
        [JsonProperty("earning")]
        public decimal Earning { get; set; }

        [JsonProperty("hoursWorked")]
        public double TotalHoursWorked { get; set; }    //exclude break hours

        [JsonProperty("breakHours")]
        public double BreakHours { get; set; }

        [JsonProperty("normalHours")]
        public double NormalHoursWorked { get; set; }     //before 6pm

        [JsonProperty("after6PmHous")]
        public double After6PmHoursWorked { get; set; }   //after 6pm & overtime may overlap 

        [JsonProperty("overtimeHours")]
        public double OvertimeHoursWorked { get; set; }
    }
}
