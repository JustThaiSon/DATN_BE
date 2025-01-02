using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Seat;
using DATN_Models.DAO.Interface.SeatAbout;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO
{
    public class SeatDAO : ISeatDAO
    {
        private static string connectionString = string.Empty;

        public SeatDAO()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        public List<ListSeatDAL> GetListSeat(Guid id, int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_RoomId", id);
                pars[1] = new SqlParameter("@_CurrentPage", currentPage);
                pars[2] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[3] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);



                var result = db.GetListSP<ListSeatDAL>("SP_Seat_GetList", pars);


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

        public List<ListSeatByShowTimeDAL> GetListSeatByShowTime(Guid roomId, Guid showTimeId, int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@_RoomId", roomId);
                pars[1] = new SqlParameter("@_ShowTimeId", showTimeId);
                pars[2] = new SqlParameter("@_CurrentPage", currentPage);
                pars[3] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[4] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[5] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);



                var result = db.GetListSP<ListSeatByShowTimeDAL>("SP_SeatByShowTime_GetList", pars);


                response = ConvertUtil.ToInt(pars[5].Value);
                totalRecord = ConvertUtil.ToInt(pars[4].Value);

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

        public void UpdateSeatStatus(UpdateSeatStatusDAL dataInput, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_SeatId", dataInput.Id);
                pars[1] = new SqlParameter("@_SeatStatus", dataInput.Status);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.ExecuteNonQuerySP("SP_Seat_Update_Status", pars);

                response = ConvertUtil.ToInt(pars[2].Value);
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

        public void UpdateSeatType(UpdateSeatTypeDAL dataInput, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_SeatId", dataInput.Id);
                pars[1] = new SqlParameter("@_SeatTypeId", dataInput.SeatTypeId);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.ExecuteNonQuerySP("SP_Seat_Update_Type", pars);

                response = ConvertUtil.ToInt(pars[2].Value);
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
    }
}
