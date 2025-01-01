using FluentValidation;
using DATN_Helpers.Extensions;
namespace DATN_Models.DTOS.Account.Req
{
    public class SignInReq
    {
        public string UseName { get; set; }
        public string PassWord { get; set; }
    }
    public class SignInReqValidator : AbstractValidator<SignInReq>
    {
        public SignInReqValidator()
        {
            RuleFor(_ => _.UseName).NotNullOrEmpty();
            RuleFor(_ => _.PassWord).NotNullOrEmpty();
        }
    }
}
