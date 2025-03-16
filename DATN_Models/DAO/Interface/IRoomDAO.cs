using DATN_Models.DAL.Room;

namespace DATN_Models.DAO.Interface
{
    public interface IRoomDAO
    {
        void CreateRoom(CreateRoomDAL resquest, out int response);
        List<ListRoomDAL> GetListRoom(int currentPage, int recordPerPage, out int totalRecord, out int response);
        void DeleteRoom(Guid Id, out int response);
    }
}
