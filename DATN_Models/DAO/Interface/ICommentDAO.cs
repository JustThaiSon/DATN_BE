using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Movie.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAO.Interface
{
    public interface ICommentDAO
    {
        #region Comment_nghia
        void CreateComment(Guid userID, CreateCommentDAL req, out int response);
        void UpdateComment(Guid Id, UpdateCommentDAL req, out int response);
        void DeleteComment(Guid Id, out int response);

        List<ListCommentDAL> GetListComment(Guid Id, int currentPage, int recordPerPage, out int totalRecord, out int response);
        //MovieDAL GetComment(Guid Id, out int response);



        #endregion
    }
}
