using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Membership;
using DATN_Models.DAO.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;

namespace DATN_Models.DAO
{
    public class MembershipDAO : IMembershipDAO
    {
        private static string connectionString = string.Empty;

        public MembershipDAO()
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        #region Membersghip_nghia

        public List<MembershipDAL> GetListMembership(int currentPage, int recordPerPage, out int totalRecord, out int response)
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

                var result = db.GetListSP<MembershipDAL>("SP_Membership_GetList", pars);

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

        public void CreateMembership(AddMembershipDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[6];

                pars[0] = new SqlParameter("@_Name", req.Name);
                pars[1] = new SqlParameter("@_Description", req.DurationMonths);
                pars[2] = new SqlParameter("@_DiscountPercentage", req.DiscountPercentage);
                pars[3] = new SqlParameter("@_MonthlyFee", req.MonthlyFee);
                pars[4] = new SqlParameter("@_DurationMonths", req.DurationMonths);
                pars[5] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Membership_Create", pars);

                response = ConvertUtil.ToInt(pars[5].Value);
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

        public void DeleteMembership(Guid Id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];

                pars[0] = new SqlParameter("@_MembershipID", Id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Membership_Delete", pars);

                //var result = db.GetListSP<ListActorDAL>("SP_Actor_GetListActor", pars);
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


        public void UpdateMembership(UpdateMembershipDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[9];

                pars[0] = new SqlParameter("@_MembershipID", req.Id);
                pars[1] = new SqlParameter("@_Name", req.Name);
                pars[2] = new SqlParameter("@_Description", req.Description);
                pars[3] = new SqlParameter("@_DiscountPercentage", req.DiscountPercentage);
                pars[4] = new SqlParameter("@_MonthlyFee", req.MonthlyFee);
                pars[5] = new SqlParameter("@_DurationMonths", req.DurationMonths);
                pars[6] = new SqlParameter("@_CreateDate", req.CreatedDate);
                pars[7] = new SqlParameter("@_STATUS", req.Status);
                pars[8] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Membership_Update", pars);

                response = ConvertUtil.ToInt(pars[8].Value);
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

        public MembershipDAL GetMembershipDetail(Guid Id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_MembershipID", Id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.GetInstanceSP<MembershipDAL>("SP_Membership_GetDetail", pars);

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

        #endregion
    }
}
