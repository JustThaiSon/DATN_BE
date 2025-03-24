namespace DATN_Models.DTOS.ShowTime.Req
{
    public class UpdateShowTimeReq
    {
        public Guid RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
} 