using DATN_Helpers.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.Models
{
    public class SeatByShowTime
    {
        public Guid SeatStatusByShowTimeId { get; set; }
        public Guid SeatId { get; set; }
        public Guid ShowTimeId { get; set; }
        public SeatStatusEnum Status { get; set; }
        public long SeatPrice { get; set; }

    }
}
