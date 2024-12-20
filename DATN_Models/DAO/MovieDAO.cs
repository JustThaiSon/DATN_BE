using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Movie;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Movies.Req;
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
        public void CreateActor(ActorReq resquest, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@_Name", resquest.Name);
                pars[1] = new SqlParameter("@_DateOfBirth", resquest.DateOfBirth);
                pars[2] = new SqlParameter("@_Biography", resquest.Biography);
                pars[3] = new SqlParameter("@_Photo", resquest.Photo);
                pars[4] = new SqlParameter("@_Status", resquest.Status);
                pars[5] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Actor_Create", pars);
                //GetDetail GetInstanceSP
                //GetList GetListSP
                response = ConvertUtil.ToInt(pars[5].Value);
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

        public ListActorDAL GetDetailActor(Guid Id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_ActorID", Id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<ListActorDAL>("SP_Actor_DetailActor", pars);
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

        public List<ListActorDAL> GetListActor(int currentPage, int recordPerPage, out int totalRecord, out int response)
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
                var result = db.GetListSP<ListActorDAL>("SP_Actor_GetListActor", pars);
                response = ConvertUtil.ToInt(pars[3].Value);
                totalRecord = ConvertUtil.ToInt(pars[2].Value);
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


        #region movie_nghia

        // Thêm phim
        /// <summary>
        /// Thêm Movie mới
        /// </summary>
        /// <param name="req">Thông tin phim</param>
        /// <param name="response">Trả về mã response sau khi thực hiện thủ tục</param>
        /// <param name="actorids">Để nhận vào nhiều actor 1 lúc (1 movie có thể có nhiều actor)</param>
        public void CreateMovie(MovieReq req, out int response, params Guid[] actorIds)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                // Tạo DataTable để chứa danh sách actorId
                // GuidList
                DataTable actorTable = new DataTable();
                actorTable.Columns.Add("Id", typeof(Guid));
                foreach (var actorId in actorIds)
                {
                    actorTable.Rows.Add(actorId);
                }

                var pars = new SqlParameter[10];

                pars[0] = new SqlParameter("@_MovieName", req.MovieName);
                pars[1] = new SqlParameter("@_Description", req.Description);
                pars[2] = new SqlParameter("@_Thumbnail", req.Thumbnail);
                pars[3] = new SqlParameter("@_Trailer", req.Trailer);
                pars[4] = new SqlParameter("@_Duration", req.Duration);
                pars[5] = new SqlParameter("@_ReleaseDate", req.ReleaseDate);
                pars[6] = new SqlParameter("@_Status", req.Status);
                pars[7] = new SqlParameter("@_BasePriceMultiplier", req.BasePriceMultiplier);
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
        /// Hàm thực hiện lấy danh sách movie
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="recordPerPage"></param>
        /// <param name="totalRecord"></param>
        /// <param name="response"></param>
        /// <returns>Trả về danh sách movie theo yêu cầu (trang hiện tại, bản ghi tói đa 1 trang)</returns>
        public List<ListMovieDAL> GetListMovie(int currentPage, int recordPerPage, out int totalRecord, out int response)
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


                var result = db.GetListSP<ListMovieDAL>("SP_Movie_GetList", pars);


                response = ConvertUtil.ToInt(pars[3].Value);
                totalRecord = ConvertUtil.ToInt(pars[2].Value);
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

        #endregion
    }
}
