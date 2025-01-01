namespace DATN_Models.DTOS.Order.Req
{
    public class CreateTicketReq
    {
        public long TotalPrice { get; set; }
        public int Status { get; set; }
        public int IsAnonymous { get; set; }
        public Guid PaymentId { get; set; }
        public int QuantityTicket { get; set; }
        public long TotalPriceTicket { get; set; }
        public List<TicketReq> Tickets {  get; set; }
        public List<CreateOrderServiceReq> Services { get; set; }
    }
}
