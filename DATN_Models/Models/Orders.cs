namespace DATN_Models.Models
{
    public class Orders
    {
        public Guid Id { get; set; }
        public string OrderCode { get; set; }
        public Guid? UserId { get; set; }
        public long TotalPrice { get; set; }
        public int Status { get; set; }
        public int IsAnonymous { get; set; }
        public Guid? PaymentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
