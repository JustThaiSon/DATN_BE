using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.PricingRule;
using DATN_Models.DAO.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO
{
    public class PricingRuleDAO : IPricingRuleDAO
    {

        private static string connectionString = string.Empty;

        public PricingRuleDAO()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        public List<GetListPricingRuleDAL> GetListPricingRule(int currentPage, int recordPerPage, out int totalRecord, out int response)
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


                var result = db.GetListSP<GetListPricingRuleDAL>("SP_PricingRule_GetList", pars);


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

        public void CreatePricingRule(CreatePricingRuleDAL dataInput, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[12];
                pars[0] = new SqlParameter("@_RuleName", dataInput.RuleName);
                pars[1] = new SqlParameter("@_Multiplier", dataInput.Multiplier);
                pars[2] = new SqlParameter("@_StartTime", dataInput.StartTime ?? (object)DBNull.Value);
                pars[3] = new SqlParameter("@_EndTime", dataInput.EndTime ?? (object)DBNull.Value);
                pars[4] = new SqlParameter("@_StartDate", dataInput.StartDate ?? (object)DBNull.Value);
                pars[5] = new SqlParameter("@_EndDate", dataInput.EndDate ?? (object)DBNull.Value);
                pars[6] = new SqlParameter("@_Date", dataInput.Date ?? (object)DBNull.Value);
                pars[7] = new SqlParameter("@_SpecialDay", dataInput.SpecialDay ?? (object)DBNull.Value);
                pars[8] = new SqlParameter("@_SpecialMonth", dataInput.SpecialMonth ?? (object)DBNull.Value);
                pars[9] = new SqlParameter("@_DayOfWeek", dataInput.DayOfWeek ?? (object)DBNull.Value);
                pars[10] = new SqlParameter("@_IsDiscount", dataInput.IsDiscount);

                pars[11] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.ExecuteNonQuerySP("SP_PricingRule_Create", pars);
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
        public void UpdatePricingRule(UpdatePricingRuleDAL dataInput, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_PricingRuleId", dataInput.PricingRuleId);
                pars[1] = new SqlParameter("@_Multiplier", dataInput.Multiplier);


                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.ExecuteNonQuerySP("SP_PricingRule_Update_Multiplier", pars);

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
        public void DeletePricingRule(Guid dataInput, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];

                pars[0] = new SqlParameter("@_PricingRuleId", dataInput);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.ExecuteNonQuerySP("SP_PricingRule_Delete", pars);

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
