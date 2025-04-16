﻿using AutoMapper;
using DATN_BackEndApi.Extension;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.AgeRating;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.AgeRating.Req;
using DATN_Models.DTOS.AgeRating.Res;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgeRatingController : ControllerBase
    {
        private readonly IAgeRatingDAO _ageRatingDAO;
        private readonly IMapper _mapper;
        private readonly string _langCode;

        public AgeRatingController(IConfiguration configuration, IAgeRatingDAO ageRatingDAO, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _ageRatingDAO = ageRatingDAO;
            _mapper = mapper;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
        }

        [HttpGet("GetAgeRatings")]
        public async Task<CommonPagination<List<AgeRatingRes>>> GetAgeRatings(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<AgeRatingRes>>();

            var result = _ageRatingDAO.GetAgeRatings(currentPage, recordPerPage, out int totalRecord, out int response);
            var resultMapper = _mapper.Map<List<AgeRatingRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;

            return res;
        }

        [HttpGet("GetAgeRatingById")]
        public async Task<CommonResponse<AgeRatingRes>> GetAgeRatingById(Guid id)
        {
            var res = new CommonResponse<AgeRatingRes>();

            var result = _ageRatingDAO.GetAgeRatingById(id, out int response);
            var resultMapper = _mapper.Map<AgeRatingRes>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpPost("CreateAgeRating")]
        public async Task<CommonResponse<bool>> CreateAgeRating([FromBody] CreateAgeRatingReq request)
        {
            var res = new CommonResponse<bool>();

            var ageRatingDAL = new AgeRatingDAL
            {
                Code = request.Code,
                Description = request.Description,
                MinimumAge = request.MinimumAge,
            };

            _ageRatingDAO.CreateAgeRating(ageRatingDAL, out int response);

            res.Data = response == 200;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpPost("UpdateAgeRating")]
        public async Task<CommonResponse<bool>> UpdateAgeRating([FromBody] UpdateAgeRatingReq request)
        {
            var res = new CommonResponse<bool>();

            var ageRatingDAL = _mapper.Map<AgeRatingDAL>(request);
            _ageRatingDAO.UpdateAgeRating(ageRatingDAL, out int response);

            res.Data = response == 200;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpPost("DeleteAgeRating")]
        public async Task<CommonResponse<bool>> DeleteAgeRating(Guid id)
        {
            var res = new CommonResponse<bool>();

            _ageRatingDAO.DeleteAgeRating(id, out int response);

            res.Data = response == 200;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }
    }
}
