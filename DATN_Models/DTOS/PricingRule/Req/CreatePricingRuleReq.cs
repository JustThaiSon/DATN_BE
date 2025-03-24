using System.ComponentModel.DataAnnotations;

namespace DATN_Models.DTOS.PricingRule.Req
{
    public class CreatePricingRuleReq
    {
        public string RuleName { get; set; } 
        public long Multiplier { get; set; } 
        public string? StartTime { get; set; } = null;
        public string? EndTime { get; set; } = null;
        public string? StartDate { get; set; } = null;
        public string? EndDate { get; set; } = null;
        public string? Date { get; set; } = null;
        public string? SpecialDay { get; set; } = null;
        public string? SpecialMonth { get; set; } = null;
        public string? DayOfWeek { get; set; } = null;
        public bool? IsDiscount { get; set; } // 0 or 1 ;
    }
}
