using DATN_Helpers.Constants;

namespace DATN_Models.DAL.Seat
{
    public class UpdateSeatByShowTimeStatusDAL
    {
        public Guid Id { get; set; }
        public SeatStatusEnum Status { get; set; }
    }
}
