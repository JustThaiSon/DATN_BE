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

            try
            {
                // Validate: Logo là bắt buộc khi tạo mới
                if (logo == null)
                {
                    res.ResponseCode = 400;
                    res.Message = "Vui lòng chọn logo cho quyền lợi";
                    return res;
                }

                var reqMapper = _mapper.Map<MembershipBenefitDAL>(req);

                // Upload logo lên cloud
                reqMapper.LogoUrl = await _cloudService.UploadImageAsync(logo).ConfigureAwait(false);

                _membershipBenefitDAO.CreateMembershipBenefit(reqMapper, out int response);
                res.Data = null;
                res.ResponseCode = response;
                res.Message = MessageUtils.GetMessage(response, _langCode);
            }
            catch (Exception ex)
            {
                res.ResponseCode = 500;
                res.Message = $"Lỗi: {ex.Message}";
            }

            return res;
        }

        [HttpPost]
        [Route("Update/{id}")]
        public async Task<CommonResponse<string>> Update(long id, IFormFile logo, [FromForm] UpdateMembershipBenefitReq req)
        {
            var res = new CommonResponse<string>();

            try
            {
                // Lấy thông tin hiện tại để giữ lại LogoUrl nếu không có logo mới
                var currentBenefit = _membershipBenefitDAO.GetMembershipBenefitById(id, out int getResponse);
                if (getResponse != 200)
                {
                    res.ResponseCode = getResponse;
                    res.Message = MessageUtils.GetMessage(getResponse, _langCode);
                    return res;
                }

                // Gán ID vào request
                req.Id = id;

                // Tạo một đối tượng MembershipBenefitDAL mới thay vì sử dụng mapper
                var membershipBenefit = new MembershipBenefitDAL
                {
                    Id = id,
                    MembershipId = req.MembershipId,
                    BenefitType = req.BenefitType,
                    Description = req.Description,
                    // Sử dụng LogoUrl hiện tại ngay từ đầu
                    LogoUrl = currentBenefit.LogoUrl
                };

                // Tạo ConfigJson dựa trên BenefitType
                switch (req.BenefitType)
                {
                    case "Discount":
                        membershipBenefit.ConfigJson = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Target = req.Target ?? string.Empty,
                            Value = req.Value ?? 0
                        });
                        break;
                    case "PointBonus":
                        membershipBenefit.ConfigJson = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Multiplier = req.Multiplier ?? 0
                        });
                        break;
                    case "Service":
                        membershipBenefit.ConfigJson = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            ServiceId = req.ServiceId ?? Guid.Empty,
                            Quantity = req.Quantity ?? 0,
                            Limit = req.Limit ?? 0
                        });
                        break;
                    case "UsePoint":
                        membershipBenefit.ConfigJson = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            UsePoint = req.UsePointValue ?? 0
                        });
                        break;
                    default:
                        membershipBenefit.ConfigJson = "{}";
                        break;
                }

                // Xử lý logo chỉ khi có logo mới
                if (logo != null)
                {
                    try
                    {
                        // Nếu có logo mới, upload lên cloud
                        membershipBenefit.LogoUrl = await _cloudService.UploadImageAsync(logo).ConfigureAwait(false);
                        Console.WriteLine($"Uploaded new logo: {membershipBenefit.LogoUrl}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error uploading logo: {ex.Message}");
                        // Nếu có lỗi khi upload, vẫn giữ logo cũ
                    }
                }
                else
                {
                    Console.WriteLine($"Using existing logo: {membershipBenefit.LogoUrl}");
                }

                // Gọi DAO để cập nhật
                _membershipBenefitDAO.UpdateMembershipBenefit(membershipBenefit, out int response);

                // Kiểm tra response
                if (response != 200)
                {
                    res.ResponseCode = response;
                    res.Message = MessageUtils.GetMessage(response, _langCode);
                    return res;
                }

                res.Data = null;
                res.ResponseCode = 200;
                res.Message = MessageUtils.GetMessage(200, _langCode);
            }
            catch (Exception ex)
            {
                res.ResponseCode = 500;
                res.Message = $"Lỗi: {ex.Message}";
            }

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
