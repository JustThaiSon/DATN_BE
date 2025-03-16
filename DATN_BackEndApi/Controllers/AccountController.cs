using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Constants;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Account;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Account.Req;
using DATN_Models.DTOS.Account.Res;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILoginDAO _loginDAO;
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        public AccountController(ILoginDAO loginDAO, IConfiguration configuration, IUltil ultils, IMapper mapper)
        {
            _loginDAO = loginDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Resgister")]
        public async Task<CommonResponse<dynamic>> Resgister(CreateAccountReq request)
        {
            var res = new CommonResponse<dynamic>();
            var (response, Opt) = await _loginDAO.RegisterUserAsync(request);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.Data = Opt;
            return res;
        }




        [HttpPost]
        [Route("Login")]
        public async Task<CommonResponse<dynamic>> Login(SignInReq req)
        {
            var res = new CommonResponse<dynamic>();
            var reqMapper = _mapper.Map<SignInDAL>(req);
            var (loginDto, responseCode) = await _loginDAO.login(reqMapper);
            if (responseCode != 200)
            {
                res.ResponseCode = (int)ResponseCodeEnum.ERR_SYSTEM;
                res.Message = MessageUtils.GetMessage(res.ResponseCode, _langCode);
                return res;
            }
            LoginRes loginCms = new LoginRes
            {
                AccessToken = _ultils.GenerateToken(loginDto.ID, loginDto.Roles),
                RefreshToken = _ultils.GenerateRefreshToken(loginDto.ID, loginDto.Roles),
                Roles = loginDto.Roles,

                UserId = loginDto.ID.ToString(),
                UserName = loginDto.UserName,
                DisplayName = loginDto.DisplayName
            };
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = loginCms;
            return res;
        }



        [HttpPost("verify-otp")]
        public async Task<CommonResponse<dynamic>> VerifyOtp(VerifyOtpReq req)
        {
            var result = await _loginDAO.VerifyOtpAndRegisterUserAsync(req);
            var res = new CommonResponse<dynamic>
            {
                ResponseCode = result,
                Message = MessageUtils.GetMessage(result, _langCode)
            };
            return res;
        }

    }
}
