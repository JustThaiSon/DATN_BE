
namespace DATN_Models.DTOS.Order.Res
{
    public class GetPaymentRes
    {
        public Guid Id { get; set; }
        public string PaymentMethodName { get; set; }
        public int Status { get; set; }
        public string LogoUrl
        {
            get; set;
        }
    }
}
