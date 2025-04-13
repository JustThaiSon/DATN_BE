namespace DATN_Models.Models
{
    public class PaymentMethod
    {
        public Guid Id { get; set; }
        public string PaymentMethodName { get; set; }
        public int Status { get;set; }
        public string LogoUrl { get; set; }
    }
}
