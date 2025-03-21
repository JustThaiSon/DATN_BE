using AutoMapper;
using Azure;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_LandingPage.Extension;
using DATN_Models.DAL.Movie.Actor;
using DATN_Models.DAL.Orders;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Models.DTOS.Comments.Req;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.Order.Req;
using DATN_Models.DTOS.Seat.Res;
using DATN_Models.DTOS.Service.Response;
using DATN_Services.Service.Interfaces;
using Microsoft.AspNetCore.Http;
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
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IOrderDAO _orderDAO;
        public MovieController(IMovieDAO movieDAO, IConfiguration configuration, IUltil ultils, IMapper mapper, IMailService mailService, ISeatDAO seatDAO, IServiceDAO serviceDAO, IOrderDAO orderDAO)
        {
            _movieDAO = movieDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _mailService = mailService;
            _seatDAO = seatDAO;
            _serviceDAO = serviceDAO;
            _orderDAO = orderDAO;
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
        public async Task<CommonResponse<GetDetailMovieLangdingRes>> GetDetailMovie(Guid movieId)
        {
            var res = new CommonResponse<GetDetailMovieLangdingRes>();
            var data = _movieDAO.GetDetailMovieLangding(movieId, out int responseCode);
            var resultMapper = _mapper.Map<GetDetailMovieLangdingRes>(data);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            return res;
        }
        [HttpGet]
        [Route("GetShowTimeLanding")]
        public async Task<CommonPagination<List<GetShowTimeLangdingRes>>> GetShowTimeLanding(Guid? movieId,string? location, DateTime? date, int currentPage, int recordPerPage)
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
    }
}
