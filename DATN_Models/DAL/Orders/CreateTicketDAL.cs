using DATN_Models.DTOS.Order.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Orders
{
    public class CreateTicketDAL
    {
        public long TotalPrice { get; set; }
        public int Status { get; set; }
        public int IsAnonymous { get; set; }
        public Guid PaymentId { get; set; }
        public int QuantityTicket { get; set; }
        public long TotalPriceTicket { get; set; }
        public List<TicketDAL> Tickets { get; set; }
        public List<CreateOrderServiceDAL> Services { get; set; }
    }
}
