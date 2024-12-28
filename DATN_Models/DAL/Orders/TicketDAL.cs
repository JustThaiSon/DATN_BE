using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Orders
{
    public class TicketDAL
    {
        public Guid ShowTimeId { get; set; }
        public Guid SeatId { get; set; }
        public long Price { get; set; }
    }
}
