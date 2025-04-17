﻿using DATN_Models.DAL.MovieFormat;

namespace DATN_Models.DAO.Interface
{
    public interface IMovieFormatDAO
    {
        List<MovieFormatDAL> GetMovieFormats(int currentPage, int recordPerPage, out int totalRecord, out int response);
        MovieFormatDAL GetMovieFormatById(Guid id, out int response);
        void CreateMovieFormat(MovieFormatDAL movieFormat, out int response);
        void UpdateMovieFormat(MovieFormatDAL movieFormat, out int response);
        void DeleteMovieFormat(Guid id, out int response);
        
        // Phương thức để gán định dạng phim cho một phim cụ thể
        void AssignFormatToMovie(MovieFormatMovieDAL formatMovie, out int response);
        
        // Phương thức để xóa định dạng phim khỏi một phim cụ thể
        void RemoveFormatFromMovie(Guid movieId, Guid formatId, out int response);
        
        // Phương thức để lấy tất cả định dạng phim của một phim cụ thể
        List<MovieFormatMovieDAL> GetMovieFormatsByMovieId(Guid movieId, out int response);
        
        // Phương thức để lấy tất cả phim có định dạng cụ thể
        List<MovieFormatMovieDAL> GetMoviesByFormatId(Guid formatId, int currentPage, int recordPerPage, out int totalRecord, out int response);
    }
}
