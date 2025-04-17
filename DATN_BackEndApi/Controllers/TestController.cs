using DATN_BackEndApi.Extension;
using DATN_Helpers.Common;
using DATN_Helpers.Constants;
using DATN_Helpers.Extensions;
using DATN_Models.DTOS.Comments.Req;
using DATN_Models.DTOS.Template.Req;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [BAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<CommonResponse<dynamic>> Get()
        {
            var _langCode = GetLanguageCode();
            var í = HttpContextHelper.GetUserId();
            var roles = HttpContextHelper.GetRoles();
            var res = new CommonResponse<dynamic>();
            res.ResponseCode = (int)ResponseCodeEnum.SUCCESS;
            res.Message = MessageUtils.GetMessage((int)ResponseCodeEnum.SUCCESS, _langCode);
            res.Data = null;
            return res;
        }


        [HttpGet]
        [Route("CheckQuyen")]
        public async Task<CommonResponse<dynamic>> CheckQuyen()
        {
            var _langCode = GetLanguageCode();
            var Roles = HttpContextHelper.GetRoles();
            var res = new CommonResponse<dynamic>();
            if (Roles.Contains("Admin"))
            {
                res.ResponseCode = (int)ResponseCodeEnum.SUCCESS;
                res.Message = MessageUtils.GetMessage((int)ResponseCodeEnum.SUCCESS, _langCode);
                res.Data = null;
            }
            var í = HttpContextHelper.GetUserId();
            var roles = HttpContextHelper.GetRoles();
            res.ResponseCode = (int)ResponseCodeEnum.SUCCESS;
            res.Message = MessageUtils.GetMessage((int)ResponseCodeEnum.SUCCESS, _langCode);
            res.Data = null;
            return res;
        }

        [HttpPost("Testt")]
        public async Task<CommonResponse<dynamic>> Testt(TemplateReq req)
        {
            var _langCode = GetLanguageCode();
            var res = new CommonResponse<dynamic>();
            res.ResponseCode = (int)ResponseCodeEnum.SUCCESS;
            res.Message = MessageUtils.GetMessage((int)ResponseCodeEnum.SUCCESS, _langCode);
            res.Data = null;
            return res;
        }
        [HttpPost("SetLanguage")]
        public IActionResult SetLanguage([FromBody] string langCode)
        {
            if (langCode != "vi" && langCode != "en")
            {
                return BadRequest("Unsupported language code.");
            }

            HttpContext.Response.Cookies.Append("LanguageCode", langCode, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            });

            return Ok(new { Message = "Language updated successfully.", LanguageCode = langCode });
        }
        [HttpPost]
        [Route("userinfo_test_nghia")]
        //[BAuthorize]
        public async Task<CommonResponse<dynamic>> userinfo(CreateCommentReq req)
        {
            var userId = HttpContextHelper.GetUserId();

            var res = new CommonResponse<dynamic>();
            res.ResponseCode = (int)ResponseCodeEnum.SUCCESS;
            res.Message = MessageUtils.GetMessage((int)ResponseCodeEnum.SUCCESS, GetLanguageCode());
            res.Data = new { UserId = userId };

            return res;
        }

        #region
        private string GetLanguageCode()
        {
            if (HttpContext.Request.Cookies.TryGetValue("LanguageCode", out string langCode))
            {
                return langCode;
            }
            return _configuration["MyCustomSettings:DefaultLanguageCode"] ?? "vi";
        }
        #endregion
    }
}
