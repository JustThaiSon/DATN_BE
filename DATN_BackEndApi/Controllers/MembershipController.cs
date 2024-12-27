using AutoMapper;
using DATN_BackEndApi.Extension.CloudinarySett;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Membership;
using DATN_Models.DAL.Movie;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Movies.Req;
using DATN_Models.DTOS.Movies.Req.Movie;
using DATN_Models.DTOS.Movies.Res;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    //[BAuthorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipDAO _membershipDAO;

        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;

        public MembershipController(IMembershipDAO membershipDAO, IConfiguration configuration, IUltil ultils, IMapper mapper, ImageService imgService)
        {
            _membershipDAO = membershipDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
        }

        #region Membership_Nghia
        [HttpGet]
        [Route("GetMembershipList")]
        public async Task<CommonPagination<List<MembershipRes>>> GetMembershipList(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<MembershipRes>>();
            var result = _membershipDAO.GetListMembership(currentPage, recordPerPage, out int TotalRecord, out int response);
            var resultMapper = _mapper.Map<List<MembershipRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = TotalRecord;

            return res;
        }


        [HttpGet]
        [Route("GetMembershipDetail")]
        public async Task<CommonResponse<MembershipRes>> GetMembershipDetail(Guid Id)
        {
            var res = new CommonResponse<MembershipRes>();
            var result = _membershipDAO.GetMembershipDetail(Id, out int response);
            var resultMapper = _mapper.Map<MembershipRes>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpPost]
        [Route("CreateMembership")]
        public async Task<CommonResponse<dynamic>> CreateMembership(AddMembershipReq req)
        {
            var res = new CommonResponse<dynamic>();
            var reqMapper = _mapper.Map<AddMembershipDAL>(req);

            _membershipDAO.CreateMembership(reqMapper, out int response);

            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }


        [HttpPost]
        [Route("UpdateMembership")]
        public async Task<CommonResponse<dynamic>> UpdateMembership(UpdateMembershipReq req)
        {
            var res = new CommonResponse<dynamic>();
            var reqMapper = _mapper.Map<UpdateMembershipDAL>(req);
            _membershipDAO.UpdateMembership(reqMapper, out int response);

            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }


        [HttpPost]
        [Route("DeleteMembership")]
        public async Task<CommonResponse<dynamic>> DeleteMembership(Guid id)
        {
            var res = new CommonResponse<dynamic>();
            _membershipDAO.DeleteMembership(id, out int response);

            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }


        

        #endregion


    }
}
