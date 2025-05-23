﻿namespace DATN_Models.DTOS.PricingRule.Res
{
    public class GetListPricingRuleRes
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
        public int? DayOfWeek { get; set; }
        public bool? IsDiscount { get; set; }
    }
}
