namespace DATN_Models.Models
{
    public class Orders
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public long TotalPrice { get; set; }
        public int Status { get; set; }
        public Guid PaymentId { get; set; }
    }
}
