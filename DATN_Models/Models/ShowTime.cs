namespace DATN_Models.Models
{
    public class ShowTime
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; } // 1 sắp chiếu 3 đang chiếu 4 đã kết thúc 5 đang có sự cố 
        public bool isDeleted { get; set; }

    }
}
