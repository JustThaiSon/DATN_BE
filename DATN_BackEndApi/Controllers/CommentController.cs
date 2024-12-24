using AutoMapper;
using DATN_BackEndApi.Extension;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Movie.Actor;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Comments.Req;
using DATN_Models.DTOS.Comments.Res;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        //private readonly ImageService _imgService;
        private readonly ICommentDAO _commentDAO;
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;

        public CommentController(ICommentDAO commentDAO, IConfiguration configuration, IUltil ultils, IMapper mapper)
        {


            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _commentDAO = commentDAO;
        }


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
            var userId = GetUserId();

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
        [BAuthorize]
        public async Task<CommonResponse<dynamic>> UpdateComment(UpdateCommentReq req)
        {
            var userId = GetUserId();

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
        [BAuthorize]
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
        public async Task<CommonPagination<List<GetListCommentRes>>> GetMovieList(Guid movieId, int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetListCommentRes>>();
            var result = _commentDAO.GetListComment(movieId, currentPage, recordPerPage, out int TotalRecord, out int response);
            var resultMapper = _mapper.Map<List<GetListCommentRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = TotalRecord;

            return res;
        }






        #region Lấy thông tin người dùng đang đăng nhập hiện tại

        private Guid GetUserId()
        {
            return (Guid)(HttpContext.Items["UserId"] ?? 0);
        }
        private List<string> GetRoles()
        {
            if (HttpContext.Items["Roles"] is List<string> roles)
            {
                return roles;
            }
            return new List<string>();
        }

        #endregion
    }
}
