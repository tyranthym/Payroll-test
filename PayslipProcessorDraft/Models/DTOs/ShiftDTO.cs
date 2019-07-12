using PayslipProcessorDraft.Constants;
using PayslipProcessorDraft.Models.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Models
{
    public class ShiftDTO
    {
        public DateTime StartedAt { get; private set; }
        public DateTime EndedAt { get; private set; }
        public DayOfWeek DayOfWeek { get; private set; }
        public int WageLevel { get; private set; }

        public DateTime? BreakStartedAt { get; private set; }
        public DateTime? BreakEndedAt { get; private set; }

        public bool HasBreak { get; private set; }
        public decimal WageBase { get; private set; }

        //business logic properties
        public double BreakHours { get; private set; }

        public double TotalHoursWorked { get; private set; }     //excluding break
        public double OvertimeHoursWorked { get; private set; }
        public double OvertimeLateHoursWorked { get; private set; }
        public double After6PmHoursWorked { get; private set; }
        public double After6PmAndOvertimeHoursWorked { get; private set; }
        public double NormalHoursWorked { get; private set; }    //excluding after 6pm && overtime(only applied to weekday)

        public decimal ShiftEarning { get; private set; }
        /// <summary>
        /// only init basic properties
        /// </summary>
        /// <param name="shift"></param>
        public ShiftDTO(Shift shift)
        {
            StartedAt = shift.StartedAt;
            EndedAt = shift.EndedAt;
            DayOfWeek = shift.StartedAt.DayOfWeek;
            WageLevel = shift.WageLevel;

            if (shift.BreakStartedAt != null && shift.BreakDurationInMinutes != null)
            {
                HasBreak = true;
                BreakStartedAt = shift.BreakStartedAt;
                BreakEndedAt = shift.BreakStartedAt.GetValueOrDefault().AddMinutes(shift.BreakDurationInMinutes.GetValueOrDefault());
            }
        }

        public ShiftDTO InitWageBaseRate(decimal wageBase)
        {
            this.WageBase = wageBase;
            return this;
        }

        public ShiftDTO CalculateHoursWorked()
        {
            //no break version
            if (this.HasBreak == false)
            {
                TotalHoursWorked = PayslipCalculator.CalculateTotalHoursWorked(StartedAt, EndedAt);
                OvertimeHoursWorked = PayslipCalculator.CalculateOvertimeHoursWorked(StartedAt, EndedAt);
                OvertimeLateHoursWorked = PayslipCalculator.CalculateOvertimeLateHoursWorked(StartedAt, EndedAt);
                After6PmHoursWorked = PayslipCalculator.CalculateAfter6PmHours(StartedAt, EndedAt);
                After6PmAndOvertimeHoursWorked = PayslipCalculator.CalculateAfter6PmAndOvertimeHoursWorked(StartedAt, EndedAt);
                NormalHoursWorked = PayslipCalculator.CalculateNormalHoursWorked(StartedAt, EndedAt);
            }
            else   //break version
            {
                DateTime breakStartTime = BreakStartedAt.GetValueOrDefault();
                DateTime breakEndTime = BreakEndedAt.GetValueOrDefault();
                BreakHours = PayslipCalculator.CalculateHours(breakStartTime, breakEndTime);
                TotalHoursWorked = PayslipCalculator.CalculateTotalHoursWorked(StartedAt, EndedAt, breakStartTime, breakEndTime);
                OvertimeHoursWorked = PayslipCalculator.CalculateOvertimeHoursWorked(StartedAt, EndedAt, breakStartTime, breakEndTime);
                OvertimeLateHoursWorked = PayslipCalculator.CalculateOvertimeLateHoursWorked(StartedAt, EndedAt, breakStartTime, breakEndTime);
                After6PmHoursWorked = PayslipCalculator.CalculateAfter6PmHours(StartedAt, EndedAt, breakStartTime, breakEndTime);
                After6PmAndOvertimeHoursWorked = PayslipCalculator.CalculateAfter6PmAndOvertimeHoursWorked(StartedAt, EndedAt, breakStartTime, breakEndTime);
                NormalHoursWorked = PayslipCalculator.CalculateNormalHoursWorked(StartedAt, EndedAt, breakStartTime, breakEndTime);
            }
            return this;
        }

        public ShiftDTO CalculateEarning()
        {
            switch (this.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    var saturdayEarningOptions = new SaturdayEarningOptions(TotalHoursWorked, OvertimeHoursWorked, OvertimeLateHoursWorked, WageBase);
                    ShiftEarning = PayslipCalculator.CalculateSaturdayEarning(saturdayEarningOptions);
                    break;
                case DayOfWeek.Sunday:
                    var sundayEarningOptions = new SundayEarningOptions(TotalHoursWorked, OvertimeHoursWorked, WageBase);
                    ShiftEarning = PayslipCalculator.CalculateSundayEarning(sundayEarningOptions);
                    break;
                default:
                    var weekdayEarningOptions = new WeekdayEarningOptions
                        (NormalHoursWorked,
                        After6PmHoursWorked,
                        OvertimeHoursWorked,
                        OvertimeLateHoursWorked,
                        After6PmAndOvertimeHoursWorked,
                        WageBase);
                    ShiftEarning = PayslipCalculator.CalculateWeekdayEarning(weekdayEarningOptions);
                    break;
            }
            return this;
        }
    }
}
