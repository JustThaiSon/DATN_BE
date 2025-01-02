using DATN_Models.DAL.Seat;

namespace DATN_Models.DAO.Interface.SeatAbout
{

    public interface ISeatDAO
    {
        List<ListSeatDAL> GetListSeat(Guid id, int currentPage, int recordPerPage, out int totalRecord, out int response);
        List<ListSeatByShowTimeDAL> GetListSeatByShowTime(Guid roomId,Guid showTimeId, int currentPage, int recordPerPage, out int totalRecord, out int response);
        void UpdateSeatStatus(UpdateSeatStatusDAL dataInput, out int response);
        void UpdateSeatType(UpdateSeatTypeDAL dataInput, out int response);

    }
}
