using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Counter
{
    public class TicketDetail_DAL
    {
        public Guid Id { get; set; }
        public Guid SeatByShowTimeId { get; set; }
        public string TickeCode { get; set; }
        public Guid OrderDetailId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string SeatName { get; set; }
        public string RoomName { get; set; }
        public string CinemaName { get; set; }
        public string MovieTitle { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long SeatPrice { get; set; }
    }
}
