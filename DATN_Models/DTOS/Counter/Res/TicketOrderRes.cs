using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Counter.Res
{
    public class TicketOrderRes
    {
        public string OrderCode { get; set; }
        public string CustomerEmail { get; set; }
        public long TotalPrice { get; set; }
        public string FormattedTotalPrice { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public DateTime OrderDate { get; set; }
        public string FormattedOrderDate { get; set; }
        public List<TicketDetailRes> Tickets { get; set; }
        public List<ServiceDetailRes> Services { get; set; }
    }
}
