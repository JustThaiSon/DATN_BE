using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Room;
using DATN_Models.DAL.Seat;

namespace DATN_Models.DAO.Interface
{
   
    public interface ISeatDAO
    {
        List<ListSeatDAL> GetListSeat(Guid id,int currentPage, int recordPerPage, out int totalRecord, out int response);
        void UpdateSeatStatus(UpdateSeatStatusDAL dataInput, out int response);
        void UpdateSeatType(UpdateSeatTypeDAL dataInput, out int response);

    }
}
