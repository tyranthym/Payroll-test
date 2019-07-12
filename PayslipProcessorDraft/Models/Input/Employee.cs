using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Models
{
    public class Employee
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    //fluent validation validatior
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(model => model.Name).NotEmpty().MaximumLength(50);
        }
    }
}
