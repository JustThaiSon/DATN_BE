using DATN_Models.DAL.ShowTime;
using DATN_Models.DTOS.ShowTime.Req;

namespace DATN_Models.DAO.Interface
{
    public interface IShowTimeDAO
    {
        void CreateShowTime(ShowTimeReq request, out int response);
        void UpdateShowTime(Guid showTimeId, UpdateShowTimeReq request, out int response);
        void UpdateShowTimeStatus(Guid showTimeId, int status, out int response);
        void DeleteShowTime(Guid showTimeId, out int response);
        List<ShowTimeDAL> GetListShowTimes(int currentPage, int recordPerPage, out int totalRecord, out int response);
        ShowTimeDAL GetShowTimeById(Guid showTimeId, out int response);
        List<AvailableRoomDAL> GetAvailableRooms(DateTime startTime, DateTime endTime, out int response);
        List<TimeSlotDAL> GetAvailableTimes(Guid roomId, DateTime date, out int response);
    }
}
