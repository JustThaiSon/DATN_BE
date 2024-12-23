using DATN_Models.DAL.Rating;
using DATN_Models.DTOS.Movies.Req;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.Rating.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAO.Interface
{
    public interface IRatingDAO
    {
        #region rating
        void CreateRating(AddRatingDAL req, out int response);
        void UpdateRating(UpdateRatingDAL req, out int response);
        void DeleteRating(Guid Id, out int response);

        // Phục vụ việc quản lý


        // Quản lý ẩn rating của khách hàng được (trong tài liệu hà ghi v)
        void HideRating(Guid Id, out int response);
        List<GetListRatingDAL> GetListRating(int currentPage, int recordPerPage, out int totalRecord, out int response);

        //List<GetListRatingDAL> SearchRating(dynamic data, int currentPage, int recordPerPage, out int totalRecord, out int response);


        #endregion
    }
}
