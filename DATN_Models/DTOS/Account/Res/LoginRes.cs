namespace DATN_Models.DTOS.Account.Res
{
    public class LoginRes
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public List<string> Roles { get; set; }




        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }







    }
}
