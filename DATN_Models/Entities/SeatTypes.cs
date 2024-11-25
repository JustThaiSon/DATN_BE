using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.Entities
{
    public class SeatTypes
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Status { get; set; } // 0 Chưa Được Đặt, 1 đã được đặt,2 
        public long Price { get; set; } 
    }
}
