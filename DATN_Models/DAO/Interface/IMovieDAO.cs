using DATN_Models.DAL.Movie;

namespace DATN_Models.DAO.Interface
{
    public interface IMovieDAO
    {
        #region movie
        void CreateMovie(AddMovieDAL req, out int response/*, params Guid[] actorIds*/);
        void UpdateMovie(UpdateMovieDAL req, out int response);
        void DeleteMovie(Guid Id, out int response);
        List<MovieDAL> GetListMovie(int currentPage, int recordPerPage, out int totalRecord, out int response);
        MovieDAL GetMovieDetail(Guid Id, out int response);



        #endregion
    }
}
