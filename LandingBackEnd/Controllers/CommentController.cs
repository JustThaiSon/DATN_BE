using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_LandingPage.Extension;
using DATN_Models.DAL.Movie.Actor;
using DATN_Models.DAL.Rating;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Comments.Req;
using DATN_Models.DTOS.Comments.Res;
using DATN_Models.DTOS.Rating.Req;
using DATN_Models.DTOS.Rating.Res;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        //private readonly CloudService _cloudService;
        private readonly ICommentDAO _commentDAO;


        private readonly string _langCode;
        private readonly IMapper _mapper;
        private readonly IRatingDAO _ratingDAO;
        private readonly IUltil _ultils;

        public CommentController(
            ICommentDAO commentDAO,
            IConfiguration configuration,
            IUltil ultils,
            IMapper mapper,
            IRatingDAO ratingDAO)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _commentDAO = commentDAO;
            _ratingDAO = ratingDAO;
        }

        #region Lấy thông tin người dùng đang đăng nhập hiện tại
        private async Task<Guid> GetUserId() { return (Guid)(HttpContext.Items["UserId"] ?? 0); }
        #endregion


        #region Comment_Nghia

        /*
          - Bắt buộc đăng nhập mới dùng được [bauthorize] ?
          - Khách hàng chỉ có thể đánh giá sau khi kết thúc phim
          - bắt buộc phải XEM PHIM mới có thể đánh giá.
         */
        [HttpPost]
        [Route("AddComment")]
        [BAuthorize]
        public async Task<CommonResponse<dynamic>> AddComment(CreateCommentReq req)
        {
            var userId = await GetUserId();

            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<CreateCommentDAL>(req);

            _commentDAO.CreateComment(userId, resultMapper, out int response);

            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.Data = null;

            return res;
        }

        [HttpPost]
        [Route("UpdateComment")]
        //[BAuthorize]
        public async Task<CommonResponse<dynamic>> UpdateComment(UpdateCommentReq req)
        {
            var userId = await GetUserId();

            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<UpdateCommentDAL>(req);

            _commentDAO.UpdateComment(userId, resultMapper, out int response);

            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.Data = null;

            return res;
        }

        [HttpPost]
        [Route("DeleteComment")]
        //[BAuthorize]
        public async Task<CommonResponse<dynamic>> DeleteComment(Guid id)
        {
            var res = new CommonResponse<dynamic>();

            _commentDAO.DeleteComment(id, out int response);

            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.Data = null;

            return res;
        }


        [HttpGet]
        [Route("GetCommentList")]
        public async Task<CommonPagination<List<GetListCommentRes>>> GetMovieList(
            Guid movieId,
            int currentPage,
            int recordPerPage)
        {
            var res = new CommonPagination<List<GetListCommentRes>>();
            var result = _commentDAO.GetListComment(
                movieId,
                currentPage,
                recordPerPage,
                out int TotalRecord,
                out int response);
            var resultMapper = _mapper.Map<List<GetListCommentRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = TotalRecord;

            return res;
        }
        #endregion

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
            reqMapper.UserId = await GetUserId().ConfigureAwait(false);

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
        #endregion
    }
}
