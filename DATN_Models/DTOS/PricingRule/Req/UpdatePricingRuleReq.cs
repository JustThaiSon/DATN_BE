using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.PricingRule.Req
{
    public class UpdatePricingRuleReq
    {
        public Guid PricingRuleId { get; set; }
        public long Multiplier { get; set; }
    }
}
