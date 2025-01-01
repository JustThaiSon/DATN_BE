using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Service;
using DATN_Models.DAO.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO
{
    public class ServiceDAO : IServiceDAO
    {
        private static string connectionString = string.Empty;

        public ServiceDAO()
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }
        public void CreateService(CreateServiceDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@_ServiceName", req.ServiceName);
                pars[1] = new SqlParameter("@_Description", req.Description);
                pars[2] = new SqlParameter("@_Price", req.Price);
                pars[3] = new SqlParameter("@_CategoryName", req.CategoryName);
                pars[4] = new SqlParameter("@_ImageUrl", req.ImageUrl);
                pars[5] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.ExecuteNonQuerySP("SP_Service_CreateService", pars);
                response = ConvertUtil.ToInt(pars[5].Value);
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

        public void DeleteService(DeleteServiceDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_ServiceId", req.Id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.ExecuteNonQuerySP("SP_Service_UpdateStatusService", pars);
                response = ConvertUtil.ToInt(pars[1].Value);
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

        public List<GetServiceDAL> GetService(int currentPage, int recordPerPage, out int totalRecord, out int response)
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
                var result = db.GetListSP<GetServiceDAL>("SP_Service_GetService", pars);
                response = ConvertUtil.ToInt(pars[3].Value);
                totalRecord = ConvertUtil.ToInt(pars[2].Value);
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

        public void UpdateService(UpdateServiceDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[7];
                pars[0] = new SqlParameter("@_ServiceId", req.Id);
                pars[1] = new SqlParameter("@_ServiceName", req.ServiceName);
                pars[2] = new SqlParameter("@_Description", req.Description);
                pars[3] = new SqlParameter("@_Price", req.Price);
                pars[4] = new SqlParameter("@_CategoryName", req.CategoryName);
                pars[5] = new SqlParameter("@_ImageUrl", req.ImageUrl);
                pars[6] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.ExecuteNonQuerySP("SP_Service_UpdateService", pars);
                response = ConvertUtil.ToInt(pars[6].Value);
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
    }
}
