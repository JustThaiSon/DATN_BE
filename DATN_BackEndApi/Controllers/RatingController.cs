using AutoMapper;
using DATN_BackEndApi.Extension;
using DATN_BackEndApi.Extension.CloudinarySett;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Rating;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Movies.Req;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.Rating.Req;
using DATN_Models.DTOS.Rating.Res;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;

        private readonly IRatingDAO _ratingDAO;

        public RatingController(IConfiguration configuration, IUltil ultils, IMapper mapper, IRatingDAO ratingDAO)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _ratingDAO = ratingDAO;
        }


        #region Rating_nghia
        [HttpGet]
        [Route("GetRatingList")]
        public async Task<CommonPagination<List<GetListRatingRes>>> GetRatingList(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetListRatingRes>>();
            var result = _ratingDAO.GetListRating(currentPage, recordPerPage, out int TotalRecord, out int response);
            var resultMapper = _mapper.Map<List<GetListRatingRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = TotalRecord;

            return res;
        }


        [HttpPost]
        [Route("CreateRating")]
        [BAuthorize]
        public async Task<CommonResponse<dynamic>> CreateRating(AddRatingReq req)
        {
            var res = new CommonResponse<dynamic>();
            var reqMapper = _mapper.Map<AddRatingDAL>(req);
            reqMapper.UserId = await GetUserId();
            _ratingDAO.CreateRating(reqMapper, out int response);

            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }



        [HttpPost]
        [Route("UpdateRating")]
        //[BAuthorize]
        public async Task<CommonResponse<dynamic>> UpdateRating(UpdateRatingReq req)
        {
            var res = new CommonResponse<dynamic>();

            var reqMapper = _mapper.Map<UpdateRatingDAL>(req);
            _ratingDAO.UpdateRating(reqMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }


        [HttpPost]
        [Route("DeleteRating")]
        //[BAuthorize]
        public async Task<CommonResponse<dynamic>> DeleteRating(Guid id)
        {
            var res = new CommonResponse<dynamic>();
            _ratingDAO.DeleteRating(id, out int response);

            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpPost]
        [Route("HideRating")]
        //[BAuthorize]
        public async Task<CommonResponse<dynamic>> HideRating(Guid id)
        {
            var res = new CommonResponse<dynamic>();
            _ratingDAO.HideRating(id, out int response);

            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }


        //[HttpPost]
        //[Route("TestMovie")]
        //public async Task<CommonResponse<dynamic>> TestMovie([FromBody] ActorReq rq, [FromQuery] params Guid[] ActorIDs)
        //{
        //    var res = new CommonResponse<dynamic>();
        //    _movieDAO.CreateActor(rq, out int response);
        //    res.Data = null;
        //    res.Message = MessageUtils.GetMessage(response, _langCode);
        //    res.ResponseCode = response;
        //    return res;
        //}


        #endregion


        #region Lấy thông tin người dùng đang đăng nhập hiện tại
        private async Task<Guid> GetUserId()
        {
            return (Guid)(HttpContext.Items["UserId"] ?? 0);
        }
        private List<string> GetRoles()
        {
            if (HttpContext.Items["Roles"] is List<string> roles)
            {
                return roles;  // Return the list of roles
            }

            return new List<string>();
        }
        #endregion
    }
}
