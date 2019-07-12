using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Models
{
    public class Shift
    {
        [JsonProperty("start")]
        public DateTime StartedAt { get; set; }
        [JsonProperty("end")]
        public DateTime EndedAt { get; set; }
        [JsonProperty("wageLevel")]
        public int WageLevel { get; set; }

        [JsonProperty("breakStart")]
        public DateTime? BreakStartedAt { get; set; }
        [JsonProperty("breakDurationMinutes")]
        public int? BreakDurationInMinutes { get; set; }
    }

    //fluent validation validatior
    public class ShiftValidator : AbstractValidator<Shift>
    {
        public ShiftValidator()
        {
            RuleFor(model => model.StartedAt).NotEmpty();
            RuleFor(model => model.EndedAt).NotEmpty();
            RuleFor(model => model.WageLevel).NotEmpty();
            RuleFor(model => model.BreakStartedAt).NotEmpty().When(model => model.BreakDurationInMinutes != null);
            RuleFor(model => model.BreakDurationInMinutes).NotEmpty().GreaterThan(0).When(model => model.BreakStartedAt != null);

        }
    }

}
