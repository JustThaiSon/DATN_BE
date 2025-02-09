using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_LandingPage.Extension;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.Seat.Res;
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
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        public MovieController(IMovieDAO movieDAO, IConfiguration configuration, IUltil ultils, IMapper mapper, IMailService mailService)
        {
            _movieDAO = movieDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _mailService = mailService;
        }
        [HttpGet]
        [Route("GetMovie")]
        public async Task<CommonPagination<List<GetMovieLandingRes>>> GetMovie(int type, int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetMovieLandingRes>>();
            var data = _movieDAO.GetMovieLanding(type,currentPage,recordPerPage,out int totalRecord,out int responseCode);
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
        public async Task<CommonPagination<List<GetShowTimeLangdingRes>>> GetShowTimeLanding(string location, DateTime date, int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetShowTimeLangdingRes>>();
            var data = _movieDAO.GetShowTimeLanding(location, date, currentPage, recordPerPage,out int totalRecord, out int responseCode);
            var resultMapper = _mapper.Map<List<GetShowTimeLangdingRes>>(data);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            res.TotalRecord = totalRecord;
            return res;
        }
        //[HttpGet]
        //[Route("GetAllSeatByShowTime")]
        //public async Task<CommonPagination<List<GetListSeatByShowTimeRes>>> GetAllSeatByShowTime(Guid showTimeId, int currentPage, int recordPerPage)
        //{

        //    var res = new CommonPagination<List<GetListSeatByShowTimeRes>>();

        //    var result = _seatDAO.GetListSeatByShowTime(roomId, showTimeId, currentPage, recordPerPage, out int TotalRecord, out int response);

        //    var resultMapper = _mapper.Map<List<GetListSeatByShowTimeRes>>(result);

        //    res.Data = resultMapper;
        //    res.Message = MessageUtils.GetMessage(response, _langCode);
        //    res.ResponseCode = response;
        //    res.TotalRecord = TotalRecord;

        //    return res;
        //}

        [HttpGet("GetTest")]
        public async Task<CommonResponse<string>> GetTest()
        {
            var res = new CommonResponse<string>();
            res.Data = "Test";
            res.Message = "Success";
            res.ResponseCode = 200;
            await _mailService.SendQrCodeEmail("hoangthaisonqs@gmail.com", "Nụ hôn bạc tỉ", "ABCD");
            return res;
        }   
    }
}
