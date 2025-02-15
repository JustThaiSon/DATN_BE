using DATN_Models.DAL.Seat;
using DATN_Models.DAL.SeatType;

namespace DATN_Models.DAO.Interface
{

    public interface ISeatDAO
    {
        List<ListSeatDAL> GetListSeat(Guid id, int currentPage, int recordPerPage, out int totalRecord, out int response);
        List<ListSeatByShowTimeDAL> GetListSeatByShowTime(Guid roomId, Guid showTimeId, int currentPage, int recordPerPage, out int totalRecord, out int response);
        void UpdateSeatStatus(UpdateSeatStatusDAL dataInput, out int response);
        void UpdateSeatByShowTimeStatus(UpdateSeatByShowTimeStatusDAL dataInput, out int response);
        void UpdateSeatType(UpdateSeatTypeDAL dataInput, out int response);
        GetStatusByIdDAL GetStatusById(Guid Id, out int response);
        List<GetListSeatTypeDAL> GetListSeatType(int currentPage, int recordPerPage, out int totalRecord, out int response);
        void UpdateSeatTypeMultiplier(UpdateSeatTypeMultiplierDAL dataInput, out int response);
        void CreateSeatType(CreateSeatTypeDAL dataInput, out int response);
        void DeleteSeatType(Guid dataInput, out int response);
        List<ListSeatByShowTimeDAL> GetListSeatByShowTimeID(Guid showTimeId, out int totalRecord, out int response);

    }
}
