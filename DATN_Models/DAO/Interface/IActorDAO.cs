using DATN_Models.DAL.Movie.Actor;

namespace DATN_Models.DAO.Interface
{
    public interface IActorDAO
    {
        #region actor
        void CreateActor(AddActorDAL resquest, out int response);
        void UpdateActor(Guid id, UpdateActorDAL resquest, out int response);
        void DeleteActor(Guid id, out int response);
        List<ListActorDAL> GetListActor(int currentPage, int recordPerPage, out int totalRecord, out int response);
        ListActorDAL GetDetailActor(Guid Id, out int response);

        #endregion
    }
}
