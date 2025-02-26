using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Movie;
using DATN_Models.DAO.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO
{
    public class MovieDAO : IMovieDAO
    {
        private static string connectionString = string.Empty;

        public MovieDAO()
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        #region movie_nghia

        /// <summary>
        /// Hàm thực hiện lấy danh sách movie
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="recordPerPage"></param>
        /// <param name="totalRecord"></param>
        /// <param name="response"></param>
        /// <returns>Trả về danh sách movie theo yêu cầu (trang hiện tại, bản ghi tói đa 1 trang)</returns>
        public List<MovieDAL> GetListMovie(int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_CurrentPage", currentPage);
                pars[1] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[2] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var (movies, actors) = db.GetMultipleSP<MovieDAL, ActorDAL>("SP_Movie_GetList", pars);


                foreach (var item in movies)
                {
                    item.listdienvien = actors.Where(x => x.MovieId == item.Id).ToList();
                }


                response = ConvertUtil.ToInt(pars[3].Value);
                totalRecord = ConvertUtil.ToInt(pars[2].Value);

                return movies;
            }
            catch (Exception ex)
            {
                response = -99;
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }


        /// <summary>
        /// Thêm Movie mới
        /// </summary>
        /// <param name="req">Thông tin phim</param>
        /// <param name="response">Trả về mã response sau khi thực hiện thủ tục</param>
        /// <param name="actorids">Để nhận vào nhiều actor 1 lúc (1 movie có thể có nhiều actor)</param>
        public void CreateMovie(AddMovieDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                // Tạo DataTable để chứa danh sách actorId
                // GuidList
                DataTable actorTable = new DataTable();

                actorTable.Columns.Add("Id", typeof(Guid));

                foreach (var actorId in req.ListActorID)
                {
                    actorTable.Rows.Add(actorId);
                }

                var pars = new SqlParameter[10];

                pars[0] = new SqlParameter("@_MovieName", req.MovieName);
                pars[1] = new SqlParameter("@_Description", req.Description);
                pars[2] = new SqlParameter("@_Thumbnail", req.ThumbnailURL);
                pars[3] = new SqlParameter("@_Banner", req.BannerURL);
                pars[4] = new SqlParameter("@_Trailer", req.TrailerURL);
                pars[5] = new SqlParameter("@_Duration", req.Duration);
                pars[6] = new SqlParameter("@_ReleaseDate", req.ReleaseDate);
                pars[7] = new SqlParameter("@_Status", req.Status);

                // Thêm id actor vào trong bảng MovieActor
                pars[8] = new SqlParameter("@_ActorIDs", SqlDbType.Structured) { TypeName = "GuidList", Value = actorTable };
                pars[9] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Movie_Create", pars);

                //var result = db.GetListSP<ListActorDAL>("SP_Actor_GetListActor", pars);
                response = ConvertUtil.ToInt(pars[9].Value);
            }
            catch (Exception ex)
            {
                response = -99;
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }


        /// <summary>
        /// Lấy thông tin chi tiết của phim (hiện tại chưa lấy được thông tin các diễn viên trong pgim)
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public MovieDAL GetMovieDetail(Guid Id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_MovieID", Id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                //var result = db.GetInstanceSP<MovieDAL>("SP_Movie_MovieDetail", pars);

                var (movies, actors) = db.GetSingleSP<MovieDAL, ActorDAL>("SP_Movie_MovieDetail", pars);

                if (movies != null)
                {
                    movies.listdienvien = actors.Where(x => x.MovieId == movies.Id).ToList();
                }

                response = ConvertUtil.ToInt(pars[1].Value);
                return movies;
            }
            catch (Exception ex)
            {
                response = -99;
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }


        public void DeleteMovie(Guid Id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];

                pars[0] = new SqlParameter("@_MovieID", Id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Movie_Delete", pars);

                //var result = db.GetListSP<ListActorDAL>("SP_Actor_GetListActor", pars);
                response = ConvertUtil.ToInt(pars[1].Value);
            }
            catch (Exception ex)
            {
                response = -99;
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }


        public void UpdateMovie(UpdateMovieDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                // Tạo DataTable để chứa danh sách actorId
                // GuidList
                DataTable actorTable = new DataTable();

                actorTable.Columns.Add("Id", typeof(Guid));

                foreach (var actorId in req.ListActorID)
                {
                    actorTable.Rows.Add(actorId);
                }

                var pars = new SqlParameter[10];

                pars[0] = new SqlParameter("@_MovieID", req.MovieName);
                pars[1] = new SqlParameter("@_MovieName", req.MovieName);
                pars[2] = new SqlParameter("@_Description", req.Description);
                pars[3] = new SqlParameter("@_Thumbnail", req.ThumbnailURL);
                pars[4] = new SqlParameter("@_Banner", req.BannerURL);
                pars[5] = new SqlParameter("@_Trailer", req.TrailerURL);
                pars[6] = new SqlParameter("@_Duration", req.Duration);
                pars[7] = new SqlParameter("@_ReleaseDate", req.ReleaseDate);
                pars[8] = new SqlParameter("@_Status", req.Status);

                // Thêm id actor vào trong bảng MovieActor
                pars[9] = new SqlParameter("@_ActorIDs", SqlDbType.Structured) { TypeName = "GuidList", Value = actorTable };
                pars[10] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Movie_Update", pars);

                response = ConvertUtil.ToInt(pars[10].Value);
            }
            catch (Exception ex)
            {
                response = -99;
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<GetMovieLandingDAL> GetMovieLanding(int type, int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_Type", type);
                pars[1] = new SqlParameter("@_CurrentPage", currentPage);
                pars[2] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[3] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetListSP<GetMovieLandingDAL>("SP_Langding_GetMovie", pars);
                response = ConvertUtil.ToInt(pars[4].Value);
                totalRecord = ConvertUtil.ToInt(pars[3].Value);
                return result;
            }
            catch (Exception ex)
            {
                response = -99;
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public GetDetailMovieLangdingDAL GetDetailMovieLangding(Guid movieId, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_MovieID", movieId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<GetDetailMovieLangdingDAL>("SP_Langding_GetDetailMovie", pars);
                if (result != null)
                {
                    result.Genres = result.GenreList.Split(',')
                  .Select(x => x.Split(':'))
                  .Where(parts => Guid.TryParse(parts[0].Trim(), out _))
                  .Select(parts => new ListGenreLangdingDAL
                  {
                      Id = Guid.Parse(parts[0].Trim()),
                      GenreName = parts[1].Trim()
                  })
                  .ToList();

                    result.Actors = result.ActorList.Split(',')
                        .Select(x => x.Split(':'))
                        .Where(parts => Guid.TryParse(parts[0].Trim(), out _))
                        .Select(parts => new ListActorLangdingDAL
                        {
                            Id = Guid.Parse(parts[0].Trim()),
                            ActorName = parts[1].Trim()
                        })
                        .ToList();
                }
                response = ConvertUtil.ToInt(pars[1].Value);
                return result;
            }

            catch (Exception ex)
            {
                response = -99;
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<GetShowTimeLandingDAL> GetShowTimeLanding(string location, DateTime date, int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            totalRecord = 0;
            DBHelper? db = null;
            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@_Location", location);
                pars[1] = new SqlParameter("@_Date", date);
                pars[2] = new SqlParameter("@_CurrentPage", currentPage);
                pars[3] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[4] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[5] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetListSP<GetShowTimeLandingDAL>("SP_Langding_GetShowTime", pars);
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        item.Showtimes = item.ListShowTime.Split(',')
                            .Select(x => x.Split('|'))
                            .Where(parts => Guid.TryParse(parts[0].Trim(), out _))
                            .Select(parts => new ShowtimesLangdingDAL
                            {
                                Id = Guid.Parse(parts[0].Trim()),
                                StartTime = TimeSpan.Parse(parts[1].Trim())
                            })
                            .ToList();
                    }
                }
                response = ConvertUtil.ToInt(pars[5].Value);
                totalRecord = ConvertUtil.ToInt(pars[4].Value);
                return result ?? new List<GetShowTimeLandingDAL>();
            }
            catch (Exception)
            {
                response = -99;
                throw;
            }
            finally
            {
                db?.Close();
            }
        }

        #endregion
    }
}
