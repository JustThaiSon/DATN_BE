using DATN_Helpers.Constants;

namespace DATN_Models.Models
{
    public class Rooms
    {
        public Guid Id { get; set; }
        public Guid CinemaId { get; set; }
        public string Name { get; set; }
        public int TotalColNumber { get; set; }
        public int TotalRowNumber { get; set; }
        public long SeatPrice { get; set; }
        public bool Isdeleted { get; set; }
        public RoomStatusEnum Status { get; set; }
    }
}
