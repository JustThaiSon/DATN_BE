using FluentValidation;
using DATN_Helpers.Extensions;
namespace DATN_Models.DTOS.Account.Req
{
    public class VerifyOtpReq
    {
        public string Email { get; set; }
        public string Opt { get; set; }
    }
    public class VerifyOtpReqValidator : AbstractValidator<VerifyOtpReq>
    {
        public VerifyOtpReqValidator()
        {
            RuleFor(_ => _.Email).NotNullOrEmpty();
            RuleFor(_ => _.Opt).NotNullOrEmpty();
        }
    }
}
