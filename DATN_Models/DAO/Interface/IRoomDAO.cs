using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Room;
using DATN_Models.DTOS.Room.Res;

namespace DATN_Models.DAO.Interface
{
    public interface IRoomDAO
    {
        void CreateRoom(CreateRoomDAL resquest, out int response);
        void UpdateRoom(UpdateRoomDAL req, out int response);
        List<ListRoomDAL> GetListRoom(int currentPage, int recordPerPage, out int totalRecord, out int response);
        void DeleteRoom(Guid Id, out int response);

        List<ListRoomByCinemaDAL> GetListRoomByCinema(Guid CinemaID, out int response);
    }
}
