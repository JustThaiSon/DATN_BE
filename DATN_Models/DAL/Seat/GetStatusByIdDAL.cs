using DATN_Helpers.Constants;

namespace DATN_Models.DAL.Seat
{
    public class GetStatusByIdDAL
    {
        public Guid SeatId { get; set; }
        public SeatStatusEnum Status { get; set; }
    }
}
