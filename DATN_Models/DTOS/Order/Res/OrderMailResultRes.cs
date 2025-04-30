using DATN_Models.DAL.Orders;


namespace DATN_Models.DTOS.Order.Res
{
    public class OrderMailResultRes
    {
        public string MovieName { get; set; }
        public string OrderCode { get; set; }
        public string CinemaName { get; set; }
        public string Address { get; set; }
        public string ShowTime { get; set; }
        public string ShowDate { get; set; }
        public string RoomName { get; set; }
        public string Duration { get; set; }
        public string SeatList { get; set; }
        public string GenreString { get; set; }
        public List<ServiceDetails> ServiceDetails { get; set; }
        public long ConcessionAmount { get; set; }
        public long TotalPrice { get; set; }
        public long TotalPriceTicket { get; set; }
        public long DiscountPrice { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? MinimumAge { get; set; }
        public string Thumbnail { get; set; }
        public long PointChange { get; set; }
        public string TransactionCode { get; set; }
        public string PaymentMethodName { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
