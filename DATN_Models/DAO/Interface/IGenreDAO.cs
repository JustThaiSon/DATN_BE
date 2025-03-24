using DATN_Models.DAL.Genre;
using System;
using System.Collections.Generic;

namespace DATN_Models.DAO.Interface
{
    public interface IGenreDAO
    {
        void CreateGenre(AddGenreDAL request, out int response);
        void UpdateGenre(Guid id, UpdateGenreDAL request, out int response);
        void DeleteGenre(Guid id, out int response);
        List<GenreDAL> GetListGenre(int currentPage, int recordPerPage, out int totalRecord, out int response);
        GenreDAL GetGenreById(Guid id, out int response);
    }
}