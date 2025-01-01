namespace DATN_Models.DTOS.Order.Req
{
    public class TicketReq
    {
        public Guid ShowTimeId { get; set; }
        public Guid SeatId { get; set; }
        public long Price { get; set; }
    }
}
