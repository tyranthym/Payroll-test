using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Models
{
    public class PayslipOutput : ResponseBase
    {
        [JsonProperty("employee")]
        public string EmployeeName { get; set; }

        [JsonProperty("totalHours")]
        public double TotalHoursWorked { get; set; }

        [JsonProperty("totalNormalHours")]
        public double TotalNormalHoursWorked { get; set; }

        [JsonProperty("totalOvertimeHours")]
        public double TotalOvertimeHoursWorked { get; set; }

        [JsonProperty("totalEarning")]
        public decimal TotalEarning { get; set; }

        [JsonProperty("shiftItems")]
        public List<ShiftOutput> ShiftOutputs { get; set; }
    }
}
