//using AutoMapper;
//using DATN_BackEndApi.Extension;
//using DATN_BackEndApi.Extension.CloudinarySett;
//using DATN_Helpers.Common;
//using DATN_Helpers.Common.interfaces;
//using DATN_Helpers.Extensions;
//using DATN_Models.DAO.Interface;
//using DATN_Models.DTOS.Movies.Res;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace DATN_BackEndApi.Controllers
//{
//    [BAuthorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CommentController : ControllerBase
//    {
//        //private readonly ImageService _imgService;
//        private readonly string _langCode;
//        private readonly IUltil _ultils;
//        private readonly IMapper _mapper;

//        public CommentController(IConfiguration configuration, IUltil ultils, IMapper mapper)
//        {


//            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
//            _ultils = ultils;
//            _mapper = mapper;
//        }

//        /* Khách hàng chỉ có thể comment được trên những phim tương ứng với vé họ mua */


//        /* 
//          - Bắt buộc đăng nhập mới dùng được [bauthorize] ?
//          - Khách hàng chỉ có thể đánh giá sau khi kết thúc phim
//          - bắt buộc phải XEM PHIM mới có thể đánh giá.
//         */
//        [HttpPost]
//        [Route("AddComment")]
//        public async Task<CommonResponse<GetMovieRes>> AddComment(Guid Id)
//        {

//            var userId = GetUserId();

//            var res = new CommonResponse<GetMovieRes>();
//            var result = _movieDAO.GetMovieDetail(Id, out int response);

//            var resultMapper = _mapper.Map<GetMovieRes>(result);

//            res.Data = resultMapper;
//            res.Message = MessageUtils.GetMessage(response, _langCode);
//            res.ResponseCode = response;

//            return res;
//        }


//        #region Lấy thông tin người dùng đang đăng nhập hiện tại

//        private Guid GetUserId()
//        {
//            return (Guid)(HttpContext.Items["UserId"] ?? 0);
//        }
//        private List<string> GetRoles()
//        {
//            if (HttpContext.Items["Roles"] is List<string> roles)
//            {
//                return roles;
//            }
//            return new List<string>();
//        }

//        #endregion
//    }
//}
