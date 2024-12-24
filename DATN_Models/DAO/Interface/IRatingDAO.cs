using DATN_Models.DAL.Rating;

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
