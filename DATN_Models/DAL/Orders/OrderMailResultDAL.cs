namespace DATN_Models.DAL.Orders
{
    public class OrderMailResultDAL
    {
        public string MovieName { get; set; }
        public string OrderCode { get; set; }
        public string CinemaName { get; set; }
        public string Address { get; set; }
        public string SessionTime { get; set; }
        public string RoomName { get; set; }
        public string SeatList { get; set; }
        public long ConcessionAmount { get; set; }
        public long TotalPrice { get; set; }
        public long DiscountPrice { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public long PointChange { get; set; }
    }
}
