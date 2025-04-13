using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Membership;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Membership.Req;
using DATN_Models.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

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

        public void AddUserMembership(Guid userId, AddUserMembershipReq userMembership, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_MembershipID", userMembership.MembershipId);
                pars[1] = new SqlParameter("@_UserId", userId);
                pars[2] = new SqlParameter("@_PaymentMethodId", userMembership.PaymentMethodId ?? (object)DBNull.Value);
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Membership_UserMembership", pars);
                response = ConvertUtil.ToInt(pars[3].Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding user membership", ex);
            }
            finally
            {
                db?.Close();
            }
        }

        public CheckMemberShipDAL CheckMembership(Guid userId, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_UserId", userId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<CheckMemberShipDAL>("SP_Membership_CheckMembership", pars);
                response = ConvertUtil.ToInt(pars[1].Value);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding user membership", ex);
            }
            finally
            {
                db?.Close();
            }
        }

        public MembershipPreviewDAL MembershipPreview(Guid userId, long orderPrice, long ticketPrice, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_UserId", userId);
                pars[1] = new SqlParameter("@_OrderPrice", orderPrice);
                pars[2] = new SqlParameter("@_TicketPrice", ticketPrice);
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<MembershipPreviewDAL>("SP_Membership_GetPreview", pars);
                result.ParseFreeServiceString();
                response = ConvertUtil.ToInt(pars[3].Value);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding user membership", ex);
            }
            finally
            {
                db?.Close();
            }
            ;
        }
    }
}
