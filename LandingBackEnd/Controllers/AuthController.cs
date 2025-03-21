using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Constants;
using DATN_Helpers.Extensions;
using DATN_Helpers.Extensions.Global;
using DATN_Models.DAL.Account;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Account.Req;
using DATN_Models.DTOS.Account.Res;
using DATN_Services.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DATN_LandingPage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginDAO _loginDAO;
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        public AuthController(ILoginDAO loginDAO, IConfiguration configuration, IUltil ultils, IMapper mapper, IMailService mailService)
        {
            _loginDAO = loginDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _mailService = mailService;
        }
        [HttpPost]
        [Route("Resgister")]
        public async Task<CommonResponse<dynamic>> Resgister(CreateAccountReq request)
        {
            var res = new CommonResponse<dynamic>();
            var (response, Opt) = await _loginDAO.RegisterUserAsync(request);
            if (response == (int)ResponseCodeEnum.SUCCESS)
            {
                await _mailService.SendMail(request.Email, MailHelper.SubjectOPT, MailHelper.BodyOPT + Opt);
            }
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.Data = null;
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
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<CommonResponse<dynamic>> RefreshToken(RefreshTokenReq req)
        {
            var res = new CommonResponse<dynamic>();

            var newAccessToken = _ultils.GenerateTokenFromRefreshToken(req.RefreshToken);
            if (string.IsNullOrEmpty(newAccessToken))
            {
                res.ResponseCode = (int)ResponseCodeEnum.ERR_TOKEN_INVALID;
                res.Message = MessageUtils.GetMessage(res.ResponseCode, _langCode);
                return res;
            }

            var (userId, roles) = _ultils.ValidateToken(newAccessToken);
            if (userId == null || roles == null)
            {
                res.ResponseCode = (int)ResponseCodeEnum.ERR_TOKEN_INVALID;
                res.Message = MessageUtils.GetMessage(res.ResponseCode, _langCode);
                return res;
            }

            var newRefreshToken = _ultils.GenerateRefreshToken(userId.Value, roles);

            LoginRes loginRes = new LoginRes
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Roles = roles,
            };

            res.ResponseCode = (int)ResponseCodeEnum.SUCCESS;
            res.Message = MessageUtils.GetMessage(res.ResponseCode, _langCode);
            res.Data = loginRes;
            return res;
        }

    }
}
public class RefreshTokenReq
{
    public string RefreshToken { get; set; }
}

