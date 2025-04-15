namespace DATN_Models.DTOS.Order.Res
{
    public class GetOrderDetailLangdingRes
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }
        public string Duration { get; set; }
        public string OrderCode { get; set; }
        public byte[] OrderCodeB64 { get; set; }
        public string Description { get; set; }
        public string CinemaName { get; set; }
        public string Thumbnail { get; set; }
        public long PointChange { get; set; }
        public string Address { get; set; }
        public string SessionTime { get; set; }
        public string sessionDate { get; set; }
        public long DiscountPrice { get; set; }
        public long TotalPriceTicket { get; set; }
        public string RoomName { get; set; }
        public List<string> SeatList { get; set; }
        public List<ServiceInfoRes> ServiceList { get; set; }
        public long ConcessionAmount { get; set; }
        public long TotalPrice { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class ServiceInfoRes
    {
        public string ServiceTypeName { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public long TotalPrice { get; set; }
    }

}
