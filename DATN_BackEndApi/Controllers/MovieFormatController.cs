using AutoMapper;
using DATN_BackEndApi.Extension;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.MovieFormat;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.MovieFormat.Req;
using DATN_Models.DTOS.MovieFormat.Res;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieFormatController : ControllerBase
    {
        private readonly IMovieFormatDAO _movieFormatDAO;
        private readonly IMapper _mapper;
        private readonly string _langCode;

        public MovieFormatController(IConfiguration configuration, IMovieFormatDAO movieFormatDAO, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _movieFormatDAO = movieFormatDAO;
            _mapper = mapper;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";

        }

        [HttpGet("GetMovieFormats")]
        public async Task<CommonPagination<List<MovieFormatRes>>> GetMovieFormats(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<MovieFormatRes>>();

            var result = _movieFormatDAO.GetMovieFormats(currentPage, recordPerPage, out int totalRecord, out int response);
            var resultMapper = _mapper.Map<List<MovieFormatRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;

            return res;
        }

        [HttpGet("GetMovieFormatById")]
        public async Task<CommonResponse<MovieFormatRes>> GetMovieFormatById(Guid id)
        {
            var res = new CommonResponse<MovieFormatRes>();

            var result = _movieFormatDAO.GetMovieFormatById(id, out int response);
            var resultMapper = _mapper.Map<MovieFormatRes>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpPost("CreateMovieFormat")]
        public async Task<CommonResponse<bool>> CreateMovieFormat([FromBody] CreateMovieFormatReq request)
        {
            var res = new CommonResponse<bool>();

            var movieFormatDAL = new MovieFormatDAL
            {
                Name = request.Name,
                Description = request.Description,
                Status = 1 // Mặc định là active
            };

            _movieFormatDAO.CreateMovieFormat(movieFormatDAL, out int response);

            res.Data = response == 200;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpPost("UpdateMovieFormat")]
        public async Task<CommonResponse<bool>> UpdateMovieFormat([FromBody] UpdateMovieFormatReq request)
        {
            var res = new CommonResponse<bool>();

            var movieFormatDAL = _mapper.Map<MovieFormatDAL>(request);
            _movieFormatDAO.UpdateMovieFormat(movieFormatDAL, out int response);

            res.Data = response == 200;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpPost("DeleteMovieFormat")]
        public async Task<CommonResponse<bool>> DeleteMovieFormat(Guid id)
        {
            var res = new CommonResponse<bool>();

            _movieFormatDAO.DeleteMovieFormat(id, out int response);

            res.Data = response == 200;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpPost("AssignFormatToMovie")]
        public async Task<CommonResponse<bool>> AssignFormatToMovie([FromBody] AssignFormatToMovieReq request)
        {
            var res = new CommonResponse<bool>();

            var formatMovieDAL = _mapper.Map<MovieFormatMovieDAL>(request);
            _movieFormatDAO.AssignFormatToMovie(formatMovieDAL, out int response);

            res.Data = response == 200;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpPost("RemoveFormatFromMovie")]
        public async Task<CommonResponse<bool>> RemoveFormatFromMovie(Guid movieId, Guid formatId)
        {
            var res = new CommonResponse<bool>();

            _movieFormatDAO.RemoveFormatFromMovie(movieId, formatId, out int response);

            res.Data = response == 200;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpGet("GetMovieFormatsByMovieId")]
        public async Task<CommonResponse<List<MovieFormatMovieRes>>> GetMovieFormatsByMovieId(Guid movieId)
        {
            var res = new CommonResponse<List<MovieFormatMovieRes>>();

            var result = _movieFormatDAO.GetMovieFormatsByMovieId(movieId, out int response);
            var resultMapper = _mapper.Map<List<MovieFormatMovieRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpGet("GetMoviesByFormatId")]
        public async Task<CommonPagination<List<MovieFormatMovieRes>>> GetMoviesByFormatId(Guid formatId, int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<MovieFormatMovieRes>>();

            var result = _movieFormatDAO.GetMoviesByFormatId(formatId, currentPage, recordPerPage, out int totalRecord, out int response);
            var resultMapper = _mapper.Map<List<MovieFormatMovieRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;

            return res;
        }
    }
}
