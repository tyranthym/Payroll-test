using PayslipProcessorDraft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Services
{
    public interface IPayslipService
    {
        PayslipOutput GeneratePayslip(PayslipInput input);

        bool ValidateInputModel(PayslipInput input);
    }
}
