namespace DATN_Models.Models
{
    public class Tickets
    {
        public Guid Id { get; set; }
        public Guid SeatByShowTimeId { get; set; }
        public string TickeCode { get; set; }
        public Guid OrderDetailId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
