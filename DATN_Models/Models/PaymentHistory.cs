namespace DATN_Models.Models
{
    public class PaymentHistory
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }  
        public Guid PaymentMethodId { get; set; }  
        public long AmountPaid { get; set; }  
        public DateTime PaymentDate { get; set; } 
        public string TransactionCode { get; set; } 

    }
}
