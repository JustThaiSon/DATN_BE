using DATN_Models.DAL.SeatType;

namespace DATN_Models.DAO.Interface.SeatAbout
{
    public interface ISeatTypeDAO
    {
        List<GetListSeatTypeDAL> GetListSeatType(int currentPage, int recordPerPage, out int totalRecord, out int response);
        void UpdateSeatTypeMultiplier(UpdateSeatTypeMultiplierDAL dataInput, out int response);
        void CreateSeatType(CreateSeatTypeDAL dataInput, out int response);
        void DeleteSeatType(Guid dataInput, out int response);
    }
}
