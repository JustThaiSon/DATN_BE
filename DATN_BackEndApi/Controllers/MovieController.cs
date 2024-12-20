using AutoMapper;
using DATN_BackEndApi.Extension;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Movies.Req;
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
        public MovieController(IMovieDAO movieDAO, IConfiguration configuration, IUltil ultils, IMapper mapper)
        {
            _movieDAO = movieDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("CreateActor")]

        public async Task<CommonResponse<dynamic>> CreateActor(ActorReq rq)
        {
            var res = new CommonResponse<dynamic>();
            _movieDAO.CreateActor(rq, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpGet]
        [Route("GetListActor")]

        public async Task<CommonPagination<List<GetListActorRes>>> GetListActor(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetListActorRes>>();
            var result = _movieDAO.GetListActor(currentPage, recordPerPage, out int TotalRecord, out int response);
            var resultMapper = _mapper.Map<List<GetListActorRes>>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = TotalRecord;
            return res;
        }

        [HttpGet]
        [Route("GetDetailActor")]

        public async Task<CommonResponse<GetListActorRes>> GetDetailActor(Guid Id)
        {
            var res = new CommonResponse<GetListActorRes>();
            var result = _movieDAO.GetDetailActor(Id, out int response);
            var resultMapper = _mapper.Map<GetListActorRes>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }











        #region Movie_Nghia
        [HttpPost]
        [Route("GetMovieList")]

        public async Task<CommonResponse<dynamic>> GetMovieList(ActorReq rq)
        {
            var res = new CommonResponse<dynamic>();
            _movieDAO.CreateActor(rq, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost]
        [Route("CreateMovie")]

        public async Task<CommonResponse<dynamic>> CreateMovie(ActorReq rq)
        {
            var res = new CommonResponse<dynamic>();
            _movieDAO.CreateActor(rq, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost]
        [Route("UpdateMovie")]

        public async Task<CommonResponse<dynamic>> UpdateMovie(ActorReq rq)
        {
            var res = new CommonResponse<dynamic>();
            _movieDAO.CreateActor(rq, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost]
        [Route("DeleteMovie")]

        public async Task<CommonResponse<dynamic>> DeleteMovie(ActorReq rq)
        {
            var res = new CommonResponse<dynamic>();
            _movieDAO.CreateActor(rq, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }



        [HttpPost]
        [Route("TestMovie")]

        public async Task<CommonResponse<dynamic>> TestMovie([FromBody] ActorReq rq, [FromQuery] params Guid[] ActorIDs)
        {
            var res = new CommonResponse<dynamic>();
            _movieDAO.CreateActor(rq, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }



        #endregion
    }
}
