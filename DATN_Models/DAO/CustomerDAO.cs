using DATN_Helpers.Common;
using DATN_Helpers.Constants;
using DATN_Helpers.Database;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Customer;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Customer.Req;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO
{
    public class CustomerDAO : ICustomerDAO
    {

        private static string connectionString = string.Empty;

        public CustomerDAO()
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        public List<GetListCustomerInformationDAL> GetListCustomer(int currentPage, int recordPerPage, out int totalRecord, out int response)
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

                var result = db.GetListSP<GetListCustomerInformationDAL>("SP_Customer_GetList", pars);

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
        public GetListCustomerInformationDAL GetCustomerDetail(Guid Id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_CustomerID", Id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.GetInstanceSP<GetListCustomerInformationDAL>("SP_Customer_GetDetail", pars);

                response = ConvertUtil.ToInt(pars[1].Value);

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

        public void DeleteCustomer(Guid CustomerId, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_CustomerID", CustomerId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                db.ExecuteNonQuerySP("SP_Customer_Delete", pars);

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


        public void UpdateCustomer(Guid CustomerId, UpdateCustomerDAL req, out int response)
        {
            response = 0;
            if (!StringExtension.IsValidEmail(req.Email))
            {
                response = (int)ResponseCodeEnum.ERR_INVALID_EMAIL;
                return;
            }

            if (!StringExtension.IsValidPhoneNumber(req.PhoneNumber))
            {
                response = (int)ResponseCodeEnum.ERR_INVALID_PHONENUMBER;
                return;
            }


            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[10];
                pars[0] = new SqlParameter("@_CustomerID", CustomerId);
                pars[1] = new SqlParameter("@_Name", req.Name);
                pars[2] = new SqlParameter("@_Email", req.Email);
                pars[3] = new SqlParameter("@_PhoneNumber", req.PhoneNumber);
                pars[4] = new SqlParameter("@_Address", req.Address);
                pars[5] = new SqlParameter("@_Sex", req.Sex);
                pars[6] = new SqlParameter("@_DOB", req.Dob);
                pars[7] = new SqlParameter("@_Status", req.Status);
                pars[8] = new SqlParameter("@_CreateDate", req.CreatedDate);
                pars[9] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.ExecuteNonQuerySP("SP_Customer_Update", pars);

                response = ConvertUtil.ToInt(pars[9].Value);
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
        public void Lockout(Guid Id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_CustomerID", Id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                db.ExecuteNonQuery("SP_Customer_Lockout", pars);

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
