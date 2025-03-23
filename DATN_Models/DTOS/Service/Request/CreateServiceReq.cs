using DATN_Helpers.Extensions;
using FluentValidation;

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
