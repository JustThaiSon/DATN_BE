using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.PricingRule.Req
{
    public class CreatePricingRuleReq
    {
        public string RuleName { get; set; }
        public long Multiplier { get; set; }

        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Invalid time format.")]
        public string StartTime { get; set; }

        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Invalid time format.")]
        public string EndTime { get; set; }

        [DataType(DataType.Date)]
        public string StartDate { get; set; }

        [DataType(DataType.Date)]
        public string EndDate { get; set; }

        [DataType(DataType.Date)]
        public string Date { get; set; }
        public int? SpecialDay { get; set; }
        public int? SpecialMonth { get; set; }
        public int? DayOfWeek { get; set; }
    }
}
