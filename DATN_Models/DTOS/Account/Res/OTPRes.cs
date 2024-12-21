using DATN_Models.DTOS.Account.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Account.Res
{
    public class OtpCacheEntry
    {
        public string Otp { get; set; }
        public CreateAccountReq UserInfo { get; set; }
    }
}
