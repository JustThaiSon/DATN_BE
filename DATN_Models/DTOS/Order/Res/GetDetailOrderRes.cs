namespace DATN_Models.DTOS.Order.Res
{
    public class GetDetailOrderRes
    {
        public string UserName { get; set; }
        public long TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OrderCode { get; set; }
        public int QuantityTicket { get; set; }
        public int TotalPriceTicket { get; set; }
        public List<GetListTicketRes> ListTicket { get; set; }
    }
}
