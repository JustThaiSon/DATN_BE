namespace DATN_Models.Models
{
    public class Seats
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid SeaTypeId { get; set; }
        public string NameSeat { get; set; }
        public int Status { get; set; }
    }
}
