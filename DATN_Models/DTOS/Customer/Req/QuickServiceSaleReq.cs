using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Customer.Req
{
    public class QuickServiceSaleReq
    {
        public string ServiceListJson { get; set; }
        public Guid UserId { get; set; }
        public string CustomerEmail { get; set; }
        public bool MarkAsUsed { get; set; } = true;
    }
}
