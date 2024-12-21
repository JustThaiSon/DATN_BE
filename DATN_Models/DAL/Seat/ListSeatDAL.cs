using DATN_Helpers.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Seat
{
    public class ListSeatDAL
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid SeatTypeId { get; set; }
        public string SeatName { get; set; }
        public int ColNumber { get; set; }
        public int RowNumber { get; set; }
        public SeatStatusEnum Status { get; set; }
    }
}
