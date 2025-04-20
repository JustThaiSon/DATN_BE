using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Counter.Req
{
    public class CancelOrderReq
    {
        public string OrderCode { get; set; }
        public Guid? UserId { get; set; }
    }
}
