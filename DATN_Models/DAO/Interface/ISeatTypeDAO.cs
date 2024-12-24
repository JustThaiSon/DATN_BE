using DATN_Models.DAL.Seat;
using DATN_Models.DAL.SeatType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAO.Interface
{
    public interface ISeatTypeDAO
    {
        List<GetListSeatTypeDAL> GetListSeatType(int currentPage, int recordPerPage, out int totalRecord, out int response);
        void UpdateSeatTypeMultiplier(UpdateSeatTypeMultiplierDAL dataInput, out int response);
        void CreateSeatType(CreateSeatTypeDAL dataInput, out int response);
        void DeleteSeatType(Guid dataInput, out int response);
    }
}
