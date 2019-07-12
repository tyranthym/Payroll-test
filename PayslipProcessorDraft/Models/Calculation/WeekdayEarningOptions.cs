using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Models.Calculation
{
    public class WeekdayEarningOptions
    {
        public double NormalHoursWorked { get; set; }

        public double After6PmHoursWorked { get; set; }

        public double OvertimeHoursWorked { get; set; }

        public double OvertimeLateHoursWorked { get; set; }

        public double After6PmAndOvertimeHoursWorked { get; set; }

        public decimal WageBase { get; set; }

        public WeekdayEarningOptions(double normalHoursWorked,
            double after6PmHoursWorked,
            double overtimeHoursWorked,
            double overtimeLateHoursWorked,
            double after6PmAndOvertimeHoursWorked,
            decimal wageBase)
        {
            NormalHoursWorked = normalHoursWorked;
            After6PmHoursWorked = after6PmHoursWorked;
            OvertimeHoursWorked = overtimeHoursWorked;
            OvertimeLateHoursWorked = overtimeLateHoursWorked;
            After6PmAndOvertimeHoursWorked = after6PmAndOvertimeHoursWorked;
            WageBase = wageBase;
        }

    }
}
