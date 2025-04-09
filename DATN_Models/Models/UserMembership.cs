namespace DATN_Models.Models
{
    public class UserMembership
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public long MembershipId { get; set; }
        public string MemberCode { get; set; }
        public DateTime PurchasedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int IsActive { get; set; }
        public int Status { get; set; }
    }
}
