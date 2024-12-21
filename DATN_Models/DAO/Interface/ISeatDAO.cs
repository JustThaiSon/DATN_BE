using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Room;
using DATN_Models.DAL.Seat;

namespace DATN_Models.DAO.Interface
{
   
    public interface ISeatDAO
    {
        List<ListSeatDAL> GetListSeat(int currentPage, int recordPerPage, out int totalRecord, out int response);
        void UpdateSeatStatus(Guid seatId,int status, out int response);
        void UpdateSeatType(Guid seatId,Guid StatusId, out int response);

    }
}
