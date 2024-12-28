namespace DATN_Models.DTOS.PricingRule.Req
{
    public class UpdatePricingRuleReq
    {
        public Guid PricingRuleId { get; set; }
        public long Multiplier { get; set; }
    }
}
