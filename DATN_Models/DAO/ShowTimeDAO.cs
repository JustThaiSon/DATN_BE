using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.ShowTime;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.ShowTime.Req;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO
{
    public class ShowTimeDAO : IShowTimeDAO
    {
        private static string connectionString = string.Empty;

        public ShowTimeDAO()
        {
            var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }
        public void CreateShowTime(ShowTimeReq request, out int response)
        {
            response = 0; // Khởi tạo mã phản hồi
            DBHelper db = null;

            try
            {
                // Tạo danh sách tham số cho Stored Procedure
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@_MovieId", request.MovieId);
                pars[1] = new SqlParameter("@_RoomId", request.RoomId);
                pars[2] = new SqlParameter("@_StartTime", request.StartTime);
                pars[3] = new SqlParameter("@_EndTime", request.EndTime);
                pars[4] = new SqlParameter("@_Status", request.Status);
                pars[5] = new SqlParameter("@_Response", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                // Gọi Stored Procedure
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_ShowTime_Create", pars);

                // Lấy mã phản hồi từ tham số OUTPUT
                response = Convert.ToInt32(pars[5].Value);
            }
            catch (Exception ex)
            {
                response = -99; // Lỗi hệ thống
                throw new Exception("Error creating show time", ex);
            }
            finally
            {
                if (db != null)
                    db.Close(); // Đóng kết nối
            }
        }

        public void UpdateShowTime(Guid ShowTimeId, ShowTimeReq request, out int response)
        {
            response = 0; // Khởi tạo mã phản hồi
            DBHelper db = null;

            try
            {
                // Tạo danh sách tham số cho Stored Procedure
                var pars = new SqlParameter[7];
                pars[0] = new SqlParameter("@_ShowTimeId", ShowTimeId);
                pars[1] = new SqlParameter("@_MovieId", request.MovieId);
                pars[2] = new SqlParameter("@_RoomId", request.RoomId);
                pars[3] = new SqlParameter("@_StartTime", request.StartTime);
                pars[4] = new SqlParameter("@_EndTime", request.EndTime);
                pars[5] = new SqlParameter("@_Status", request.Status);
                pars[6] = new SqlParameter("@_Response", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                // Gọi Stored Procedure
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_ShowTime_Update", pars);

                // Lấy mã phản hồi từ tham số OUTPUT
                response = ConvertUtil.ToInt(pars[6].Value);
            }
            catch (Exception ex)
            {
                response = -99; // Lỗi hệ thống
                throw new Exception("Error updating show time", ex);
            }
            finally
            {
                if (db != null)
                    db.Close(); // Đóng kết nối
            }
        }

        public void DeleteShowTime(Guid showTimeId, out int response)
        {
            response = 0; // Khởi tạo mã phản hồi
            DBHelper db = null;

            try
            {
                // Tạo danh sách tham số cho Stored Procedure
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_ShowTimeId", showTimeId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                // Gọi Stored Procedure
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_ShowTime_Delete", pars);

                // Lấy mã phản hồi từ tham số OUTPUT
                response = ConvertUtil.ToInt(pars[1].Value);
            }
            catch (Exception ex)
            {
                response = -99; // Lỗi hệ thống
                throw new Exception("Error deleting show time", ex);
            }
            finally
            {
                if (db != null)
                    db.Close(); // Đóng kết nối
            }
        }

        public List<ShowTimeDAL> GetListShowTime(int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            DBHelper db = null;
            totalRecord = 0;

            try
            {
                // Tạo tham số cho Stored Procedure
                var pars = new SqlParameter[4];

                pars[0] = new SqlParameter("@_CurrentPage", currentPage);
                pars[1] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[2] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                // Kết nối đến cơ sở dữ liệu và gọi Stored Procedure
                db = new DBHelper(connectionString);
                var result = db.GetListSP<ShowTimeDAL>("SP_ShowTime_GetList", pars);

                // Lấy kết quả từ các tham số OUTPUT
                response = ConvertUtil.ToInt(pars[3].Value);
                totalRecord = ConvertUtil.ToInt(pars[2].Value);

                // Trả về danh sách lịch chiếu
                return result;
            }
            catch (Exception ex)
            {
                response = -99; // Mã lỗi hệ thống
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close(); // Đảm bảo đóng kết nối
            }
        }

    }
}
