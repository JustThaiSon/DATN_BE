using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_LandingPage.Extension;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Movies.Res;
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
        public MovieController(IMovieDAO movieDAO, IConfiguration configuration, IUltil ultils, IMapper mapper)
        {
            _movieDAO = movieDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
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
    }
}
