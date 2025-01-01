using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Service.Request
{
    public class CreateServiceReq
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public string CategoryName { get; set; }
        //public string ImageUrl { get; set; }
    }
}
