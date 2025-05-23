﻿using DATN_Models.DAL.Movie;
using DATN_Models.DTOS.Movies.Res;

namespace DATN_Models.DAO.Interface
{
    public interface IMovieDAO
    {
        #region movie
        void CreateMovie(AddMovieDAL req, out int response);
        void UpdateMovie(UpdateMovieDAL req, out int response);
        void DeleteMovie(Guid Id, out int response);
        List<MovieDAL> GetListMovie(int currentPage, int recordPerPage, out int totalRecord, out int response);
        MovieDAL GetMovieDetail(Guid Id, out int response);

        List<MovieGenreDAL> GetMovieGenres(Guid movieId, out int response);

        List<GetMovieLandingDAL> GetMovieLanding(Guid? movieType, int type, int currentPage, int recordPerPage, out int totalRecord, out int response);
        MovieDAL GetDetailMovieLangding(Guid movieId, out int response);
        List<GetAllNameMovieDAL> GetAllNameMovie(out int response);
        List<GetShowTimeLandingDAL> GetShowTimeLanding(Guid? cinemaId,Guid? movieId, string? location, DateTime? date, int currentPage, int recordPerPage, out int totalRecord, out int response);

        GetMovieByShowTimeDAL GetMovieByShowTime(Guid showtime,out int response);
        #endregion

        List<GetCinemaByLocationRes> GetCinemaByLocation(string location, out int response);
        List<GetCinemaByLocationRes> GetCinemaAll( out int response);

        List<GetMovieTypeRes> GetMovieType(out int response);
    }
}
