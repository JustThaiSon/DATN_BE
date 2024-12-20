namespace DATN_Models.Models
{
    public class OrderDetails
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public Guid OrderDetailId { get; set; }
        public int Quantity { get; set; }
        public long UnitPrice { get; set; }
        public long TotalPrice { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
