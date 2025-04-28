using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Counter
{
    public class ServiceOrderDetail_DAL
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ServiceId { get; set; }
        public long Quantity { get; set; }
        public long TotalPrice { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public long UnitPrice { get; set; }
        public string ImageUrl { get; set; }
    }
}
