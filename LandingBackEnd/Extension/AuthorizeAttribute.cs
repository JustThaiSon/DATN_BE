 using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DTOS.Account.Res;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DATN_LandingPage.Extension
{
    public class BAuthorizeAttribute : TypeFilterAttribute
    {
        public BAuthorizeAttribute() : base(typeof(AuthorizeAttributeImpl))
        {

        }
        private class AuthorizeAttributeImpl : Attribute, IActionFilter, IAsyncActionFilter
        {
            private readonly IUltil _ultils;
            private readonly string _langCode;
            public AuthorizeAttributeImpl(IUltil ultils, IConfiguration configuration)
            {
                _ultils = ultils;
                _langCode = configuration["ProjectSettings:LanguageCode"] ?? "vi";
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {

            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                // Do something after the action executes.
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var check = CheckUserPemission(context);
                if (!check) return;

                await next();
            }

            #region Private method
            private bool CheckUserPemission(ActionExecutingContext context)
            {
                int INVALID_TOKEN = 1015;

                HttpRequest httpRequest = context.HttpContext.Request;
                string path = context.ActionDescriptor.AttributeRouteInfo.Template;
                string action = httpRequest.Method;

                var bearerToken = httpRequest.Headers["Authorization"];
                var token = !string.IsNullOrEmpty(bearerToken) ? bearerToken.ToString().Substring("Bearer ".Length) : null;
                var (userID, Roles) = _ultils.ValidateToken(token);
                if ((userID, Roles) == (null, null))
                {
                    var res = new CommonResponse<LoginRes>();
                    res.ResponseCode = INVALID_TOKEN;
                    res.Message = MessageUtils.GetMessage(res.ResponseCode, _langCode);

                    context.Result = new UnauthorizedObjectResult(res);
                    return false;
                }
                context.HttpContext.Items["UserId"] = userID;
                context.HttpContext.Items["Roles"] = Roles;
                return true;
            }
            #endregion
        }
    }
}

