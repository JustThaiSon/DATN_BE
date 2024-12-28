using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Order.Req
{
    public class TicketReq
    {
        public Guid ShowTimeId { get; set; }
        public Guid SeatId { get; set; }
        public long Price { get; set; }
    }
}
