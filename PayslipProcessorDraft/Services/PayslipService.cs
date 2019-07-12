using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PayslipProcessorDraft.Constants;
using PayslipProcessorDraft.Mappers;
using PayslipProcessorDraft.Models;
using PayslipProcessorDraft.Models.Calculation;

namespace PayslipProcessorDraft.Services
{
    public class PayslipService : IPayslipService
    {
        PayslipOutput IPayslipService.GeneratePayslip(PayslipInput input)
        {
            List<WageLevel> wageLevels = PayslipMapper.MapFromDictionaryToWageLevelModel(input.WageLevels);
            PayslipOutput payslipOutput = new PayslipOutput
            {
                EmployeeName = input.Employee?.Name,
                ShiftOutputs = new List<ShiftOutput>()
            };
            foreach (var shift in input.Shifts)
            {
                var shiftOutput = ProcessShift(shift, wageLevels);
                payslipOutput.ShiftOutputs.Add(shiftOutput);
            }

            payslipOutput.TotalHoursWorked = payslipOutput.ShiftOutputs.Sum(so => so.TotalHoursWorked);
            payslipOutput.TotalNormalHoursWorked = payslipOutput.ShiftOutputs.Sum(so => so.NormalHoursWorked);
            payslipOutput.TotalOvertimeHoursWorked = payslipOutput.ShiftOutputs.Sum(so => so.OvertimeHoursWorked);
            payslipOutput.TotalEarning = payslipOutput.ShiftOutputs.Sum(so => so.Earning);
            //init response properties
            payslipOutput.Message = "payslip has been successfully generated";
            payslipOutput.IsSuccessful = true;
            return payslipOutput;
        }

        private ShiftOutput ProcessShift(Shift shift, List<WageLevel> wageLevels)
        {
            ShiftDTO shiftDTO = new ShiftDTO(shift);

            WageLevel wageLevel = wageLevels.FirstOrDefault(wl => wl.Key == shiftDTO.WageLevel);
            if (wageLevel == null)
            {
                throw new ArgumentNullException($"wageLevel in shift started at: [{shift.StartedAt}] cannot be found.");
            }
            decimal wageBase = wageLevel.Value;
            //init shiftDTO
            shiftDTO.InitWageBaseRate(wageBase).CalculateHoursWorked().CalculateEarning();
            //map to output
            ShiftOutput shiftOutput = PayslipMapper.MapFromShiftDTOToShiftOutput(shiftDTO);
            return shiftOutput;
        }

        bool IPayslipService.ValidateInputModel(PayslipInput input)
        {
            var shifts = input.Shifts;
            List<int> wageLevels = input.WageLevels.Select(wl => wl.Key).ToList();
            foreach (var shift in shifts)
            {
                //validate datetime 
                if (shift.EndedAt <= shift.StartedAt)
                {
                    return false;
                }
                if (shift.BreakStartedAt != null && shift.BreakDurationInMinutes != null)
                {
                    var breakEndedAt = shift.BreakStartedAt.GetValueOrDefault().AddMinutes(shift.BreakDurationInMinutes.GetValueOrDefault());
                    if (shift.BreakStartedAt <= shift.StartedAt || breakEndedAt >= shift.EndedAt)
                    {
                        return false;
                    }
                }
                //check wage level exists
                if (!wageLevels.Contains(shift.WageLevel))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
