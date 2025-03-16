
using DATN_Helpers.Constants;

namespace DATN_Models.DTOS.Room.Req
{
    public class SeatInfo
    {
        public Guid Id { get; set; }
        public string Row { get; set; } = string.Empty;
        public int Number { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Price { get; set; }
        public SeatStatusEnum Status { get; set; }
        public string? PairSeatId { get; set; }
    }
}
