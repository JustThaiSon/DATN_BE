using DATN_Helpers.Extensions;
using FluentValidation;

namespace DATN_Models.DTOS.Service.Request
{
    public class DeleteServiceReq
    {
        public Guid? Id { get; set; }
    }
    public class DeleteServiceReqValidator : AbstractValidator<DeleteServiceReq>
    { 
        public DeleteServiceReqValidator()
        {
            RuleFor(_ => _.Id).NotNegative().NotNullOrEmpty();
        }
    }
}
