using DATN_Helpers.Constants;

namespace DATN_Models.Models
{
    public class Cinemas
    {
        public Guid CinemasId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int TotalRooms { get; set; }
        public CinemaStatusEnum Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
