namespace DATN_Models.DTOS.ShowTime.Req
{
    public class ShowTimeReq
    {
        public Guid MovieId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
    }
}
