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
    }
}