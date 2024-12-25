namespace DATN_Models.Models
{
    public class OrderDetails
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
        public long TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
