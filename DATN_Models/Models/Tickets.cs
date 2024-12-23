namespace DATN_Models.Models
{
    public class Tickets
    {
        public Guid Id { get; set; }
        public Guid ShowtimeId { get; set; }
        public Guid SeatId { get; set; }
        public Guid OrderDetailId { get; set; }
        public long Price { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
