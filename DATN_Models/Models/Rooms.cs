namespace DATN_Models.Models
{
    public class Rooms
    {
        public Guid Id { get; set; }
        public Guid CinemaId { get; set; }
        public string Name { get; set; }
        public int SeatsCount { get; set; }
        public int Status { get; set; }
    }
}
