using DATN_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Account
{
    public class LogicDAL
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public List<AppRoles> Roles { get; set; }
    }
}
