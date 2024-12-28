namespace DATN_Models.DTOS.Cinemas.Res
{
    public class CinemasRes
    {
        public Guid CinemasId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int TotalRooms { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
