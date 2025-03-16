using DATN_Helpers.Constants;

namespace DATN_Models.Models
{
    public class Seats
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid? SeatTypeId { get; set; }
        public Guid? PairId { get; set; }
        public string SeatName { get; set; }
        public int ColNumber { get; set; }
        public int RowNumber { get; set; }
        public long SeatPrice { get; set; }
        public SeatStatusEnum Status { get; set; }

    }
}
