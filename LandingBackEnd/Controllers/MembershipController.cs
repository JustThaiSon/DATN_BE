using AutoMapper;
using DATN_Helpers.Common.interfaces;
using DATN_LandingPage.Extension;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Models.DAO.Interface;
using Microsoft.AspNetCore.Mvc;
using DATN_Helpers.Common;
using DATN_Helpers.Extensions;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.Membership.Req;
using DATN_Models.DTOS.Membership.Res;
using DATN_Services.Service.Interfaces;

namespace DATN_LandingPage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly IVNPayService _vnPayService;
        private readonly IMembershipDAO _membershipDAO;
        private readonly IMailService _mailService;


        public MembershipController(IConfiguration configuration, IUltil ultils, IMapper mapper, IVNPayService vnPayService, IMembershipDAO membershipDAO, IMailService mailService)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _vnPayService = vnPayService;
            _membershipDAO = membershipDAO;
            _mailService = mailService;
        }
        [BAuthorize]
        [HttpPost]
        [Route("AddUserMembership")]
        public async Task<CommonResponse<string>> AddUserMembership(AddUserMembershipReq req)
        {
            var res = new CommonResponse<string>();
            var userId = HttpContextHelper.GetUserId();
            _membershipDAO.AddUserMembership(userId, req, out int responseCode);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            return res;
        }

        [BAuthorize]
        [HttpPost]
        [Route("CheckMembership")]
        public async Task<CommonResponse<CheckMemberShipRes>> CheckMembership()
        {
            var res = new CommonResponse<CheckMemberShipRes>();
            var userId = HttpContextHelper.GetUserId();
            var result = _membershipDAO.CheckMembership(userId, out int responseCode);
            var resultMapper = _mapper.Map<CheckMemberShipRes>(result);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            return res;
        }
        [BAuthorize]
        [HttpGet]
        [Route("MembershipPreview")]
        public async Task<CommonResponse<MembershipPreviewRes>> MembershipPreview(long orderPrice,long ticketPrice )
        {
            var res = new CommonResponse<MembershipPreviewRes>();
            var userId = HttpContextHelper.GetUserId();
            var result = _membershipDAO.MembershipPreview(userId, orderPrice, ticketPrice, out int responseCode);
            var resultMapper = _mapper.Map<MembershipPreviewRes>(result);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            return res;
        }
        [BAuthorize]
        [HttpGet]
        [Route("GetPointByUser")]
        public async Task<CommonResponse<GetPointByUserRes>> GetPointByUser()
        {
            var res = new CommonResponse<GetPointByUserRes>();
            var userId = HttpContextHelper.GetUserId();
            var result = _membershipDAO.GetPointByUser(userId,  out int responseCode);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = result;
            return res;
        }
        [BAuthorize]
        [HttpGet]
        [Route("GetmembershipByUserRes")]
        public async Task<CommonResponse<GetmembershipByUserRes>> GetmembershipByUser()
        {
            var res = new CommonResponse<GetmembershipByUserRes>();
            var userId = HttpContextHelper.GetUserId();
            var result = _membershipDAO.GetmembershipByUser(userId, out int responseCode);
            var resultMapper = _mapper.Map<GetmembershipByUserRes>(result);
            resultMapper.UserMembershipDetails.MemberCodeBase64 =  _mailService.GenerateQrCode(resultMapper.UserMembershipDetails.MemberCode);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            return res;
        }
        [BAuthorize]
        [HttpGet]
        [Route("GetPointHistory")]
        public async Task<CommonResponse<List<GetPointHistoryRes>>> GetPointHistory(int type,int currentPage, int recordPerPage)
        {
            var res = new CommonResponse<List<GetPointHistoryRes>>();
            var userId = HttpContextHelper.GetUserId();
            var result = _membershipDAO.GetPointHistory(userId, type, currentPage, recordPerPage, out int totalRecord, out int response);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.Data = result;
            return res;
        }
    }
}
