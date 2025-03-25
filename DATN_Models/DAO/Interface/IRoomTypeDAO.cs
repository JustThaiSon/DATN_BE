using DATN_Models.DAL.RoomType;
using DATN_Models.DAL.SeatType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAO.Interface
{
   public interface IRoomTypeDAO
    {
        List<GetListRoomTypeDAL> GetListRoomType(int currentPage, int recordPerPage, out int totalRecord, out int response);
        void CreateRoomType(string dataInput, out int response);
        void DeleteRoomType(Guid dataInput, out int response);

    }
}
