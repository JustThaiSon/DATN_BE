using DATN_Models.DAL.Cinemas;
using DATN_Models.DTOS.Cinemas.Res;
using DATN_Models.DTOS.Cinemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN_Models.DTOS.ShowTime.Req;
using DATN_Models.Models;
using DATN_Models.DAL.ShowTime;

namespace DATN_Models.DAO.Interface
{
    public interface IShowTimeDAO
    {
        void CreateShowTime(ShowTimeReq resquest, out int response);
        public void UpdateShowTime(Guid ShowTimeId, ShowTimeReq request, out int response);
        public void DeleteShowTime(Guid showTimeId, out int response);
        public List<ShowTimeDAL> GetListShowTime(Guid movieId, Guid roomId, int currentPage, int recordPerPage, out int totalRecord, out int response);


    }
}
