using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Models.Calculation
{
    public class SaturdayEarningOptions
    {
        public double TotalHoursWorked { get; set; }   //excluding break

        public double OvertimeHoursWorked { get; set; }

        public double OvertimeLateHoursWorked { get; set; }

        public decimal WageBase { get; set; }

        public SaturdayEarningOptions(double totalHoursWorked, double overtimeHoursWorked, double overtimeLateHoursWorked, decimal wageBase)
        {
            TotalHoursWorked = totalHoursWorked;
            OvertimeHoursWorked = overtimeHoursWorked;
            OvertimeLateHoursWorked = overtimeLateHoursWorked;
            WageBase = wageBase;
        }

    }
}
