
namespace DATN_Models.DTOS.Order.Req
{
    public class CreateOrderReq
    {
        public string Email { get; set; }
        public Guid? UserId { get; set; }
        public int IsAnonymous { get; set; }
        public Guid? PaymentId { get; set; }
        public string? TransactionCode { get; set; }
        public string? VoucherCode { get; set; }
        public long PointUse { get; set; }
        public decimal TotalDiscount { get; set; }

        public decimal TotalPriceMethod { get; set; }
        public List<ServiceReq>? Services { get; set; }
        public List<TicketReq>? Tickets { get; set; }
    }
    public class TicketReq
    {
        public Guid SeatByShowTimeId { get; set; }
    }
    public class ServiceReq
    {
        public Guid ServiceId { get; set; }
        public int Quantity { get; set; }
    }
}
