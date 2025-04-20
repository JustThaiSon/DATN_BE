namespace DATN_Models.Models
{
    public class PendingPointRefund
    {
        public long Id { get; set; } 
        public string Email { get; set; }
        public Guid OrderId { get; set; }
        public long Points { get; set; } 
        public int IsClaimed { get; set; }
        public Guid? ClaimedUserId { get; set; } 
        public DateTime CreatedDate { get; set; }
    }
}
