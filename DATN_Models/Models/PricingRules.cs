using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.Models
{
    public class PricingRules
    {
        public Guid PricingRuleId { get; set; }
        public string RuleName { get; set; } 
        public long Multiplier { get; set; }
        public TimeSpan? StartTime { get; set; } 
        public TimeSpan? EndTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; } 
        public DateTime? Date { get; set; }
        public int? SpecialDay { get; set; }
        public int? SpecialMonth { get; set; }
        public int? DayOfWeek { get; set; } // từ 1 - 7 tương ứng với 7 ngày trong tuần
        public bool IsDeleted { get; set; }

    }
}
