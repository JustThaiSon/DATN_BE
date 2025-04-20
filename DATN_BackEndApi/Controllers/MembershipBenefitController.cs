﻿using AutoMapper;
using DATN_Helpers.Common;
using DATN_Models.DAO.Interface;
using DATN_Models.DAL.MembershipBenefit;
using DATN_Models.DTOS.MembershipBenefit.Req;
using DATN_Models.DTOS.MembershipBenefit.Res;
using Microsoft.AspNetCore.Mvc;
using DATN_Helpers.Extensions;
using DATN_BackEndApi.Extension.CloudinarySett;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipBenefitController : ControllerBase
    {
        private readonly IMembershipBenefitDAO _membershipBenefitDAO;
        private readonly IMapper _mapper;
        private readonly string _langCode;
        private readonly CloudService _cloudService;


        public MembershipBenefitController(IMembershipBenefitDAO membershipBenefitDAO, IMapper mapper, CloudService cloudService)
        {
            _membershipBenefitDAO = membershipBenefitDAO;
            _mapper = mapper;
            _langCode = "vi";
            _cloudService = cloudService;
        }

        [HttpGet]
        [Route("GetAll")]
        public CommonResponse<List<MembershipBenefitRes>> GetAll()
        {
            var res = new CommonResponse<List<MembershipBenefitRes>>();
            var result = _membershipBenefitDAO.GetAllMembershipBenefits(out int response);
            var resultMapper = _mapper.Map<List<MembershipBenefitRes>>(result);
            res.Data = resultMapper;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            return res;
        }

        [HttpGet]
        [Route("GetByMembershipId/{membershipId}")]
        public CommonResponse<List<MembershipBenefitRes>> GetByMembershipId(long membershipId)
        {
            var res = new CommonResponse<List<MembershipBenefitRes>>();
            var result = _membershipBenefitDAO.GetMembershipBenefitsByMembershipId(membershipId, out int response);
            var resultMapper = _mapper.Map<List<MembershipBenefitRes>>(result);
            res.Data = resultMapper;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            return res;
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public CommonResponse<MembershipBenefitRes> GetById(long id)
        {
            var res = new CommonResponse<MembershipBenefitRes>();
            var result = _membershipBenefitDAO.GetMembershipBenefitById(id, out int response);
            var resultMapper = _mapper.Map<MembershipBenefitRes>(result);
            res.Data = resultMapper;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            return res;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<CommonResponse<string>> Create(IFormFile logo, [FromForm] MembershipBenefitReq req)
        {
            var res = new CommonResponse<string>();
            var reqMapper = _mapper.Map<MembershipBenefitDAL>(req);

            if (logo != null)
            {
                reqMapper.LogoUrl = await _cloudService.UploadImageAsync(logo).ConfigureAwait(false);
            }
            else
            {
                // Sử dụng URL mặc định nếu không có logo
                reqMapper.LogoUrl = "https://amc-theatres-res.cloudinary.com/image/upload/c_limit,w_45/f_auto/q_auto/v1729012832/amc-cdn/content/general/xl0f0ce6j8wuctienp9y.png";
            }

            _membershipBenefitDAO.CreateMembershipBenefit(reqMapper, out int response);
            res.Data = null;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            return res;
        }

        [HttpPost]
        [Route("Update/{id}")]
        public async Task<CommonResponse<string>> Update(long id, IFormFile logo, [FromForm] UpdateMembershipBenefitReq req)
        {
            var res = new CommonResponse<string>();
            req.Id = id;
            var reqMapper = _mapper.Map<MembershipBenefitDAL>(req);

            // Lấy thông tin hiện tại để giữ lại LogoUrl nếu không có logo mới
            var currentBenefit = _membershipBenefitDAO.GetMembershipBenefitById(id, out int getResponse);
            if (getResponse != 200)
            {
                res.ResponseCode = getResponse;
                res.Message = MessageUtils.GetMessage(getResponse, _langCode);
                return res;
            }

            if (logo != null)
            {
                reqMapper.LogoUrl = await _cloudService.UploadImageAsync(logo).ConfigureAwait(false);
            }
            else
            {
                // Giữ lại LogoUrl hiện tại nếu không có logo mới
                reqMapper.LogoUrl = currentBenefit.LogoUrl;
            }

            _membershipBenefitDAO.UpdateMembershipBenefit(reqMapper, out int response);
            res.Data = null;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            return res;
        }

        [HttpPost]
        [Route("Delete/{id}")]
        public CommonResponse<string> Delete(long id)
        {
            var res = new CommonResponse<string>();
            _membershipBenefitDAO.DeleteMembershipBenefit(id, out int response);
            res.Data = null;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            return res;
        }
    }
}
