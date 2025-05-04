using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Voucher;
using DATN_Models.DAO.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace DATN_Models.DAO
{
    public class VoucherDAO : IVoucherDAO
    {
        private static string connectionString = string.Empty;

        public VoucherDAO()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        public void CreateVoucher(VoucherDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[13];
                pars[0] = new SqlParameter("@_Code", req.Code);
                pars[1] = new SqlParameter("@_Description", req.Description);
                pars[2] = new SqlParameter("@_DiscountType", req.DiscountType);
                pars[3] = new SqlParameter("@_DiscountValue", req.DiscountValue);
                pars[4] = new SqlParameter("@_MinOrderValue", req.MinOrderValue);
                pars[5] = new SqlParameter("@_StartDate", req.StartDate);
                pars[6] = new SqlParameter("@_EndDate", req.EndDate);
                pars[7] = new SqlParameter("@_MaxUsage", req.MaxUsage);
                pars[8] = new SqlParameter("@_Status", req.Status);
                pars[9] = new SqlParameter("@_IsStackable", req.IsStackable);
                pars[10] = new SqlParameter("@_CreatedAt", DateTime.Now);
                pars[11] = new SqlParameter("@_VoucherType", req.VoucherType);
                pars[12] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Voucher_Create", pars);

                response = ConvertUtil.ToInt(pars[12].Value);
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

        public void UpdateVoucher(VoucherDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[14];
                pars[0] = new SqlParameter("@_Id", req.Id);
                pars[1] = new SqlParameter("@_Code", req.Code);
                pars[2] = new SqlParameter("@_Description", req.Description);
                pars[3] = new SqlParameter("@_DiscountType", req.DiscountType);
                pars[4] = new SqlParameter("@_DiscountValue", req.DiscountValue);
                pars[5] = new SqlParameter("@_MinOrderValue", req.MinOrderValue);
                pars[6] = new SqlParameter("@_StartDate", req.StartDate);
                pars[7] = new SqlParameter("@_EndDate", req.EndDate);
                pars[8] = new SqlParameter("@_MaxUsage", req.MaxUsage);
                pars[9] = new SqlParameter("@_Status", req.Status);
                pars[10] = new SqlParameter("@_IsStackable", req.IsStackable);
                pars[11] = new SqlParameter("@_UpdatedAt", DateTime.Now);
                pars[12] = new SqlParameter("@_VoucherType", req.VoucherType);
                pars[13] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Voucher_Update", pars);

                response = ConvertUtil.ToInt(pars[13].Value);
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

        public void DeleteVoucher(Guid id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_Id", id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Voucher_Delete", pars);

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

        public VoucherDAL GetVoucherById(Guid id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_Id", id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<VoucherDAL>("SP_Voucher_GetById", pars);

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

        public VoucherDAL GetVoucherByCode(string code, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_Code", code);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<VoucherDAL>("SP_Voucher_GetByCode", pars);

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

        public List<VoucherDAL> GetListVoucher(int currentPage, int recordPerPage, out int totalRecord, out int response)
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
                var result = db.GetListSP<VoucherDAL>("SP_Voucher_GetList", pars);

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

        public void UseVoucher(VoucherUsageDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_VoucherId", req.VoucherId);
                pars[1] = new SqlParameter("@_UserId", req.UserId ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_OrderId", req.OrderId ?? (object)DBNull.Value);
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Voucher_Use", pars);

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

        public List<VoucherUsageDAL> GetVoucherUsageHistory(Guid voucherId, int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            totalRecord = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_VoucherId", voucherId);
                pars[1] = new SqlParameter("@_CurrentPage", currentPage);
                pars[2] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[4] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<VoucherUsageDAL>("SP_VoucherUsage_GetHistory", pars);

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




        // Thêm phương thức sau vào class VoucherDAO

        public List<VoucherUsageDAL> GetAllVoucherUsage(int currentPage, int recordPerPage, out int totalRecord, out int response)
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
                var result = db.GetListSP<VoucherUsageDAL>("SP_VoucherUsage_GetAll", pars);

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
    }
}