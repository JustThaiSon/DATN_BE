namespace DATN_Models.DTOS.Room.Req
{
    public class CreateRoomReq
    {
        public Guid CinemaId { get; set; }
        public Guid RoomTypeId { get; set; }
        public string Name { get; set; }
        public int TotalColNumber { get; set; }
        public int TotalRowNumber { get; set; }
        public long SeatPrice { get; set; }

    }
}
