using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Counter.Req
{
    public class PaymentConfirmationReq
    {
        public string OrderCode { get; set; }
        // Đã bỏ PaymentMethodId
        public Guid? UserId { get; set; }
    }
}
