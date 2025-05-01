namespace DATN_Models.Models
{
    public class OptLog
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string OtpCode { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
