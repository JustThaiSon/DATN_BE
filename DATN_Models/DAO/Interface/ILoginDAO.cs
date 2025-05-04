using DATN_Models.DAL.Account;
using DATN_Models.DTOS.Account;
using DATN_Models.DTOS.Account.Req;
using DATN_Models.DTOS.Employee.Req;

namespace DATN_Models.DAO.Interface
{
    public interface ILoginDAO
    {

        Task<(int, string)> RegisterUserAsync(CreateAccountReq request);

        Task<(LoginDTO LoginDto, int Response)> login(SignInDAL req);
        Task<(LoginDTO LoginDto, int Response)> LoginByEmployee(LoginEmployeeReq req);
        Task<int> VerifyOtpAndRegisterUserAsync(VerifyOtpReq req);
        Task<int> ChangePasswordAsync(Guid userId, ChangePasswordCustomerReq req);
        GetUserInfoDAL GetUserInfo(Guid userId, out int response);
        //void SaveSession(Guid userId);
        Task<(int,string)> ReSendOpt(ReSendOptReq req);
        Task<(int Code, string Token)> ForgotPassword(ForgotPasswordReq req);
        Task<int> ResetPassword(ResetPasswordReq req);
    }
}
