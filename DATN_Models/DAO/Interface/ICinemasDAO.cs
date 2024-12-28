using DATN_Models.DAL.Cinemas;
using DATN_Models.DTOS.Cinemas.Req;
using DATN_Models.DTOS.Cinemas.Res;

namespace DATN_Models.DAO.Interface
{
    public interface ICinemasDAO
    {
        void CreateCinemas(CinemasReq resquest, out int response);
        public void UpdateCinemas(Guid CinemasId, CinemasReq request, out int response);
        List<CinemasDAL> GetListCinemas(int currentPage, int recordPerPage, out int totalRecord, out int response);
        public List<CinemasDAL> GetListCinemasByName(string nameCinemas, int currentPage, int recordPerPage, out int totalRecord, out int response);
        public void UpdateCinemasAdress(Guid CinemasId, string newAdress, out int response);
        public CinemasRes GetCinemaById(Guid cinemasId, out int response);


    }
}
