using DATN_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Account
{
    public class LoginDTO
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public List<AppRoles> Roles { get; set; }
    }

}
