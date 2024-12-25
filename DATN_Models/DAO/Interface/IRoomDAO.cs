using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Room;
using DATN_Models.DTOS.Movies.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAO.Interface
{
    public interface IRoomDAO
    {
        void CreateRoom(CreateRoomDAL resquest, out int response);
        List<ListRoomDAL> GetListRoom(int currentPage, int recordPerPage, out int totalRecord, out int response);
    }
}
