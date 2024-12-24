using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Seat;
using DATN_Models.DAL.SeatType;
using DATN_Models.DAO.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAO
{
    public class SeatTypeDAO : ISeatTypeDAO
    {
        private static string connectionString = string.Empty;

        public SeatTypeDAO()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        public void CreateSeatType(CreateSeatTypeDAL dataInput, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_SeatTypeName", dataInput.SeatTypeName);
                pars[1] = new SqlParameter("@_Multiplier", dataInput.Multiplier);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.ExecuteNonQuerySP("SP_SeatType_Create", pars);

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

        public void DeleteSeatType(Guid dataInput, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];

                pars[0] = new SqlParameter("@_SeatTypeId", dataInput);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                 db = new DBHelper(connectionString);

                var result = db.ExecuteNonQuerySP("SP_SeatType_Delete", pars);

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

        public List<GetListSeatTypeDAL> GetListSeatType(int currentPage, int recordPerPage, out int totalRecord, out int response)
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


                var result = db.GetListSP<GetListSeatTypeDAL>("SP_SeatType_GetList", pars);


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

        public void UpdateSeatTypeMultiplier(UpdateSeatTypeMultiplierDAL dataInput, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_SeatTypeId", dataInput.Id);
                pars[1] = new SqlParameter("@_Multiplier", dataInput.Multiplier);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.ExecuteNonQuerySP("SP_SeatType_Update", pars);

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
