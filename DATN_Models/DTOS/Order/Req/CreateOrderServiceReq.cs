namespace DATN_Models.DTOS.Order.Req
{
    public class CreateOrderServiceReq
    {
        public Guid ServiceId { get; set; }
        public int Quantity { get; set; }
    }
}

