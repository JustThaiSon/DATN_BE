
namespace DATN_Models.DAL.Orders
{
    public class CreateOrderDAL
    {
        public int Type { get; set; }
        public string Email { get; set; }
        public Guid? UserId { get; set; }
        public int IsAnonymous { get; set; }
        public Guid? PaymentId { get; set; }
        public string? TransactionCode { get; set; }
        public string? VoucherCode { get; set; }
        public long PointUse { get; set; }
        public decimal TotalPriceMethod { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPrice { get; set; }
        public List<ServiceDAL>? Services { get; set; }
        public List<TicketDAL>? Tickets { get; set; }
    }
    public class TicketDAL
    {
        public Guid SeatByShowTimeId { get; set; }
    }
    public class ServiceDAL
    {
        public Guid ServiceId { get; set; }
        public int Quantity { get; set; }
    }
}
