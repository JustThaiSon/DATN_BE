using DATN_Models.DTOS.Account.Req;

namespace DATN_Models.DTOS.Account.Res
{
    public class OtpCacheEntry
    {
        public string Otp { get; set; }
        public CreateAccountReq UserInfo { get; set; }
    }
}
