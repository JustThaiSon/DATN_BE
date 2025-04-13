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

        public MembershipController(IConfiguration configuration, IUltil ultils, IMapper mapper, IVNPayService vnPayService, IMembershipDAO membershipDAO)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _vnPayService = vnPayService;
            _membershipDAO = membershipDAO;
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
    }
}
