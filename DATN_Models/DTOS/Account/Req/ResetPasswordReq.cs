namespace DATN_Models.DTOS.Account.Req
{
   public class ResetPasswordReq
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
