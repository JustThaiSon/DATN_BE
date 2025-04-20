namespace DATN_Models.Models
{
    public class UserServiceHistory
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; } 
        public DateTime UsedDate { get; set; } 
        public long? MembershipId { get; set; }
    }
}
