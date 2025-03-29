using AutoMapper;
using DATN_BackEndApi.VNPay;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Orders;
using DATN_Models.DAL.Service;
using DATN_Models.DAL.ServiceType;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.Order.Req;
using DATN_Models.DTOS.Seat.Res;
using DATN_Models.DTOS.SeatType.Res;
using DATN_Models.DTOS.Service.Response;
using DATN_Models.DTOS.ServiceType.Res;
using DATN_Services.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DATN_LandingPage.Controllers
{
    //[BAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieDAO _movieDAO;
        private readonly ISeatDAO _seatDAO;
        private readonly IServiceDAO _serviceDAO;
        private readonly IServiceTypeDAO _serviceTypeDAO;
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IOrderDAO _orderDAO;
        private readonly IVNPayService _vnPayService;

        public MovieController(IMovieDAO movieDAO, IConfiguration configuration, IUltil ultils, IMapper mapper, IMailService mailService, ISeatDAO seatDAO, IServiceDAO serviceDAO, IOrderDAO orderDAO, IVNPayService vnPayService, IServiceTypeDAO seatTypeDAO)
        {
            _movieDAO = movieDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _mailService = mailService;
            _seatDAO = seatDAO;
            _serviceDAO = serviceDAO;
            _orderDAO = orderDAO;
            _vnPayService = vnPayService;
            _serviceTypeDAO = seatTypeDAO;
        }
        [HttpGet]
        [Route("GetMovie")]
        public async Task<CommonPagination<List<GetMovieLandingRes>>> GetMovie(int type, int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetMovieLandingRes>>();
            var data = _movieDAO.GetMovieLanding(type, currentPage, recordPerPage, out int totalRecord, out int responseCode);
            var resultMapper = _mapper.Map<List<GetMovieLandingRes>>(data);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            res.TotalRecord = totalRecord;
            return res;
        }
        [HttpGet]
        [Route("GetDetailMovie")]
        public async Task<CommonResponse<GetMovieRes>> GetDetailMovie(Guid movieId)
        {
            var res = new CommonResponse<GetMovieRes>();
            var data = _movieDAO.GetMovieDetail(movieId, out int responseCode);
            var resultMapper = _mapper.Map<GetMovieRes>(data);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            return res;
        }
        [HttpGet]
        [Route("GetShowTimeLanding")]
        public async Task<CommonPagination<List<GetShowTimeLangdingRes>>> GetShowTimeLanding(Guid? movieId, string? location, DateTime? date, int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetShowTimeLangdingRes>>();
            var data = _movieDAO.GetShowTimeLanding(movieId, location, date, currentPage, recordPerPage, out int totalRecord, out int responseCode);
            var resultMapper = _mapper.Map<List<GetShowTimeLangdingRes>>(data);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            res.TotalRecord = totalRecord;
            return res;
        }
        [HttpGet]
        [Route("GetService")]
        public async Task<CommonPagination<List<GetServiceRes>>> GetService(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetServiceRes>>();
            var result = _serviceDAO.GetService(currentPage, recordPerPage, out int totalRecord, out int response);
            var resultMapper = _mapper.Map<List<GetServiceRes>>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;
            return res;
        }
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<CommonResponse<string>> CreateOrder(CreateOrderReq req)
        {
            var userId = HttpContextHelper.GetUserId();
            var res = new CommonResponse<string>();
            var reqpMapper = _mapper.Map<CreateOrderDAL>(req);
            var result = _orderDAO.CreateOrder(userId, reqpMapper, out int responseCode);
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.ResponseCode = responseCode;
            if (result != null)
            {
                await _mailService.SendQrCodeEmail(result);
            }
            return res;
        }
        [HttpGet]
        [Route("GetAllNameMovie")]
        public async Task<CommonResponse<List<GetAllNameMovieRes>>> GetAllNameMovie()
        {
            var res = new CommonResponse<List<GetAllNameMovieRes>>();
            var data = _movieDAO.GetAllNameMovie(out int responseCode);
            var resultMapper = _mapper.Map<List<GetAllNameMovieRes>>(data);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            return res;
        }
        [HttpGet]
        [Route("GetSeatByShowTime")]
        public async Task<CommonResponse<List<GetSeatByShowTimeRes>>> GetSeatByShowTime(Guid showTimeId)
        {
            var res = new CommonResponse<List<GetSeatByShowTimeRes>>();
            var data = _seatDAO.GetSeatByShowTime(showTimeId, out int totalRecord, out int responseCode);
            var resultMapper = _mapper.Map<List<GetSeatByShowTimeRes>>(data);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            return res;
        }


        [HttpGet("GetMovieGenres")]
        public async Task<CommonResponse<List<MovieGenreRes>>> GetMovieGenres(Guid id)
        {
            var res = new CommonResponse<List<MovieGenreRes>>();

            var result = _movieDAO.GetMovieGenres(id, out int response);
            var resultMapper = _mapper.Map<List<MovieGenreRes>>(result);

            res.Data = resultMapper;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }
        [HttpGet]
        [Route("GetServiceTypeList")]
        public async Task<CommonPagination<List<ServiceTypeRes>>> GetServiceTypeList(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<ServiceTypeRes>>();
            var result = _serviceTypeDAO.GetServiceTypeList(currentPage, recordPerPage, out int totalRecord, out int response);
            var resultMapper = _mapper.Map<List<ServiceTypeRes>>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;
            return res;
        }

        [HttpGet]
        [Route("GetMovieByShowTime")]
        public async Task<CommonResponse<GetMovieByShowTimeRes>> GetMovieByShowTime(Guid showtimeId)
        {
            var res = new CommonResponse<GetMovieByShowTimeRes>();
            var result = _movieDAO.GetMovieByShowTime(showtimeId, out int response);
            var resultMapper = _mapper.Map<GetMovieByShowTimeRes>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost]
        [Route("create-payment")]
        public async Task<CommonResponse<string>> CreatePayment([FromBody] OrderInfo orderInfo)
        {
            var res = new CommonResponse<string>();
            try
            {
                var paymentUrl = _vnPayService.CreatePaymentUrl(orderInfo, HttpContext);
                res.Data = paymentUrl;
                res.ResponseCode = 1;
                res.Message = "Success";
            }
            catch (Exception ex)
            {
                res.ResponseCode = -99;
                res.Message = ex.Message;
            }
            return res;
        }







        [HttpGet]
        [Route("payment-callback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.ProcessPaymentCallback(Request.Query);
            // Redirect back to Angular with payment result
            var redirectUrl = "http://localhost:4200/payment-callback";
            var queryString = $"?vnp_ResponseCode={response.VnPayResponseCode}&vnp_TxnRef={response.OrderId}";
            return Redirect(redirectUrl + queryString);
        }



    }
}
