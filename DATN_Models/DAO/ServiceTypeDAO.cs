using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Orders;
using DATN_Models.DAL.Service;
using DATN_Models.DAL.ServiceType;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.ServiceType.Req;
using DATN_Models.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO
{
    public class ServiceTypeDAO : IServiceTypeDAO
    {
        private static string connectionString = string.Empty;

        public ServiceTypeDAO()
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        public List<ServiceTypeDAL> GetServiceTypeList(int currentPage, int recordPerPage, out int totalRecord, out int response)
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
                var (servicetype, services) = db.GetMultipleSP<ServiceTypeDAL, GetServiceDAL>("SP_ServiceType_GetList", pars);

                foreach (var item in servicetype)
                {
                    item.serviceList = services.Where(x => x.ServiceTypeID == item.Id).ToList();
                }


                response = ConvertUtil.ToInt(pars[3].Value);
                totalRecord = ConvertUtil.ToInt(pars[2].Value);

                return servicetype;
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

        public ServiceTypeDAL GetServiceTypeById(Guid id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_ServiceTypeID", id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var (serviceType, services) = db.GetSingleSP<ServiceTypeDAL, GetServiceDAL>("SP_ServiceType_GetById", pars);

                if (serviceType != null)
                {
                    serviceType.serviceList = services.Where(x => x.ServiceTypeID == serviceType.Id).ToList();
                }

                response = ConvertUtil.ToInt(pars[1].Value);
                return serviceType;
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

        public void CreateServiceType(CreateServiceTypeDAL request, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_Name", request.Name);
                pars[1] = new SqlParameter("@_Description", request.Description);
                pars[2] = new SqlParameter("@_ImageURL", request.ImageUrl);
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_ServiceType_Create", pars);

                response = ConvertUtil.ToInt(pars[3].Value);
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

        public void UpdateServiceType(UpdateServiceTypeDAL request, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_Id", request.Id);
                pars[1] = new SqlParameter("@_Name", request.Name);
                pars[2] = new SqlParameter("@_Description", request.Description);
                pars[3] = new SqlParameter("@_ImageURL", request.ImageUrl);
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_ServiceType_Update", pars);

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

        public void DeleteServiceType(Guid id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_Id", id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_ServiceType_Delete", pars);

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
    }
}