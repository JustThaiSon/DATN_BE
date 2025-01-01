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
    public class CreateServiceReqValidator : AbstractValidator<CreateServiceReq>
    {
        public CreateServiceReqValidator()
        {
            RuleFor(_ => _.ServiceName).NotNegative().NotNullOrEmpty();
            RuleFor(_ => _.Description).NotNegative().NotNullOrEmpty();
            RuleFor(_ => _.CategoryName).NotNegative().NotNullOrEmpty();
            RuleFor(_ => _.Price).NotNegative().GreaterThanZero();
        }
    }
}
