using DATN_Models.DAL.Account;
using DATN_Models.DTOS.Account;
using DATN_Models.DTOS.Account.Req;

namespace DATN_Models.DAO.Interface
{
    public interface ILoginDAO
    {

        Task<(int, string)> RegisterUserAsync(CreateAccountReq request);

        Task<(LoginDTO LoginDto, int Response)> login(SignInDAL req);
        Task<int> VerifyOtpAndRegisterUserAsync(VerifyOtpReq req);
        //void SaveSession(Guid userId);
    }
}
