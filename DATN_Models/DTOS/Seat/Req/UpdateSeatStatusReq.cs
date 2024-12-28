using DATN_Helpers.Constants;

namespace DATN_Models.DTOS.Seat.Req
{
    public class UpdateSeatStatusReq
    {
        public Guid Id { get; set; }
        public SeatStatusEnum Status { get; set; }
    }
}
