using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Counter.Res
{
    public class ServiceDetailRes
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public long UnitPrice { get; set; }
        public string FormattedUnitPrice { get; set; }
        public long TotalPrice { get; set; }
        public string FormattedTotalPrice { get; set; }
        public string ImageUrl { get; set; }
    }
}
