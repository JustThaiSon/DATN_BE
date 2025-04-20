using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Counter
{
    public class TicketAndServiceOrder_DAL
    {
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; }
        public string Email { get; set; }
        public Guid? UserId { get; set; }
        public long TotalPrice { get; set; }
        public int Status { get; set; }
        public bool IsAnonymous { get; set; }
        public long DiscountPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
