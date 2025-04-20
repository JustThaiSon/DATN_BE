namespace DATN_Models.Models
{
    public class UserPoint
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public long TotalPoint { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Status { get; set; }
    }
}
