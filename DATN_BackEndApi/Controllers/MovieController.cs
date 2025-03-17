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
        [HttpGet]
        [Route("GetMovieList")]
        public async Task<CommonPagination<List<GetMovieRes>>> GetMovieList(int currentPage, int recordPerPage)
        {
            // Khai báo phân trang
            var res = new CommonPagination<List<GetMovieRes>>();

            // Truyền req (trang hiện tại + bản ghi/ trang) => trả về 1 list phim từ DB (thủ tục), tổng bản ghi + mã response
            var result = _movieDAO.GetListMovie(currentPage, recordPerPage, out int TotalRecord, out int response);

            // Map lại res
            var resultMapper = _mapper.Map<List<GetMovieRes>>(result);

            // Gán dữ liệu => trả về phía FE (dữ liệu phim, tổng bản ghi + mã response + message)
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = TotalRecord;

            return res;
        }


        [HttpGet]
        [Route("GetMovieDetail")]
        public async Task<CommonResponse<GetMovieRes>> GetMovieDetail(Guid Id)
        {
            var res = new CommonResponse<GetMovieRes>();
            var result = _movieDAO.GetMovieDetail(Id, out int response);

            var resultMapper = _mapper.Map<GetMovieRes>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        /// <summary>
        /// Thêm movie
        /// </summary>
        /// <param name="rq">Request</param>
        /// <param name="ActorIDs">Ds Actor muốn thêm (ID)</param>
        /// <returns>
        /// Trả về response, bao gồm mã response và message
        /// </returns>
        [HttpPost]
        [Route("CreateMovie")]
        public async Task<CommonResponse<dynamic>> CreateMovie(AddMovieReq req)
        {
            var res = new CommonResponse<dynamic>();
            var reqMapper = _mapper.Map<AddMovieDAL>(req);


            if (req.Thumbnail != null)
            {
                reqMapper.ThumbnailURL = await _cloudService.UploadImageAsync(req.Thumbnail).ConfigureAwait(false);
            }

            if (req.Banner != null)
            {
                reqMapper.BannerURL = await _cloudService.UploadImageAsync(req.Banner).ConfigureAwait(false);
            }

            if (req.Trailer != null)
            {
                reqMapper.TrailerURL = await _cloudService.UploadVideoAsync(req.Trailer).ConfigureAwait(false);
            }

            _movieDAO.CreateMovie(reqMapper, out int response);


            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }


        // Phần update này tạm thời như vậy, sau này sửa lại sau tuỳ vào thiết kế giao diện.
        [HttpPost]
        [Route("UpdateMovie")]
        public async Task<CommonResponse<dynamic>> UpdateMovie(UpdateMovieReq req)
        {
            var res = new CommonResponse<dynamic>();
            var reqMapper = _mapper.Map<UpdateMovieDAL>(req);


            if (req.Thumbnail != null)
            {
                reqMapper.ThumbnailURL = await _cloudService.UploadImageAsync(req.Thumbnail).ConfigureAwait(false);
            }

            if (req.Banner != null)
            {
                reqMapper.BannerURL = await _cloudService.UploadImageAsync(req.Banner).ConfigureAwait(false);
            }

            if (req.Trailer != null)
            {
                reqMapper.TrailerURL = await _cloudService.UploadVideoAsync(req.Trailer).ConfigureAwait(false);
            }


            _movieDAO.UpdateMovie(reqMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }


        [HttpPost]
        [Route("DeleteMovie")]
        public async Task<CommonResponse<dynamic>> DeleteMovie(Guid id)
        {
            var res = new CommonResponse<dynamic>();
            _movieDAO.DeleteMovie(id, out int response);

            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }



        //[HttpPost]
        //[Route("TestMovie")]
        //public async Task<CommonResponse<dynamic>> TestMovie([FromBody] ActorReq rq, [FromQuery] params Guid[] ActorIDs)
        //{
        //    var res = new CommonResponse<dynamic>();
        //    _movieDAO.CreateActor(rq, out int response);
        //    res.Data = null;
        //    res.Message = MessageUtils.GetMessage(response, _langCode);
        //    res.ResponseCode = response;
        //    return res;
        //}



        #endregion


    }
}
