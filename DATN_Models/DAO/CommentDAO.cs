using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Comment;
using DATN_Models.DAL.Movie.Actor;
using DATN_Models.DAO.Interface;
using DATN_Models.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;

namespace DATN_Models.DAO
{
    public class CommentDAO : ICommentDAO
    {
        private static string connectionString = string.Empty;

        public CommentDAO()
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        public void CreateComment(Guid userID, CreateCommentDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[4];

                pars[0] = new SqlParameter("@_UserID", userID);
                pars[1] = new SqlParameter("@_Content", req.Content);
                pars[2] = new SqlParameter("@_MovieID", req.MovieID);
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Comment_Create", pars);

                //var result = db.GetListSP<ListActorDAL>("SP_Actor_GetListActor", pars);
                response = ConvertUtil.ToInt(pars[3].Value);
            }
            catch (Exception ex)
            {
                response = -99;
                Console.WriteLine($"Error in CreateComment: {ex.Message}");
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public void UpdateComment(Guid Id, UpdateCommentDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[3];

                pars[0] = new SqlParameter("@_CommentID", Id);
                pars[1] = new SqlParameter("@_Content", req.Content);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Comment_Update", pars);

                //var result = db.GetListSP<ListActorDAL>("SP_Actor_GetListActor", pars);
                response = ConvertUtil.ToInt(pars[2].Value);
            }
            catch (Exception ex)
            {
                response = -99;
                Console.WriteLine($"Error in UpdateComment: {ex.Message}");
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public void DeleteComment(Guid Id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];

                pars[0] = new SqlParameter("@_CommentID", Id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Comment_Delete", pars);

                //var result = db.GetListSP<ListActorDAL>("SP_Actor_GetListActor", pars);
                response = ConvertUtil.ToInt(pars[1].Value);
            }
            catch (Exception ex)
            {
                response = -99;
                Console.WriteLine($"Error in DeleteComment: {ex.Message}");
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<ListCommentDAL> GetListComment(Guid MovieId, int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            totalRecord = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_MovieID", MovieId);
                pars[1] = new SqlParameter("@_CurrentPage", currentPage);
                pars[2] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[3] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                // Sử dụng CommentRawDAL để lấy dữ liệu trực tiếp từ stored procedure
                Debug.WriteLine("Executing SP_Comment_GetList with parameters:");
                Debug.WriteLine($"@_MovieID: {MovieId}");
                Debug.WriteLine($"@_CurrentPage: {currentPage}");
                Debug.WriteLine($"@_RecordPerPage: {recordPerPage}");

                var rawResult = db.GetListSP<CommentRawDAL>("SP_Comment_GetList", pars);

                response = ConvertUtil.ToInt(pars[4].Value);
                totalRecord = ConvertUtil.ToInt(pars[3].Value);

                // Kiểm tra response code
                if (response != 200)
                {
                    Console.WriteLine($"SP returned error code: {response}");
                }

                // Chuyển đổi từ CommentRawDAL sang ListCommentDAL
                var result = new List<ListCommentDAL>();
                foreach (var item in rawResult)
                {
                    result.Add(new ListCommentDAL
                    {
                        Id = item.Id,
                        UserID = item.UserID,
                        MovieID = item.MovieID,
                        Username = item.Username,
                        Content = item.Content,
                        ratingvalue = item.ratingvalue,
                        CreateDate = item.CreateDate,
                        Status = item.Status
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                response = -99;
                Debug.WriteLine($"Error in GetListComment: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Debug.WriteLine($"Inner stack trace: {ex.InnerException.StackTrace}");
                }

                return new List<ListCommentDAL>();
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }










    }



}
