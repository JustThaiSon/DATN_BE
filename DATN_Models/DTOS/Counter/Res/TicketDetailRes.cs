using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Counter.Res
{
    public class TicketDetailRes
    {
        public string TicketCode { get; set; }
        public string MovieTitle { get; set; }
        public string CinemaName { get; set; }
        public string RoomName { get; set; }
        public string SeatName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string FormattedStartTime { get; set; }
        public string FormattedEndTime { get; set; }
        public long SeatPrice { get; set; }
        public string FormattedSeatPrice { get; set; }
    }
}
