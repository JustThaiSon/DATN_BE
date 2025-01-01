using AutoMapper;
using DATN_BackEndApi.Extension.CloudinarySett;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Movie;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Movies.Req.Movie;
using DATN_Models.DTOS.Movies.Res;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
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
        private readonly CloudService _cloudService;

        public MovieController(IMovieDAO movieDAO, IConfiguration configuration, IUltil ultils, IMapper mapper, CloudService imgService)
        {
            _movieDAO = movieDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _cloudService = imgService;
        }

        #region Movie_Nghia
        [HttpGet("GetMovieList")]
        public async Task<CommonPagination<List<GetMovieRes>>> GetMovieList(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetMovieRes>>();
            var result = _movieDAO.GetListMovie(currentPage, recordPerPage, out int totalRecord, out int response);
            res.Data = _mapper.Map<List<GetMovieRes>>(result);
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;
            return res;
        }

        [HttpGet("GetMovieDetail")]
        public async Task<CommonResponse<GetMovieRes>> GetMovieDetail(Guid id)
        {
            var res = new CommonResponse<GetMovieRes>();
            var result = _movieDAO.GetMovieDetail(id, out int response);
            res.Data = _mapper.Map<GetMovieRes>(result);
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost("CreateMovie")]
        public async Task<CommonResponse<dynamic>> CreateMovie(AddMovieReq req)
        {
            var res = new CommonResponse<dynamic>();
            var reqMapper = _mapper.Map<AddMovieDAL>(req);

            var uploadTasks = new List<Task>();
            if (req.Thumbnail != null)
            {
                uploadTasks.Add(Task.Run(async () => reqMapper.ThumbnailURL = await _cloudService.UploadImageAsync(req.Thumbnail).ConfigureAwait(false)));
            }
            if (req.Banner != null)
            {
                uploadTasks.Add(Task.Run(async () => reqMapper.BannerURL = await _cloudService.UploadImageAsync(req.Banner).ConfigureAwait(false)));
            }
            if (req.Trailer != null)
            {
                uploadTasks.Add(Task.Run(async () => reqMapper.TrailerURL = await _cloudService.UploadVideoAsync(req.Trailer).ConfigureAwait(false)));
            }
            await Task.WhenAll(uploadTasks);

            _movieDAO.CreateMovie(reqMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost("UpdateMovie")]
        public async Task<CommonResponse<dynamic>> UpdateMovie(UpdateMovieReq req)
        {
            var res = new CommonResponse<dynamic>();
            var reqMapper = _mapper.Map<UpdateMovieDAL>(req);
            _movieDAO.UpdateMovie(reqMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost("DeleteMovie")]
        public async Task<CommonResponse<dynamic>> DeleteMovie(Guid id)
        {
            var res = new CommonResponse<dynamic>();
            _movieDAO.DeleteMovie(id, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }
        #endregion
    }
}
