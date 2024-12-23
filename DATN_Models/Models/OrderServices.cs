using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.Models
{
    public class OrderServices
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ServiceId { get; set; }
        public long Quantity { get; set; }
        public long TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
