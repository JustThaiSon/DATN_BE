using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Customer.Req
{
    public class ServiceOrderReq
    {
        public string ServiceListJson { get; set; }
        public string Email { get; set; }
        public Guid? UserId { get; set; }
        public bool? IsAnonymous { get; set; } = true;
    }


}
