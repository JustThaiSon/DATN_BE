namespace DATN_Models.DTOS.Order.Res
{
    public class GetInfoRefundRes
    {
        public string Email { get; set; } 
        public long PointRefund { get; set; }
        public Guid ShowTimeId { get; set; }
        public string SeatStatusByShowTimeIds { get; set; } 
    }
}
