using DATN_Models.DAO.Interface;
using DATN_Models.DAL.Voucher;
using System.Data;
using Microsoft.Extensions.Configuration;
using DATN_Helpers.Database;
using DATN_Helpers.Common;
using Microsoft.Data.SqlClient;

namespace DATN_Models.DAO
{
    public class UserVoucherDAO : IUserVoucherDAO
    {
        private static string connectionString = string.Empty;

        public UserVoucherDAO()
        {
            var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        public void ClaimVoucher(UserVoucherDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_VoucherId", req.VoucherId);
                pars[1] = new SqlParameter("@_UserId", req.UserId);
                pars[2] = new SqlParameter("@_Quantity", req.Quantity);
                pars[3] = new SqlParameter("@_UsedQuantity", req.UsedQuantity);
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_UserVoucher_Claim", pars);

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

        public List<UserVoucherDAL> GetUserVouchers(Guid userId, int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            totalRecord = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_UserId", userId);
                pars[1] = new SqlParameter("@_CurrentPage", currentPage);
                pars[2] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[4] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<UserVoucherDAL>("SP_UserVoucher_GetByUserId", pars);

                response = ConvertUtil.ToInt(pars[3].Value);
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

        public UserVoucherDAL GetUserVoucherById(Guid id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_Id", id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<UserVoucherDAL>("SP_UserVoucher_GetById", pars);

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

        //public bool CheckUserHasVoucher(Guid userId, Guid voucherId, out int response)
        //{
        //    response = 0;
        //    DBHelper db = null;
        //    try
        //    {
        //        var pars = new SqlParameter[3];
        //        pars[0] = new SqlParameter("@_UserId", userId);
        //        pars[1] = new SqlParameter("@_VoucherId", voucherId);
        //        pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

        //        db = new DBHelper(connectionString);
        //        var result = db.ExecuteScalarSP<int>("SP_UserVoucher_CheckExists", pars);

        //        response = ConvertUtil.ToInt(pars[2].Value);
        //        return result > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        response = -99;
        //        throw;
        //    }
        //    finally
        //    {
        //        if (db != null)
        //            db.Close();
        //    }
        //}

        public void UpdateUserVoucherStatus(Guid id, int status, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_Id", id);
                pars[1] = new SqlParameter("@_Status", status);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_UserVoucher_UpdateStatus", pars);

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

        public List<VoucherDAL> GetAvailableVouchers(int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            totalRecord = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_CurrentPage", currentPage);
                pars[1] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[3] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<VoucherDAL>("SP_Voucher_GetAvailable", pars);

                response = ConvertUtil.ToInt(pars[2].Value);
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



        public void IncreaseUserVoucherQuantity(Guid userId, Guid voucherId, int quantity, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_UserId", userId);
                pars[1] = new SqlParameter("@_VoucherId", voucherId);
                pars[2] = new SqlParameter("@_Quantity", quantity);
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_UserVoucher_IncreaseQuantity", pars);

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

        public void CheckVoucherAvailability(Guid userId, string voucherCode, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_UserId", userId);
                pars[1] = new SqlParameter("@_VoucherCode", voucherCode);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_UserVoucher_CheckAvailavbility", pars);

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
