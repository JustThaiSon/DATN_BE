namespace DATN_Models.DAL.Orders
{
   public class GetPaymentDAL
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
