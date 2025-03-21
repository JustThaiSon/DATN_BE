namespace DATN_Models.DAL.Orders
{
    public class GetDetailOrderDAL
    {
        public string UserName { get; set; }
        public long TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OrderCode { get; set; }
        public int QuantityTicket { get; set; }
        public long TotalPriceTicket { get; set; }
        public List<GetListTicketDAL> ListTicket { get; set; }
    }
}
