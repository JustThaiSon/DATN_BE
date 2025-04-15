using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Rating;
using DATN_Models.DAL.Service;
using DATN_Models.DAO.Interface;
using DATN_Models.Models;
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
            totalRecord = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_CurrentPage", currentPage);
                pars[1] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[2] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                // Sử dụng phương thức tùy chỉnh để lấy 3 kết quả từ stored procedure
                SqlConnection conn = null;
                List<MovieDAL> movies = new List<MovieDAL>();
                List<ActorDAL> actors = new List<ActorDAL>();
                Dictionary<Guid, List<MovieFormatInfoDAL>> movieFormats = new Dictionary<Guid, List<MovieFormatInfoDAL>>();

                try
                {
                    conn = db.OpenConnection();

                    using (var command = new SqlCommand("SP_Movie_GetList", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(pars);

                        using (var reader = command.ExecuteReader())
                        {
                            // Đọc danh sách phim
                            while (reader.Read())
                            {
                                var movie = new MovieDAL();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var columnName = reader.GetName(i);
                                    var property = typeof(MovieDAL).GetProperty(columnName);
                                    if (property != null && !reader.IsDBNull(i))
                                    {
                                        property.SetValue(movie, reader.GetValue(i));
                                    }
                                }
                                movies.Add(movie);
                            }

                            // Đọc danh sách diễn viên
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var actor = new ActorDAL();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        var columnName = reader.GetName(i);
                                        var property = typeof(ActorDAL).GetProperty(columnName);
                                        if (property != null && !reader.IsDBNull(i))
                                        {
                                            property.SetValue(actor, reader.GetValue(i));
                                        }
                                    }
                                    actors.Add(actor);
                                }
                            }

                            // Đọc danh sách định dạng phim
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var format = new MovieFormatInfoDAL();
                                    Guid movieId = Guid.Empty;

                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        var columnName = reader.GetName(i);
                                        if (columnName == "MovieId" && !reader.IsDBNull(i))
                                        {
                                            movieId = reader.GetGuid(i);
                                            continue;
                                        }

                                        var property = typeof(MovieFormatInfoDAL).GetProperty(columnName);
                                        if (property != null && !reader.IsDBNull(i))
                                        {
                                            property.SetValue(format, reader.GetValue(i));
                                        }
                                    }

                                    if (movieId != Guid.Empty)
                                    {
                                        if (!movieFormats.ContainsKey(movieId))
                                        {
                                            movieFormats[movieId] = new List<MovieFormatInfoDAL>();
                                        }
                                        movieFormats[movieId].Add(format);
                                    }
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }

                // Process each movie to add actors, genres and formats
                foreach (var item in movies)
                {
                    // Add actors to the movie
                    item.listdienvien = actors.Where(x => x.MovieId == item.Id).ToList();

                    // Add genres to the movie
                    item.genres = GetMovieGenres(item.Id, out int genreResponse);

                    // Add formats to the movie
                    if (movieFormats.ContainsKey(item.Id))
                    {
                        item.Formats = movieFormats[item.Id];
                    }
                    else
                    {
                        item.Formats = new List<MovieFormatInfoDAL>();
                    }
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
                // Create DataTable for actor IDs
                DataTable actorTable = new DataTable();
                actorTable.Columns.Add("Id", typeof(Guid));
                if (req.ListActorID != null && req.ListActorID.Any())
                {
                    foreach (var actorId in req.ListActorID)
                    {
                        actorTable.Rows.Add(actorId);
                    }
                }

                // Create DataTable for genre IDs
                DataTable genreTable = new DataTable();
                genreTable.Columns.Add("Id", typeof(Guid));
                if (req.ListGenreID != null && req.ListGenreID.Any())
                {
                    foreach (var genreId in req.ListGenreID)
                    {
                        genreTable.Rows.Add(genreId);
                    }
                }

                var pars = new SqlParameter[11]; // Updated parameter count

                pars[0] = new SqlParameter("@_MovieName", req.MovieName);
                pars[1] = new SqlParameter("@_Description", req.Description);
                pars[2] = new SqlParameter("@_Thumbnail", req.ThumbnailURL);
                pars[3] = new SqlParameter("@_Banner", req.BannerURL);
                pars[4] = new SqlParameter("@_Trailer", req.TrailerURL);
                pars[5] = new SqlParameter("@_Duration", req.Duration);
                pars[6] = new SqlParameter("@_ReleaseDate", req.ReleaseDate);
                pars[7] = new SqlParameter("@_Status", req.Status);
                pars[8] = new SqlParameter("@_ActorIDs", SqlDbType.Structured) { TypeName = "GuidList", Value = actorTable };
                pars[9] = new SqlParameter("@_GenreIDs", SqlDbType.Structured) { TypeName = "GuidList", Value = genreTable };
                pars[10] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Movie_Create", pars);

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

                // Sử dụng phương thức tùy chỉnh để lấy 3 kết quả từ stored procedure
                SqlConnection conn = null;
                MovieDAL movies = null;
                List<ActorDAL> actors = new List<ActorDAL>();
                List<MovieFormatInfoDAL> formats = new List<MovieFormatInfoDAL>();

                try
                {
                    conn = db.OpenConnection();

                    using (var command = new SqlCommand("SP_Movie_MovieDetail", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(pars);

                        using (var reader = command.ExecuteReader())
                        {
                            // Đọc thông tin phim
                            var moviesList = new List<MovieDAL>();
                            while (reader.Read())
                            {
                                var movie = new MovieDAL();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var columnName = reader.GetName(i);
                                    var property = typeof(MovieDAL).GetProperty(columnName);
                                    if (property != null && !reader.IsDBNull(i))
                                    {
                                        property.SetValue(movie, reader.GetValue(i));
                                    }
                                }
                                moviesList.Add(movie);
                            }
                            movies = moviesList.FirstOrDefault();

                            // Đọc thông tin diễn viên
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var actor = new ActorDAL();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        var columnName = reader.GetName(i);
                                        var property = typeof(ActorDAL).GetProperty(columnName);
                                        if (property != null && !reader.IsDBNull(i))
                                        {
                                            property.SetValue(actor, reader.GetValue(i));
                                        }
                                    }
                                    actors.Add(actor);
                                }
                            }

                            // Đọc thông tin định dạng phim
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var format = new MovieFormatInfoDAL();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        var columnName = reader.GetName(i);
                                        var property = typeof(MovieFormatInfoDAL).GetProperty(columnName);
                                        if (property != null && !reader.IsDBNull(i))
                                        {
                                            property.SetValue(format, reader.GetValue(i));
                                        }
                                    }
                                    formats.Add(format);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }

                if (movies != null)
                {
                    // Add actors to the movie
                    movies.listdienvien = actors.Where(x => x.MovieId == movies.Id).ToList();

                    // Add genres to the movie
                    movies.genres = GetMovieGenres(Id, out int genreResponse);

                    // Add formats to the movie
                    movies.Formats = formats;

                    // Ensure average rating is set
                    if (movies.AverageRating == 0)
                    {
                        movies.AverageRating = GetMovieRating(Id);
                    }
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
                // Create DataTable for actor IDs
                DataTable actorTable = new DataTable();
                actorTable.Columns.Add("Id", typeof(Guid));
                if (req.ListActorID != null && req.ListActorID.Any())
                {
                    foreach (var actorId in req.ListActorID)
                    {
                        actorTable.Rows.Add(actorId);
                    }
                }

                // Create DataTable for genre IDs
                DataTable genreTable = new DataTable();
                genreTable.Columns.Add("Id", typeof(Guid));
                if (req.ListGenreID != null && req.ListGenreID.Any())
                {
                    foreach (var genreId in req.ListGenreID)
                    {
                        genreTable.Rows.Add(genreId);
                    }
                }

                var pars = new SqlParameter[12]; // Updated parameter count

                pars[0] = new SqlParameter("@_MovieID", req.MovieID);
                pars[1] = new SqlParameter("@_MovieName", req.MovieName);
                pars[2] = new SqlParameter("@_Description", req.Description);
                pars[3] = new SqlParameter("@_Thumbnail", req.ThumbnailURL);
                pars[4] = new SqlParameter("@_Banner", req.BannerURL);
                pars[5] = new SqlParameter("@_Trailer", req.TrailerURL);
                pars[6] = new SqlParameter("@_Duration", req.Duration);
                pars[7] = new SqlParameter("@_ReleaseDate", req.ReleaseDate);
                pars[8] = new SqlParameter("@_Status", req.Status);
                pars[9] = new SqlParameter("@_ActorIDs", SqlDbType.Structured) { TypeName = "GuidList", Value = actorTable };
                pars[10] = new SqlParameter("@_GenreIDs", SqlDbType.Structured) { TypeName = "GuidList", Value = genreTable };
                pars[11] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Movie_Update", pars);

                response = ConvertUtil.ToInt(pars[11].Value);
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

        public MovieDAL GetDetailMovieLangding(Guid movieId, out int response)
        {
            //response = 0;
            //DBHelper db = null;
            //try
            //{
            //    var pars = new SqlParameter[2];
            //    pars[0] = new SqlParameter("@_MovieID", movieId);
            //    pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
            //    db = new DBHelper(connectionString);
            //    var result = db.GetInstanceSP<GetDetailMovieLangdingDAL>("SP_Langding_GetDetailMovie", pars);
            //    if (result != null)
            //    {
            //        result.Genres = result.GenreList.Split(',')
            //      .Select(x => x.Split(':'))
            //      .Where(parts => Guid.TryParse(parts[0].Trim(), out _))
            //      .Select(parts => new ListGenreLangdingDAL
            //      {
            //          Id = Guid.Parse(parts[0].Trim()),
            //          GenreName = parts[1].Trim()
            //      })
            //      .ToList();

            //        result.Actors = result.ActorList.Split(',')
            //            .Select(x => x.Split(':'))
            //            .Where(parts => Guid.TryParse(parts[0].Trim(), out _))
            //            .Select(parts => new ListActorLangdingDAL
            //            {
            //                Id = Guid.Parse(parts[0].Trim()),
            //                ActorName = parts[1].Trim()
            //            })
            //            .ToList();
            //    }
            //    response = ConvertUtil.ToInt(pars[1].Value);
            //    return result;
            //}

            //catch (Exception ex)
            //{
            //    response = -99;
            //    throw;
            //}
            //finally
            //{
            //    if (db != null)
            //        db.Close();
            //}



            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_MovieID", movieId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var (movies, actors) = db.GetSingleSP<MovieDAL, ActorDAL>("SP_Movie_MovieDetail", pars);

                if (movies != null)
                {
                    // Add actors to the movie
                    movies.listdienvien = actors.Where(x => x.MovieId == movies.Id).ToList();

                    // Add genres to the movie
                    movies.genres = GetMovieGenres(movieId, out int genreResponse);


                    movies.AverageRating = GetMovieRating(movieId);
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
        public List<GetShowTimeLandingDAL> GetShowTimeLanding(Guid? movieId, string? location, DateTime? date, int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            totalRecord = 0;
            DBHelper? db = null;
            try
            {
                var pars = new SqlParameter[7];
                pars[0] = new SqlParameter("@_MovieID", movieId == Guid.Empty ? (object)DBNull.Value : movieId);
                pars[1] = new SqlParameter("@_Location", location);
                pars[2] = new SqlParameter("@_Date", date);
                pars[3] = new SqlParameter("@_CurrentPage", currentPage);
                pars[4] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[5] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[6] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

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
                response = ConvertUtil.ToInt(pars[6].Value);
                totalRecord = ConvertUtil.ToInt(pars[5].Value);
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

        public List<GetAllNameMovieDAL> GetAllNameMovie(out int response)
        {
            response = 0;
            DBHelper? db = null;
            try
            {
                var pars = new SqlParameter[1];
                pars[0] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<GetAllNameMovieDAL>("SP_Movie_GetNameMovie", pars);

                response = ConvertUtil.ToInt(pars[0].Value);
                return result ?? new List<GetAllNameMovieDAL>();
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





        public List<MovieGenreDAL> GetMovieGenres(Guid movieId, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_MovieID", movieId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<MovieGenreDAL>("SP_Movie_GetGenres", pars);

                response = ConvertUtil.ToInt(pars[1].Value);
                return result ?? new List<MovieGenreDAL>();
            }
            catch (Exception)
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



        public double GetMovieRating(Guid movieId)
        {
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2]; // Corrected parameter count to 3
                pars[0] = new SqlParameter("@_MovieID", movieId);
                pars[1] = new SqlParameter("@_RatingAverage", SqlDbType.Decimal) { Direction = ParameterDirection.Output, Precision = 5, Scale = 1 }; // Changed to Decimal to match SQL parameter

                db = new DBHelper(connectionString);
                var result = db.GetListSP<RatingDAL>("SP_Rating_MovieDetail", pars);

                // Check if pars[1].Value is null or DBNull
                if (pars[1].Value == null || pars[1].Value == DBNull.Value)
                {
                    return 0;
                }

                // Convert to double and check if it's 0
                double ratingAverage = Convert.ToDouble(pars[1].Value);
                return ratingAverage;
            }
            catch (Exception)
            {
                return 0; // Return 0 in case of exception
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public GetMovieByShowTimeDAL GetMovieByShowTime(Guid showtime, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_ShowTimeId", showtime);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<GetMovieByShowTimeDAL>("SP_ShowTime_GetMovieByShowtime", pars);
                response = ConvertUtil.ToInt(pars[1].Value);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }






        #endregion
    }
}
