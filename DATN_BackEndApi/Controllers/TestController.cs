using DATN_BackEndApi.Extension;
using DATN_Helpers.Common;
using DATN_Helpers.Constants;
using DATN_Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

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
            var í = GetUserId();
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

        #region Define
        private Guid GetUserId()
        {
            return (Guid)(HttpContext.Items["UserId"] ?? 0);
        }
        #endregion
    }
}
