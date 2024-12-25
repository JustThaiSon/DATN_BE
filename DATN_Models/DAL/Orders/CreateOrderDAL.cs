using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Orders
{
    public class CreateOrderDAL
    {
        public long TotalPrice { get; set; }
        public int Status { get; set; }
        public int IsAnonymous { get; set; }
        public Guid PaymentId { get; set; }
    }
}
