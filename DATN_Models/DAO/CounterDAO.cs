using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Counter;
using DATN_Models.DAL.ShowTime;
using DATN_Models.DAO.Interface;
using DATN_Models.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DATN_Models.DAO
{
    public class CounterDAO : ICounterDAO
    {
        private static string connectionString = string.Empty;

        public CounterDAO()
        {
            var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        public List<Counter_NowPlayingMovies_GetList_DAL> GetNowPlayingMovies(
            int currentPage,
            int recordPerPage,
            DateTime? showDate,
            Guid? cinemaId,
            Guid? genreId,
            out int totalRecord,
            out int response)
        {
            response = 0;
            totalRecord = 0;
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure
                var pars = new SqlParameter[7];
                pars[0] = new SqlParameter("@_CurrentPage", currentPage);
                pars[1] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[2] = new SqlParameter("@_ShowDate", showDate ?? (object)DBNull.Value);
                pars[3] = new SqlParameter("@_CinemaId", cinemaId ?? (object)DBNull.Value);
                pars[4] = new SqlParameter("@_GenreId", genreId ?? (object)DBNull.Value);
                pars[5] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[6] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);
                var result = db.GetListSP<Counter_NowPlayingMovies_GetList_DAL>("SP_Counter_NowPlayingMovies_GetList", pars);

                // Lấy giá trị các tham số đầu ra
                response = ConvertUtil.ToInt(pars[6].Value);
                totalRecord = ConvertUtil.ToInt(pars[5].Value);

                return result;
            }
            catch (Exception ex)
            {
                response = -99; // Mã lỗi hệ thống
                throw;
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

        public DataSet GetTicketInfoFromQR(
            string orderCode,
            out int response,
            out string message)
        {
            response = 0;
            message = string.Empty;
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_OrderCode", orderCode);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[2] = new SqlParameter("@_Message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);

                // Sử dụng DataSet thay vì class cụ thể vì SP trả về nhiều bảng
                DataSet result = db.ExecuteDataSetSP("SP_Counter_QRTicket_GetInfo", pars);

                // Lấy giá trị các tham số đầu ra
                response = ConvertUtil.ToInt(pars[1].Value);
                message = pars[2].Value.ToString();

                return result;
            }
            catch (Exception ex)
            {
                response = -99; // Mã lỗi hệ thống
                message = ex.Message;
                throw;
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

        public bool ConfirmTicketUsage(
            string orderCode,
            out int response,
            out string message)
        {
            response = 0;
            message = string.Empty;
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_OrderCode", orderCode);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[2] = new SqlParameter("@_Message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Counter_QRTicket_ConfirmUsage", pars);

                // Lấy giá trị các tham số đầu ra
                response = ConvertUtil.ToInt(pars[1].Value);
                message = pars[2].Value.ToString();

                // Nếu response là 200 (thành công)
                return response == 200;
            }
            catch (Exception ex)
            {
                response = -99; // Mã lỗi hệ thống
                message = ex.Message;
                throw;
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

        public DataSet ManageServiceOrder(int action, string orderCode, string email, Guid? userId, bool isAnonymous, string serviceListJson, out int response, out string message, out string orderCodeOut)
        {
            response = 0;
            message = string.Empty;
            orderCodeOut = string.Empty;
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure
                var pars = new SqlParameter[9];
                pars[0] = new SqlParameter("@_Action", action);
                pars[1] = new SqlParameter("@_OrderCode", (orderCode != null) ? orderCode : (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Email", (email != null) ? email : (object)DBNull.Value);
                pars[3] = new SqlParameter("@_UserId", userId ?? (object)DBNull.Value);
                pars[4] = new SqlParameter("@_IsAnonymous", isAnonymous);
                pars[5] = new SqlParameter("@_ServiceList", (serviceListJson != null) ? serviceListJson : (object)DBNull.Value);
                pars[6] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[7] = new SqlParameter("@_Message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                pars[8] = new SqlParameter("@_OrderCodeOut", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);

                // Nếu là action 4 (hoàn tất), sử dụng ExecuteDataSetSP để lấy kết quả cho hóa đơn
                DataSet result = (action == 4)
                    ? db.ExecuteDataSetSP("SP_Counter_ManageServiceOrder", pars)
                    : null;

                // Nếu không phải action 4, chỉ cần thực thi SP không cần kết quả trả về
                if (action != 4)
                {
                    db.ExecuteNonQuerySP("SP_Counter_ManageServiceOrder", pars);
                }

                // Lấy giá trị các tham số đầu ra
                response = ConvertUtil.ToInt(pars[6].Value);
                message = pars[7].Value.ToString();
                orderCodeOut = pars[8].Value.ToString();

                return result; // Có thể null với các action khác action 4
            }
            catch (Exception ex)
            {
                response = -99; // Mã lỗi hệ thống
                message = ex.Message;
                throw;
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

        public DataSet CreateServiceOrder(string email, Guid? userId, bool isAnonymous, string serviceListJson, out int response, out string message, out string orderCodeOut)
        {
            response = 0;
            message = string.Empty;
            orderCodeOut = string.Empty;
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure
                var pars = new SqlParameter[7];
                pars[0] = new SqlParameter("@_Email", (email != null) ? email : (object)DBNull.Value);
                pars[1] = new SqlParameter("@_UserId", userId ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_IsAnonymous", isAnonymous);
                pars[3] = new SqlParameter("@_ServiceList", serviceListJson);
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[5] = new SqlParameter("@_Message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                pars[6] = new SqlParameter("@_OrderCodeOut", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);
                DataSet result = db.ExecuteDataSetSP("SP_Counter_CreateServiceOrder", pars);

                // Lấy giá trị các tham số đầu ra
                response = ConvertUtil.ToInt(pars[4].Value);
                message = pars[5].Value.ToString();
                orderCodeOut = pars[6].Value.ToString();

                return result;
            }
            catch (Exception ex)
            {
                response = -99; // Mã lỗi hệ thống
                message = ex.Message;
                throw;
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

        public DataSet ConfirmServicePayment(string orderCode, Guid? userId, out int response, out string message)
        {
            response = 0;
            message = string.Empty;
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_OrderCode", orderCode);
                pars[1] = new SqlParameter("@_UserId", userId ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[3] = new SqlParameter("@_Message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);
                DataSet result = db.ExecuteDataSetSP("SP_Counter_ConfirmServicePayment", pars);

                // Lấy giá trị các tham số đầu ra
                response = ConvertUtil.ToInt(pars[2].Value);
                message = pars[3].Value.ToString();

                return result;
            }
            catch (Exception ex)
            {
                response = -99; // Mã lỗi hệ thống
                message = ex.Message;
                throw;
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

        public DataSet ConfirmServiceUsage(string orderCode, Guid? userId, out int response, out string message)
        {
            response = 0;
            message = string.Empty;
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_OrderCode", orderCode);
                pars[1] = new SqlParameter("@_UserId", userId ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[3] = new SqlParameter("@_Message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);
                DataSet result = db.ExecuteDataSetSP("SP_Counter_ConfirmServiceUsage", pars);

                // Lấy giá trị các tham số đầu ra
                response = ConvertUtil.ToInt(pars[2].Value);
                message = pars[3].Value.ToString();

                return result;
            }
            catch (Exception ex)
            {
                response = -99; // Mã lỗi hệ thống
                message = ex.Message;
                throw;
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

        public DataSet CompleteServiceTransaction(string orderCode, Guid userId, out int response, out string message)
        {
            response = 0;
            message = string.Empty;
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_OrderCode", orderCode);
                pars[1] = new SqlParameter("@_UserId", userId);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[3] = new SqlParameter("@_Message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);
                DataSet result = db.ExecuteDataSetSP("SP_Counter_CompleteServiceTransaction", pars);

                // Lấy giá trị các tham số đầu ra
                response = ConvertUtil.ToInt(pars[2].Value);
                message = pars[3].Value.ToString();

                return result;
            }
            catch (Exception ex)
            {
                response = -99; // Mã lỗi hệ thống
                message = ex.Message;
                throw;
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

        public DataSet GetServiceOrderInfo(string orderCode)
        {
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_OrderCode", orderCode);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[2] = new SqlParameter("@_Message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);
                DataSet result = db.ExecuteDataSetSP("SP_Counter_GetServiceOrderInfo", pars);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

        public string QuickServiceSale(string serviceListJson, Guid userId, string customerEmail, bool markAsUsed, out int response, out string message, out decimal totalAmount)
        {
            response = 0;
            message = string.Empty;
            totalAmount = 0;
            string orderCode = string.Empty;
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure
                var pars = new SqlParameter[8];
                pars[0] = new SqlParameter("@_ServiceList", serviceListJson);
                pars[1] = new SqlParameter("@_UserId", userId);
                pars[2] = new SqlParameter("@_CustomerEmail", (customerEmail != null) ? customerEmail : (object)DBNull.Value);
                pars[3] = new SqlParameter("@_MarkAsUsed", markAsUsed);
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[5] = new SqlParameter("@_Message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                pars[6] = new SqlParameter("@_OrderCode", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                pars[7] = new SqlParameter("@_TotalAmount", SqlDbType.Decimal) { Direction = ParameterDirection.Output };

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Counter_QuickServiceSale", pars);

                // Lấy giá trị các tham số đầu ra
                response = ConvertUtil.ToInt(pars[4].Value);
                message = pars[5].Value.ToString();
                orderCode = pars[6].Value.ToString();
                totalAmount = ConvertUtil.ToDecimal(pars[7].Value);

                return orderCode;
            }
            catch (Exception ex)
            {
                response = -99; // Mã lỗi hệ thống
                message = ex.Message;
                throw;
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

        // Thêm vào DATN_Models/DAO/CounterDAO.cs
        public string CreateTicketAndServiceOrder(
            string email,
            Guid? userId,
            bool isAnonymous,
            Guid showTimeId,
            string seatListJson,
            string serviceListJson,
            out int response,
            out string message)
        {
            response = 0;
            message = string.Empty;
            string orderCodeOut = string.Empty;
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure
                var pars = new SqlParameter[9];
                pars[0] = new SqlParameter("@_Email", (email != null) ? email : (object)DBNull.Value);
                pars[1] = new SqlParameter("@_UserId", userId ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_IsAnonymous", isAnonymous);
                pars[3] = new SqlParameter("@_ShowTimeId", showTimeId);
                pars[4] = new SqlParameter("@_SeatList", seatListJson);
                pars[5] = new SqlParameter("@_ServiceList", (serviceListJson != null) ? serviceListJson : (object)DBNull.Value);
                pars[6] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[7] = new SqlParameter("@_Message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                pars[8] = new SqlParameter("@_OrderCodeOut", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Counter_CreateTicketAndServiceOrder", pars);

                // Lấy giá trị các tham số đầu ra
                response = ConvertUtil.ToInt(pars[6].Value);
                message = pars[7].Value.ToString();
                orderCodeOut = pars[8].Value.ToString();

                return orderCodeOut;
            }
            catch (Exception ex)
            {
                response = -99; // Mã lỗi hệ thống
                message = ex.Message;
                throw;
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

        public bool ConfirmTicketAndServicePayment(
            string orderCode,
            Guid? userId,  // Đã bỏ tham số paymentMethodId
            out int response,
            out string message)
        {
            response = 0;
            message = string.Empty;
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure - giảm xuống còn 4 tham số
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_OrderCode", orderCode);
                pars[1] = new SqlParameter("@_UserId", userId ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[3] = new SqlParameter("@_Message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Counter_ConfirmTicketAndServicePayment", pars);

                // Lấy giá trị các tham số đầu ra
                response = ConvertUtil.ToInt(pars[2].Value);
                message = pars[3].Value.ToString();

                // Trả về kết quả thành công hay không
                return response == 200;
            }
            catch (Exception ex)
            {
                response = -99; // Mã lỗi hệ thống
                message = ex.Message;
                throw;
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

        public DataSet GetTicketAndServiceOrderInfo(
            string orderCode,
            out int response,
            out string message)
        {
            response = 0;
            message = string.Empty;
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_OrderCode", orderCode);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[2] = new SqlParameter("@_Message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);
                DataSet result = db.ExecuteDataSetSP("SP_Counter_GetTicketAndServiceOrderInfo", pars);

                // Lấy giá trị các tham số đầu ra
                response = ConvertUtil.ToInt(pars[1].Value);
                message = pars[2].Value.ToString();

                return result;
            }
            catch (Exception ex)
            {
                response = -99; // Mã lỗi hệ thống
                message = ex.Message;
                throw;
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

        public bool CancelUnpaidOrder(
            string orderCode,
            Guid? userId,
            out int response,
            out string message)
        {
            response = 0;
            message = string.Empty;
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_OrderCode", orderCode);
                pars[1] = new SqlParameter("@_UserId", userId ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[3] = new SqlParameter("@_Message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Counter_CancelUnpaidOrder", pars);

                // Lấy giá trị các tham số đầu ra
                response = ConvertUtil.ToInt(pars[2].Value);
                message = pars[3].Value.ToString();

                // Trả về kết quả thành công hay không
                return response == 200;
            }
            catch (Exception ex)
            {
                response = -99; // Mã lỗi hệ thống
                message = ex.Message;
                throw;
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

    }
}