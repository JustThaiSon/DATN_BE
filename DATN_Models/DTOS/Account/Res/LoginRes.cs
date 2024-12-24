namespace DATN_Models.DTOS.Account.Res
{
    public class LoginRes
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public List<string> Roles { get; set; }
    }
}
