using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.PricingRule
{
    public class UpdatePricingRuleDAL
    {
        public Guid PricingRuleId { get; set; }
        public long Multiplier { get; set; }
    }
}
