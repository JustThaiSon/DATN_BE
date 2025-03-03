using DATN_Models.DAL.ShowTime;
using DATN_Models.DTOS.ShowTime.Req;

namespace DATN_Models.DAO.Interface
{
    public interface IShowTimeDAO
    {
        void CreateShowTime(ShowTimeReq resquest, out int response);
        public void UpdateShowTime(Guid ShowTimeId, ShowTimeReq request, out int response);
        public void DeleteShowTime(Guid showTimeId, out int response);
        public List<ShowTimeDAL> GetListShowTime(int currentPage, int recordPerPage, out int totalRecord, out int response);


    }
}
