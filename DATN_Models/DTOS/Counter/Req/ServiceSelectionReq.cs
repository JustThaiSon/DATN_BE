using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Counter.Req
{
    public class ServiceSelectionReq
    {
        public Guid ServiceId { get; set; }
        public int Quantity { get; set; }
    }
}
