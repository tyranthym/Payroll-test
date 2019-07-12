using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Models.Calculation
{
    public class SundayEarningOptions
    {
        public double TotalHoursWorked { get; set; }   //excluding break

        public double OvertimeHoursWorked { get; set; }

        public decimal WageBase { get; set; }

        public SundayEarningOptions(double totalHoursWorked, double overtimeHoursWorked, decimal wageBase)
        {
            TotalHoursWorked = totalHoursWorked;
            OvertimeHoursWorked = overtimeHoursWorked;
            WageBase = wageBase;
        }

    }
}
