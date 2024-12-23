using DATN_Models.Models;

namespace DATN_Models.DAL.Account
{
    public class LogicDAL
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public List<AppRoles> Roles { get; set; }
    }
}
