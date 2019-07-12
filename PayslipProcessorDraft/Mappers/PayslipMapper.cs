using PayslipProcessorDraft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Mappers
{
    public static class PayslipMapper
    {
        public static List<WageLevel> MapFromDictionaryToWageLevelModel(Dictionary<int, decimal> wageLevels)
        {
            List<WageLevel> wageLevelList = new List<WageLevel>();

            foreach (var wageLevel in wageLevels)
            {
                WageLevel wageLevelIndividual = new WageLevel
                {
                    Key = wageLevel.Key,
                    Value = wageLevel.Value
                };
                wageLevelList.Add(wageLevelIndividual);
            }
            return wageLevelList;
        }


        public static ShiftOutput MapFromShiftDTOToShiftOutput(ShiftDTO shiftDTO)
        {
            ShiftOutput shiftOutput = new ShiftOutput
            {
                Earning = shiftDTO.ShiftEarning,
                TotalHoursWorked = shiftDTO.TotalHoursWorked,
                BreakHours = shiftDTO.BreakHours,
                NormalHoursWorked = shiftDTO.NormalHoursWorked,
                After6PmHoursWorked = shiftDTO.After6PmHoursWorked,
                OvertimeHoursWorked = shiftDTO.OvertimeHoursWorked
            };
            return shiftOutput;
        }
    }
}
