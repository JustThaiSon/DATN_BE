using DATN_Models.DAL.Movie;
using DATN_Models.DTOS.Movies.Req;

namespace DATN_Models.DAO.Interface
{
    public interface IMovieDAO
    {
        #region movie
        void CreateMovie(AddMovieDAL req, out int response, params Guid[] actorIds);
        void DeleteMovie(Guid Id, out int response);
        void UpdateMovie(Guid Id, out int response);
        List<MovieDAL> GetListMovie(int currentPage, int recordPerPage, out int totalRecord, out int response);
        MovieDAL GetMovieDetail(Guid Id, out int response);



        #endregion
    }
}
