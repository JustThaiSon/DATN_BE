namespace DATN_Models.Models
{
    public class MembershipPurchase
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public long MembershipId { get; set; }
        public long Amount { get; set; }
        public Guid? PaymentMethodId { get; set; } 
        public DateTime PaidAt { get; set; } = DateTime.Now;
    }

}
