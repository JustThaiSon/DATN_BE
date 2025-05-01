using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_LandingPage.Extension;
using DATN_Models.DAL.Movie.Actor;
using DATN_Models.DAL.Rating;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Comments.Req;
using DATN_Models.DTOS.Comments.Res;
using DATN_Models.DTOS.Order.Res;
using DATN_Models.DTOS.Rating.Req;
using DATN_Models.DTOS.Rating.Res;
using DATN_Services.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        //private readonly CloudService _cloudService;
        private readonly ICommentDAO _commentDAO;
        private readonly IOrderDAO _orderDAO;
        private readonly IMailService _mailService;
        private readonly string _langCode;
        private readonly IMapper _mapper;
        private readonly IRatingDAO _ratingDAO;
        private readonly IUltil _ultils;

        public TestController(
            ICommentDAO commentDAO,
            IConfiguration configuration,
            IUltil ultils,
            IMapper mapper,
            IRatingDAO ratingDAO,
            IOrderDAO orderDAO,
            IMailService mailService)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _commentDAO = commentDAO;
            _ratingDAO = ratingDAO;
            _orderDAO = orderDAO;
            _mailService = mailService;
        }

        #region Lấy thông tin người dùng đang đăng nhập hiện tại
        private async Task<Guid> GetUserId() { return (Guid)(HttpContext.Items["UserId"] ?? 0); }
        #endregion


        [HttpPost]
        [Route("userinfo_test_nghia")]
        //[BAuthorize]
        public async Task<CommonResponse<dynamic>> userinfo(CreateCommentReq req)
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

        [HttpGet]
        [Route("Test")]
        public async Task<CommonResponse<dynamic>> Test(Guid OrderId)
        {
            var res = new CommonResponse<dynamic>();
            var data = _orderDAO.GetOrderSendMail(OrderId, out int responseCode);
            var resultMapper = _mapper.Map<OrderMailResultRes>(data);
            if (resultMapper != null)
            {
                _mailService.SendQrCodeEmail(resultMapper);
            }
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            return res;
        }


    }
}
