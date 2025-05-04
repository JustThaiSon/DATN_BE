using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Membership;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Membership.Req;
using DATN_Models.DTOS.Membership.Res;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.Json;

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

        public List<MembershipDAL> GetAllMemberships(out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[1];
                pars[0] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<MembershipDAL>("SP_Membership_GetAll", pars);
                response = ConvertUtil.ToInt(pars[0].Value);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting all memberships", ex);
            }
            finally
            {
                db?.Close();
            }
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

        public GetmembershipByUserDAL GetmembershipByUser(Guid userId, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_UserId", userId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<GetmembershipByUserDAL>("SP_Membership_GetMembershipByUser", pars);

                response = ConvertUtil.ToInt(pars[1].Value);

                // Parse JSON từ raw string thành object nếu không null
                if (result != null)
                {    
                if (!string.IsNullOrEmpty(result.RawUserMembershipDetails))
                {
                    result.UserMembershipDetails = JsonSerializer.Deserialize<UserMembershipDetailsDAL>(result.RawUserMembershipDetails);
                }
                if (!string.IsNullOrEmpty(result.RawCurrentLevelBenefits))
                {
                    result.CurrentLevelBenefits = JsonSerializer.Deserialize<List<MembershipBenefitDAL>>(result.RawCurrentLevelBenefits);
                }
                if (!string.IsNullOrEmpty(result.RawNextLevelBenefits))
                {
                    result.NextLevelBenefits = JsonSerializer.Deserialize<List<MembershipBenefitDAL>>(result.RawNextLevelBenefits);
                }
                }
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

        public GetPointByUserRes GetPointByUser(Guid userId, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_UserId", userId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<GetPointByUserRes>("SP_Point_GetPointByUser", pars);
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

        public List<GetPointHistoryRes> GetPointHistory(Guid userId,int type, int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@_UserId", userId);
                pars[1] = new SqlParameter("@_Type", type);
                pars[2] = new SqlParameter("@_CurrentPage", currentPage);
                pars[3] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[4] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[5] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetListSP<GetPointHistoryRes>("SP_Point_GetHistoryPoint", pars);
                response = ConvertUtil.ToInt(pars[5].Value);
                totalRecord = ConvertUtil.ToInt(pars[4].Value);
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

        public MembershipPreviewDAL MembershipPreview(Guid userId, long orderPrice, long ticketPrice,int type, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[5]; 
                pars[0] = new SqlParameter("@_Type", type); 
                pars[1] = new SqlParameter("@_UserId", userId);
                pars[2] = new SqlParameter("@_OrderPrice", orderPrice);
                pars[3] = new SqlParameter("@_TicketPrice", ticketPrice);
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<MembershipPreviewDAL>("SP_Membership_GetPreview", pars);

                if (result != null)
                {
                    result.ParseFreeServiceString();
                }

                response = ConvertUtil.ToInt(pars[4].Value);
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

        public GetMembershipRes GetMembership(long membershipId, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_MemberShipId", membershipId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<GetMembershipRes>("SP_Membership_GetMembershipPrice", pars);


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
    }
}
