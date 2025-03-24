namespace DATN_Models.DAL.PricingRule
{
    public class CreatePricingRuleDAL
    {
        public string RuleName { get; set; }
        public long Multiplier { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Date { get; set; }
        public string? SpecialDay { get; set; }
        public string? SpecialMonth { get; set; }
        public string? DayOfWeek { get; set; }
        public bool? IsDiscount { get; set; } // 0 or 1
    }
}
