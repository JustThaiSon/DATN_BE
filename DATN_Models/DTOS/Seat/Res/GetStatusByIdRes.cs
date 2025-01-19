using DATN_Helpers.Constants;

namespace DATN_Models.DTOS.Seat.Res
{
    public class GetStatusByIdRes
    {
        public Guid SeatId { get; set; }
        public SeatStatusEnum Status { get; set; }
    }
}
