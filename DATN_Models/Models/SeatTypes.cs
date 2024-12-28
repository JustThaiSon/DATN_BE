namespace DATN_Models.Models
{
    public class SeatTypes
    {
        public Guid Id { get; set; }
        public string SeatTypeName { get; set; }
        public long Multiplier { get; set; }
        public bool IsDeleted { get; set; }
    }
}
