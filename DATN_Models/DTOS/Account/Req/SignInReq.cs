using FluentValidation;
using DATN_Helpers.Extensions;
namespace DATN_Models.DTOS.Account.Req
{
    public class SignInReq
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
    public class SignInReqValidator : AbstractValidator<SignInReq>
    {
        public SignInReqValidator()
        {
            RuleFor(_ => _.UserName).NotNullOrEmpty();
            RuleFor(_ => _.PassWord).NotNullOrEmpty();
        }
    }
}
