using PayslipProcessorDraft.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Models.Calculation
{
    //all pure functions, easy for unit testing
    /// <summary>
    /// all methods assume input logic is valid, validate input logic before calling any method in this class
    /// </summary>
    public static class PayslipCalculator
    {
        /// <summary>
        /// given a datetime, get the 6pm in that day
        /// </summary>
        /// <returns></returns>
        private static DateTime Get6Pm(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 18, 0, 0);
        }


        /// <summary>
        /// Given 2 times, calculate hours between them
        /// </summary>
        /// <param name="startTime">earlier time</param>
        /// <param name="endTime">later time</param>
        /// <returns></returns>
        public static double CalculateHours(DateTime startTime, DateTime endTime)
        {
            return (endTime - startTime).TotalHours;
        }
        public static double CalculateTotalHoursWorked(DateTime startTime, DateTime endTime)
        {
            var totalHours = CalculateHours(startTime, endTime);
            return totalHours;
        }
        /// <summary>
        /// total hour worked (excluding break) for shift
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="breakStartTime"></param>
        /// <param name="breakEndTime"></param>
        /// <returns></returns>
        public static double CalculateTotalHoursWorked(DateTime startTime, DateTime endTime, DateTime breakStartTime, DateTime breakEndTime)
        {
            var totalHours = CalculateHours(startTime, endTime);
            var breakHours = CalculateHours(breakStartTime, breakEndTime);
            return (totalHours - breakHours);
        }

        public static double CalculateOvertimeHoursWorked(DateTime startTime, DateTime endTime)
        {
            var totalHoursWorked = CalculateTotalHoursWorked(startTime, endTime);
            var overtimeHoursWorked = 0.0D;
            if (totalHoursWorked > PayslipRules.OvertimeThresholdHours)
            {
                overtimeHoursWorked = totalHoursWorked - PayslipRules.OvertimeThresholdHours;
            }

            return overtimeHoursWorked;
        }
        public static double CalculateOvertimeHoursWorked(DateTime startTime, DateTime endTime, DateTime breakStartTime, DateTime breakEndTime)
        {
            var totalHoursWorked = CalculateTotalHoursWorked(startTime, endTime, breakStartTime, breakEndTime);
            var overtimeHoursWorked = 0.0D;
            if (totalHoursWorked > PayslipRules.OvertimeThresholdHours)
            {
                overtimeHoursWorked = totalHoursWorked - PayslipRules.OvertimeThresholdHours;
            }

            return overtimeHoursWorked;
        }

        public static double CalculateOvertimeLateHoursWorked(DateTime startTime, DateTime endTime)
        {
            var overtimeHoursWorked = CalculateOvertimeHoursWorked(startTime, endTime);
            var overtimeLateHoursWorked = 0.0D;
            if (overtimeHoursWorked > PayslipRules.OvertimeLateThresholdHours)
            {
                overtimeLateHoursWorked = overtimeHoursWorked - PayslipRules.OvertimeLateThresholdHours;
            }

            return overtimeLateHoursWorked;
        }
        public static double CalculateOvertimeLateHoursWorked(DateTime startTime, DateTime endTime, DateTime breakStartTime, DateTime breakEndTime)
        {
            var overtimeHoursWorked = CalculateOvertimeHoursWorked(startTime, endTime, breakStartTime, breakEndTime);
            var overtimeLateHoursWorked = 0.0D;
            if (overtimeHoursWorked > PayslipRules.OvertimeLateThresholdHours)
            {
                overtimeLateHoursWorked = overtimeHoursWorked - PayslipRules.OvertimeLateThresholdHours;
            }

            return overtimeLateHoursWorked;
        }

        public static double CalculateAfter6PmHours(DateTime startTime, DateTime endTime)
        {
            DateTime sixPm = Get6Pm(startTime);
            if (endTime <= sixPm)   // before 6pm shift
            {
                return 0.0D;
            }
            else if (startTime < sixPm)  //cross 6pm shift
            {
                return CalculateHours(sixPm, endTime);
            }
            else  //after 6pm shift
            {
                return CalculateTotalHoursWorked(startTime, endTime);
            }
        }
        public static double CalculateAfter6PmHours(DateTime startTime, DateTime endTime, DateTime breakStartTime, DateTime breakEndTime)
        {
            DateTime sixPm = Get6Pm(startTime);
            if (endTime <= sixPm)   // before 6pm shift
            {
                return 0.0D;
            }
            else if (startTime < sixPm)  //cross 6pm shift
            {
                if (breakEndTime > sixPm)
                {
                    return CalculateHours(breakEndTime, endTime);
                }
                else
                {
                    return CalculateHours(sixPm, endTime);
                }
            }
            else  //after 6pm shift
            {
                return CalculateTotalHoursWorked(startTime, endTime, breakStartTime, breakEndTime);
            }
        }

        /// <summary>
        /// after 6pm and overtime overlap hours
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static double CalculateAfter6PmAndOvertimeHoursWorked(DateTime startTime, DateTime endTime)
        {
            var overtimeHoursWorked = CalculateOvertimeHoursWorked(startTime, endTime);
            var after6PmHoursWorked = CalculateAfter6PmHours(startTime, endTime);
            if (overtimeHoursWorked == 0.0D || after6PmHoursWorked == 0.0D)
            {
                return 0.0D;
            }
            DateTime? overtimeStartTime = CalculateOvertimeStartTime(startTime, endTime);
            if (overtimeStartTime == null)   //error proof
            {
                return 0.0D;
            }
            DateTime sixPm = Get6Pm(startTime);
            if (overtimeStartTime < sixPm)
            {
                return after6PmHoursWorked;
            }
            else
            {
                return overtimeHoursWorked;
            }
        }
        /// <summary>
        /// after 6pm and overtime overlap hours
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="breakStartTime"></param>
        /// <param name="breakEndTime"></param>
        /// <returns></returns>
        public static double CalculateAfter6PmAndOvertimeHoursWorked(DateTime startTime, DateTime endTime, DateTime breakStartTime, DateTime breakEndTime)
        {
            var overtimeHoursWorked = CalculateOvertimeHoursWorked(startTime, endTime, breakStartTime, breakEndTime);
            var after6PmHoursWorked = CalculateAfter6PmHours(startTime, endTime, breakStartTime, breakEndTime);
            if (overtimeHoursWorked == 0.0D || after6PmHoursWorked == 0.0D)
            {
                return 0.0D;
            }
            DateTime? overtimeStartTime = CalculateOvertimeStartTime(startTime, endTime, breakStartTime, breakEndTime);
            if (overtimeStartTime == null)   //error proof
            {
                return 0.0D;
            }
            DateTime sixPm = Get6Pm(startTime);
            if (overtimeStartTime < sixPm)
            {
                return after6PmHoursWorked;
            }
            else
            {
                return overtimeHoursWorked;
            }
        }

        /// <summary>
        /// hours worked excluding after 6pm or overtime
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static double CalculateNormalHoursWorked(DateTime startTime, DateTime endTime)
        {
            var totalHoursWorked = CalculateTotalHoursWorked(startTime, endTime);
            var after6PmHoursWorked = CalculateAfter6PmHours(startTime, endTime);
            var overtimeHoursWorked = CalculateOvertimeHoursWorked(startTime, endTime);
            var after6PmAndOvertimeHoursWorked = CalculateAfter6PmAndOvertimeHoursWorked(startTime, endTime);

            var normalHoursWorked = totalHoursWorked - after6PmHoursWorked - overtimeHoursWorked + after6PmAndOvertimeHoursWorked;
            return normalHoursWorked;
        }
        /// <summary>
        /// hours worked excluding after 6pm or overtime
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="breakStartTime"></param>
        /// <param name="breakEndTime"></param>
        /// <returns></returns>
        public static double CalculateNormalHoursWorked(DateTime startTime, DateTime endTime, DateTime breakStartTime, DateTime breakEndTime)
        {
            var totalHoursWorked = CalculateTotalHoursWorked(startTime, endTime, breakStartTime, breakEndTime);
            var after6PmHoursWorked = CalculateAfter6PmHours(startTime, endTime, breakStartTime, breakEndTime);
            var overtimeHoursWorked = CalculateOvertimeHoursWorked(startTime, endTime, breakStartTime, breakEndTime);
            var after6PmAndOvertimeHoursWorked = CalculateAfter6PmAndOvertimeHoursWorked(startTime, endTime, breakStartTime, breakEndTime);

            var normalHoursWorked = totalHoursWorked - after6PmHoursWorked - overtimeHoursWorked + after6PmAndOvertimeHoursWorked;
            return normalHoursWorked;
        }

        //calculate time
        public static DateTime? CalculateOvertimeStartTime(DateTime startTime, DateTime endTime)
        {
            var overtimeHoursWorked = CalculateOvertimeHoursWorked(startTime, endTime);
            if (overtimeHoursWorked == 0.0D)
            {
                return null;
            }

            return startTime.AddHours(PayslipRules.OvertimeThresholdHours);
        }
        public static DateTime? CalculateOvertimeStartTime(DateTime startTime, DateTime endTime, DateTime breakStartTime, DateTime breakEndTime)
        {
            var overtimeHoursWorked = CalculateOvertimeHoursWorked(startTime, endTime, breakStartTime, breakEndTime);
            if (overtimeHoursWorked == 0.0D)
            {
                return null;
            }
            var beforeBreakHoursWorked = CalculateHours(startTime, breakStartTime);
            if (beforeBreakHoursWorked > PayslipRules.OvertimeThresholdHours)
            {
                return startTime.AddHours(PayslipRules.OvertimeThresholdHours);
            }
            else
            {
                var breakHours = CalculateHours(breakStartTime, breakEndTime);
                return startTime.AddHours(PayslipRules.OvertimeThresholdHours + breakHours);
            }
        }

        //calculate earning
        public static decimal CalculateWeekdayEarning(WeekdayEarningOptions weekdayEarningOptions)
        {
            decimal wageBase = weekdayEarningOptions.WageBase;
            //normal hours
            double baseLoading = PayslipRules.DefaultBaseLoading;
            decimal wageNormalHours = CalculateWage(wageBase, baseLoading, weekdayEarningOptions.NormalHoursWorked);
            //after 6pm hours only (not overlap with overtime hours)
            double after6PmLoading = PayslipRules.DefaultAfter6PmLoading;
            double after6PmHoursOnly = weekdayEarningOptions.After6PmHoursWorked - weekdayEarningOptions.After6PmAndOvertimeHoursWorked;
            decimal wageAfter6PmHoursOnly = CalculateWage(wageBase, after6PmLoading, after6PmHoursOnly);
            //overtime hours only (not include overtime late part)
            double overtimeLoading = PayslipRules.DefaultOvertimeLoading;
            double overtimeHoursOnly = weekdayEarningOptions.OvertimeHoursWorked - weekdayEarningOptions.OvertimeLateHoursWorked;
            decimal wageOvertimeHoursOnly = CalculateWage(wageBase, overtimeLoading, overtimeHoursOnly);
            //overtime late hours
            double overtimeLateLoading = PayslipRules.OvertimeLateLoading;
            decimal wageOvertimeLateHours = CalculateWage(wageBase, overtimeLateLoading, weekdayEarningOptions.OvertimeLateHoursWorked);

            decimal shiftEarning = (wageNormalHours + wageAfter6PmHoursOnly + wageOvertimeHoursOnly + wageOvertimeLateHours);
            return Math.Round(shiftEarning, 2);
        }

        public static decimal CalculateSaturdayEarning(SaturdayEarningOptions saturdayEarningOptions)
        {
            decimal wageBase = saturdayEarningOptions.WageBase;
            //overtime hours only (not include overtime late part)
            double overtimeLoading = PayslipRules.DefaultOvertimeLoading;
            double overtimeHoursOnly = saturdayEarningOptions.OvertimeHoursWorked - saturdayEarningOptions.OvertimeLateHoursWorked;
            decimal wageOvertimeHoursOnly = CalculateWage(wageBase, overtimeLoading, overtimeHoursOnly);
            //overtime late hours
            double overtimeLateLoading = PayslipRules.OvertimeLateLoading;
            decimal wageOvertimeLateHours = CalculateWage(wageBase, overtimeLateLoading, saturdayEarningOptions.OvertimeLateHoursWorked);
            //normal case (no after 6pm involved)
            double baseLoading = PayslipRules.DefaultBaseLoadingSaturday;
            double normalHours = saturdayEarningOptions.TotalHoursWorked - saturdayEarningOptions.OvertimeHoursWorked;
            decimal wageNormalHours = CalculateWage(wageBase, baseLoading, normalHours);

            decimal shiftEarning = (wageNormalHours + wageOvertimeHoursOnly + wageOvertimeLateHours);
            return Math.Round(shiftEarning, 2);
        }

        public static decimal CalculateSundayEarning(SundayEarningOptions sundayEarningOptions)
        {
            decimal wageBase = sundayEarningOptions.WageBase;

            //overtime hours
            double overtimeLoading = PayslipRules.DefaultOvertimeLoadingSunday;
            decimal wageOvertimeHours = CalculateWage(wageBase, overtimeLoading, sundayEarningOptions.OvertimeHoursWorked);
            //normal case (no after 6pm involved)
            double baseLoading = PayslipRules.DefaultBaseLoadingSunday;
            double normalHours = sundayEarningOptions.TotalHoursWorked - sundayEarningOptions.OvertimeHoursWorked;
            decimal wageNormalHours = CalculateWage(wageBase, baseLoading, normalHours);

            decimal shiftEarning = (wageNormalHours + wageOvertimeHours);
            return Math.Round(shiftEarning, 2);
        }

        private static decimal CalculateWage(decimal wageBase, double loading, double hour)
        {
            var hourlyRate = CalculateHourlyRate(wageBase, loading);
            return hourlyRate * (decimal)hour;
        }
        private static decimal CalculateHourlyRate(decimal wageBase, double loading)
        {
            return wageBase * (decimal)(1 + loading);
        }
    }
}
