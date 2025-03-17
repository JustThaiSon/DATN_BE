using DATN_Helpers.Extensions;
using FluentValidation;
namespace DATN_Models.DTOS.Template.Req
{
    public class TemplateReq
    {
        public string Play { get; set; }
        public int Number { get; set; }
    }
    public class TemplateReqValidator : AbstractValidator<TemplateReq>
    {
        public TemplateReqValidator()
        {
            RuleFor(_ => _.Play).NotNegative().NotNullOrEmpty();
            RuleFor(_ => _.Number).NotNegative().GreaterThanZero();
        }
    }

}
