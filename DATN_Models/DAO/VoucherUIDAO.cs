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
    public class VoucherUIDAO : IVoucherUIDAO
    {
        private static string connectionString = string.Empty;

        public VoucherUIDAO()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        public void CreateVoucherUI(VoucherUIDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[10];
                pars[0] = new SqlParameter("@_VoucherId", req.VoucherId);
                pars[1] = new SqlParameter("@_Title", req.Title);
                pars[2] = new SqlParameter("@_Content", req.Content);
                pars[3] = new SqlParameter("@_ImageUrl", req.ImageUrl);
                pars[4] = new SqlParameter("@_DisplayOrder", req.DisplayOrder);
                pars[5] = new SqlParameter("@_StartTime", req.StartTime);
                pars[6] = new SqlParameter("@_EndTime", req.EndTime);
                pars[7] = new SqlParameter("@_Status", req.Status);
                pars[8] = new SqlParameter("@_CreatedAt", DateTime.Now);
                pars[9] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_VoucherUI_Create", pars);

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

        public void UpdateVoucherUI(VoucherUIDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[11];
                pars[0] = new SqlParameter("@_Id", req.Id);
                pars[1] = new SqlParameter("@_VoucherId", req.VoucherId);
                pars[2] = new SqlParameter("@_Title", req.Title);
                pars[3] = new SqlParameter("@_Content", req.Content);
                pars[4] = new SqlParameter("@_ImageUrl", req.ImageUrl);
                pars[5] = new SqlParameter("@_DisplayOrder", req.DisplayOrder);
                pars[6] = new SqlParameter("@_StartTime", req.StartTime);
                pars[7] = new SqlParameter("@_EndTime", req.EndTime);
                pars[8] = new SqlParameter("@_Status", req.Status);
                pars[9] = new SqlParameter("@_UpdatedAt", DateTime.Now);
                pars[10] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_VoucherUI_Update", pars);

                response = ConvertUtil.ToInt(pars[10].Value);
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

        public void DeleteVoucherUI(Guid id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_Id", id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_VoucherUI_Delete", pars);

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

        public VoucherUIDAL GetVoucherUIById(Guid id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_Id", id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<VoucherUIDAL>("SP_VoucherUI_GetById", pars);

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

        public List<VoucherUIDAL> GetListVoucherUI(int currentPage, int recordPerPage, out int totalRecord, out int response)
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
                var result = db.GetListSP<VoucherUIDAL>("SP_VoucherUI_GetList", pars);

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
