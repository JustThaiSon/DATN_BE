namespace DATN_Models.DTOS.ShowTime.Res
{
    public class ShowTimeRes
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
        public string MovieName { get; set; }
        public string RoomName { get; set; }
        public int Capacity { get; set; }
    }
}
