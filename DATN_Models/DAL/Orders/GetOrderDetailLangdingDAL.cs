using DATN_Models.DTOS.Order.Res;

namespace DATN_Models.DAL.Orders
{
    public class GetOrderDetailLangdingDAL
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public string OrderCode { get; set; }
        public string CinemaName { get; set; }
        public string Address { get; set; }
        public string Thumbnail { get; set; }
        public string SessionTime { get; set; }
        public string SessionDate { get; set; }
        public string RoomName { get; set; }
        public long DiscountPrice { get; set; }
        public long TotalPriceTicket { get; set; }
        public long PointChange { get; set; }
        public List<string> SeatList { get; set; }
        public List<ServiceInfoRes> ServiceList { get; set; }
        public long ConcessionAmount { get; set; }
        public long TotalPrice { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
       
    }
}
