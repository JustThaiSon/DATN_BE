using DATN_Helpers.Constants;

namespace DATN_Models.DTOS.Room.Res
{
    public class GetListRoomRes
    {
        public Guid Id { get; set; }
        public Guid CinemaId { get; set; }
        public Guid RoomTypeId { get; set; }
        public string Name { get; set; }
        public int TotalColNumber { get; set; }
        public int TotalRowNumber { get; set; }
        public long SeatPrice { get; set; }
        public int TotalSeats { get; set; }
        public bool Isdeleted { get; set; }
        public RoomStatusEnum Status { get; set; }
    }

    public class GetListRoomByCinemaRes
    {
        public Guid Id { get; set; }
        public Guid CinemasId { get; set; }
        public string Name { get; set; }
        public string RoomType { get; set; }
    }
}
