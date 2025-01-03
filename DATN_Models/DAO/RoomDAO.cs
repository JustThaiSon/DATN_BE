using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Room;
using DATN_Models.DAO.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO
{
    public class RoomDAO : IRoomDAO
    {
        private static string connectionString = string.Empty;

        public RoomDAO()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }
        public void CreateRoom(CreateRoomDAL resquest, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_CinemaId", resquest.CinemaId);
                pars[1] = new SqlParameter("@_Name", resquest.Name);
                pars[2] = new SqlParameter("@_TotalColNumber", resquest.TotalColNumber);
                pars[3] = new SqlParameter("@_TotalRowNumber", resquest.TotalRowNumber);
                pars[3] = new SqlParameter("@_SeatPrice", resquest.SeatPrice);
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Room_Create", pars);

                response = ConvertUtil.ToInt(pars[4].Value);
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


        public List<ListRoomDAL> GetListRoom(int currentPage, int recordPerPage, out int totalRecord, out int response)
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



                var result = db.GetListSP<ListRoomDAL>("SP_Room_GetList", pars);


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
    }
}
