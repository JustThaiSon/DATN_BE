
using DATN_Helpers.Constants;

namespace DATN_Models.DTOS.Entities
{
    public class SeatStatusByShow
    {
        public Guid Id { get; set; }
        public SeatStatusEnum Status { get; set; }
    }
}
