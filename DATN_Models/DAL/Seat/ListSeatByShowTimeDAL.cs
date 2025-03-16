using DATN_Helpers.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Seat
{
    public class ListSeatByShowTimeDAL
    {
        public Guid SeatStatusByShowTimeId { get; set; }
        public Guid SeatId { get; set; }
        public Guid ShowTimeId { get; set; }
        public int SeatByShowTimeStatus { get; set; }
        public long SeatPrice { get; set; }
        public string SeatName { get; set; }
        public int RowNumber { get; set; }
        public int ColNumber { get; set; }
        public int SeatStatus { get; set; }
        public Guid? PairId { get; set; }

    }
}
