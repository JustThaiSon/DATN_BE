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
        public bool IsDeleted { get; set; }

    }
}
