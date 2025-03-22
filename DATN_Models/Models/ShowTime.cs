namespace DATN_Models.Models
{
    public class ShowTime
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
        public bool  isDeleted { get; set; }
    }
}
