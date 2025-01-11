namespace DATN_Models.DAL.Orders
{
    public class CreateTicketDAL
    {
        public long TotalPrice { get; set; }
        public int Status { get; set; }
        public int IsAnonymous { get; set; }
        public Guid PaymentId { get; set; }
        public long TotalPriceTicket { get; set; }
        public List<TicketDAL> Tickets { get; set; }
        public List<CreateOrderServiceDAL> Services { get; set; }
    }
}
