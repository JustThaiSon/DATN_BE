namespace DATN_Models.DTOS.Order.Req
{
    public class TicketReq
    {
        public Guid SeatByShowTimeId { get; set; }
        public long Price { get; set; }
    }
}
