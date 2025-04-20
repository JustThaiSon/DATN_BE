using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.MembershipBenefit;
using DATN_Models.DAO.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace DATN_Models.DAO
{
    public class MembershipBenefitDAO : IMembershipBenefitDAO
    {
        private static string connectionString = string.Empty;

        public MembershipBenefitDAO()
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        public List<MembershipBenefitDAL> GetAllMembershipBenefits(out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[1];
                pars[0] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<MembershipBenefitDAL>("SP_MembershipBenefit_GetAll", pars);
                response = ConvertUtil.ToInt(pars[0].Value);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db?.Close();
            }
        }

        public List<MembershipBenefitDAL> GetMembershipBenefitsByMembershipId(long membershipId, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_MembershipId", membershipId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<MembershipBenefitDAL>("SP_MembershipBenefit_GetByMembershipId", pars);
                response = ConvertUtil.ToInt(pars[1].Value);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db?.Close();
            }
        }

        public MembershipBenefitDAL GetMembershipBenefitById(long id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_Id", id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<MembershipBenefitDAL>("SP_MembershipBenefit_GetById", pars);
                response = ConvertUtil.ToInt(pars[1].Value);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db?.Close();
            }
        }

        public void CreateMembershipBenefit(MembershipBenefitDAL membershipBenefit, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@_MembershipId", membershipBenefit.MembershipId);
                pars[1] = new SqlParameter("@_BenefitType", membershipBenefit.BenefitType);
                pars[2] = new SqlParameter("@_ConfigJson", membershipBenefit.ConfigJson);
                pars[3] = new SqlParameter("@_Description", membershipBenefit.Description);
                pars[4] = new SqlParameter("@_LogoUrl", membershipBenefit.LogoUrl);
                pars[5] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_MembershipBenefit_Create", pars);
                response = ConvertUtil.ToInt(pars[5].Value);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db?.Close();
            }
        }

        public void UpdateMembershipBenefit(MembershipBenefitDAL membershipBenefit, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[7];
                pars[0] = new SqlParameter("@_Id", membershipBenefit.Id);
                pars[1] = new SqlParameter("@_MembershipId", membershipBenefit.MembershipId);
                pars[2] = new SqlParameter("@_BenefitType", membershipBenefit.BenefitType);
                pars[3] = new SqlParameter("@_ConfigJson", membershipBenefit.ConfigJson);
                pars[4] = new SqlParameter("@_Description", membershipBenefit.Description);
                pars[5] = new SqlParameter("@_LogoUrl", membershipBenefit.LogoUrl);
                pars[6] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_MembershipBenefit_Update", pars);
                response = ConvertUtil.ToInt(pars[6].Value);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db?.Close();
            }
        }

        public void DeleteMembershipBenefit(long id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_Id", id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_MembershipBenefit_Delete", pars);
                response = ConvertUtil.ToInt(pars[1].Value);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db?.Close();
            }
        }
    }
}
