namespace DATN_Models.DTOS.Order.Res
{
    public class GetListHistoryOrderByUserRes
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string MovieName { get; set; }
        public string OrderCode { get; set; }
        public string CinemaName { get; set; }
        public string Thumbnail { get; set; }
        public string Address { get; set; }
        public string SessionTime { get; set; }
        public string SessionDate { get; set; }
        public string RoomName { get; set; }
        public int Status { get; set; }
        public List<string> SeatList { get; set; }
        public List<ServiceInfoModel> ServiceList { get; set; }
        public long ConcessionAmount { get; set; }
        public long TotalPrice { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class ServiceInfoModel
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public long TotalPrice { get; set; }
    }
}
