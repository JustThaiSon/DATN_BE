using DATN_Models.DAL.Movie;
using DATN_Models.DTOS.Movies.Req;

namespace DATN_Models.DAO.Interface
{
    public interface IMovieDAO
    {
        void CreateActor(ActorReq resquest, out int response);
        List<ListActorDAL> GetListActor(int currentPage, int recordPerPage, out int totalRecord, out int response);
        ListActorDAL GetDetailActor(Guid Id, out int response);
    }
}
