namespace DATN_Models.DTOS.Cinemas.Req
{
    public class CinemasReq
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int TotalRooms { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
