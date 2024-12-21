using DATN_Models.DAL.Account;
using DATN_Models.DTOS.Account;
using DATN_Models.DTOS.Account.Req;
using DATN_Models.DTOS.Account.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAO.Interface
{
    public interface ILoginDAO
    {
        Task<(int, string)> RegisterUserAsync(CreateAccountReq request);
        Task<(LoginDTO LoginDto, int Response)> login(string userName, string passWord);
        Task<int> VerifyOtpAndRegisterUserAsync(string email, string otp);
    }
}
