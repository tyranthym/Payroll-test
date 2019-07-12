using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayslipProcessorDraft.Constants
{
    public static class PayslipRules
    {
        //base loading 
        public const double DefaultBaseLoading = 0.25D;
        public const double DefaultBaseLoadingSaturday = 0.5D;
        public const double DefaultBaseLoadingSunday = 0.95D;


        //after 6pm
        public const double DefaultAfter6PmLoading = 0.3D;

        //overtime 
        public const double OvertimeThresholdHours = 9;     //Hours beyond ? hours of work (excluding breaks) are considered overtime.

        public const double OvertimeLateThresholdHours = 3;  //For hours worked beyond ? hours of overtime the employee earns more loading.

        public const double DefaultOvertimeLoading = 0.75D;
        public const double DefaultOvertimeLoadingSunday = 1.25D;

        public const double OvertimeLateLoading = 1.25D;
    }
}
