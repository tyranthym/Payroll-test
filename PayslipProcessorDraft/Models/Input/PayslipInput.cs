using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Models
{
    public class PayslipInput
    {
        [JsonProperty("employee")]
        public Employee Employee { get; set; }

        [JsonProperty("wageLevels")]
        public Dictionary<int, decimal> WageLevels { get; set; }

        [JsonProperty("shifts")]
        public List<Shift> Shifts { get; set; }

        //fluent validation validatior
        public class PayslipInputValidator : AbstractValidator<PayslipInput>
        {
            public PayslipInputValidator()
            {
                RuleFor(model => model.Employee).SetValidator(new EmployeeValidator());
                RuleFor(model => model.WageLevels).NotEmpty();
                RuleFor(model => model.Shifts).NotEmpty();
                RuleForEach(model => model.Shifts).SetValidator(new ShiftValidator());
            }
        }
    }
}
