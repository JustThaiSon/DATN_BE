using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Movie.Actor;
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


        public ShowtimeAutoDateDAL AutoDateNghia(ShowtimeAutoDateReq showtimereq, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_CinemaId", showtimereq.CinemasId);
                pars[1] = new SqlParameter("@_RoomId", showtimereq.RoomId);
                pars[2] = new SqlParameter("@_Date", showtimereq.Date);
                pars[3] = new SqlParameter("@_MovieID", showtimereq.MovieId);
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };


                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<ShowtimeAutoDateDAL>("SP_ShowTime_AutoDate_Nghia", pars);
                response = ConvertUtil.ToInt(pars[4].Value);
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

        public void UpdateShowTime(Guid ShowTimeId, UpdateShowTimeReq request, out int response)
        {
            response = 0; // Khởi tạo mã phản hồi
            DBHelper db = null;

            try
            {
                // Tạo danh sách tham số cho Stored Procedure
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_ShowTimeId", ShowTimeId);
                pars[1] = new SqlParameter("@_RoomId", request.RoomId);
                pars[2] = new SqlParameter("@_StartTime", request.StartTime);
                pars[3] = new SqlParameter("@_EndTime", request.EndTime);
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                // Gọi Stored Procedure
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_ShowTime_Update", pars);

                // Lấy mã phản hồi từ tham số OUTPUT
                response = Convert.ToInt32(pars[4].Value);

                // Log thông tin để debug
                if (response == -1) // Lỗi trùng lịch
                {
                    // Kiểm tra các lịch chiếu trong phòng
                    var checkPars = new SqlParameter[1];
                    checkPars[0] = new SqlParameter("@_RoomId", request.RoomId);
                    var existingShowtimes = db.GetListSP<ShowTimeDAL>("SP_ShowTime_GetListByRoom", checkPars);

                    if (existingShowtimes != null)
                    {
                        foreach (var showtime in existingShowtimes)
                        {
                            Console.WriteLine($"Existing showtime: ID={showtime.Id}, Start={showtime.StartTime}, End={showtime.EndTime}");
                        }
                    }
                }
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

        public List<ShowTimeDAL> GetListShowTimes(int currentPage, int recordPerPage, out int totalRecord, out int response)
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
                var result = db.GetListSP<ShowTimeDAL>("SP_ShowTime_GetList", pars);

                response = ConvertUtil.ToInt(pars[3].Value);
                totalRecord = ConvertUtil.ToInt(pars[2].Value);

                return result ?? new List<ShowTimeDAL>();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting show time list", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public ShowTimeDAL GetShowTimeById(Guid showTimeId, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_ShowTimeId", showTimeId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var results = db.GetListSP<ShowTimeDAL>("SP_ShowTime_GetById", pars);

                response = ConvertUtil.ToInt(pars[1].Value);
                return results?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting show time by id", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<AvailableRoomDAL> GetAvailableRooms(DateTime startTime, DateTime endTime, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_StartTime", startTime);
                pars[1] = new SqlParameter("@_EndTime", endTime);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<AvailableRoomDAL>("SP_ShowTime_GetAvailableRooms", pars);

                response = ConvertUtil.ToInt(pars[2].Value);
                return result ?? new List<AvailableRoomDAL>();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting available rooms", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<TimeSlotDAL> GetAvailableTimes(Guid roomId, DateTime date, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_RoomId", roomId);
                pars[1] = new SqlParameter("@_Date", date);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<TimeSlotDAL>("SP_ShowTime_GetAvailableTimes", pars);

                response = ConvertUtil.ToInt(pars[2].Value);
                return result ?? new List<TimeSlotDAL>();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting available time slots", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public void UpdateShowTimeStatus(Guid showTimeId, int status, out int response)
        {
            response = 0; // Khởi tạo mã phản hồi
            DBHelper db = null;

            try
            {
                // Tạo danh sách tham số cho Stored Procedure
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_ShowTimeId", showTimeId);
                pars[1] = new SqlParameter("@_Status", status);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                // Gọi Stored Procedure
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_ShowTime_UpdateStatus", pars);

                // Lấy mã phản hồi từ tham số OUTPUT
                response = ConvertUtil.ToInt(pars[2].Value);
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
    }
}
