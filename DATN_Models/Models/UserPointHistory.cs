namespace DATN_Models.Models
{
    public class UserPointHistory
    {
        public long Id { get; set; } 
        public Guid UserId { get; set; } 
        public Guid? OrderId { get; set; }
        public long PointChange { get; set; } 
        public string? Reason { get; set; } 
        public DateTime CreatedDate { get; set; } 
    }
}
