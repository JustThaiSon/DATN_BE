using DATN_Models.DAL.Seat;

namespace DATN_Services.WebSockets.Interfaces
{
    interface IBookingService
    {
        Task<List<GetSeatByShowTimeDAL>> GetSeatByShowTime(Guid showtimeId);
    }
}
