using AutoMapper;
using DATN_BackEndApi.Extension.CloudinarySett;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Movie.Actor;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Movies.Req;
using DATN_Models.DTOS.Movies.Req.Actor;
using DATN_Models.DTOS.Movies.Res;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    //[BAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly IMovieDAO _movieDAO;
        private readonly IActorDAO _actorDAO;
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly ImageService _imgService;

        public ActorController(IMovieDAO movieDAO, IConfiguration configuration, IUltil ultils, IMapper mapper, ImageService imgService, IActorDAO actorDAO)
        {
            _movieDAO = movieDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _imgService = imgService;
            _actorDAO = actorDAO;
        }


        #region Actor


        // Cái api này sẽ chạy khá lâu vì cần thời gian để upload ảnh lên cloud (mất tầm 5-10s)
        [HttpPost]
        [Route("CreateActor")]
        public async Task<CommonResponse<dynamic>> CreateActor([FromForm] AddActorReq rq/*, [FromQuery] IBrowserFile? Photo*/)
        {
            var res = new CommonResponse<dynamic>();

            var reqMapper = _mapper.Map<AddActorDAL>(rq);

            if (rq.Photo != null)
            {
                // gán photoURL = ảnh cloud
                reqMapper.PhotoURL = await _imgService.UploadImageAsync(rq.Photo);
            }

            _actorDAO.CreateActor(reqMapper, out int response);

            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }


        [HttpPost]
        [Route("UpdateActor")]
        public async Task<CommonResponse<dynamic>> UpdateActor([FromQuery] Guid Id, [FromForm] UpdateActorReq rq)
        {
            var res = new CommonResponse<dynamic>();
            var reqMapper = _mapper.Map<UpdateActorDAL>(rq);

            if (rq.Photo != null)
            {
                // gán photoURL = ảnh cloud
                reqMapper.PhotoURL = await _imgService.UploadImageAsync(rq.Photo);
            }

            _actorDAO.UpdateActor(Id, reqMapper, out int response);

            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpPost]
        [Route("DeleteActor")]
        public async Task<CommonResponse<dynamic>> DeleteActor([FromQuery] Guid Id)
        {
            var res = new CommonResponse<dynamic>();

            _actorDAO.DeleteActor(Id, out int response);

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
            var result = _actorDAO.GetListActor(currentPage, recordPerPage, out int TotalRecord, out int response);
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
            var result = _actorDAO.GetDetailActor(Id, out int response);
            var resultMapper = _mapper.Map<GetListActorRes>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        #endregion
    }
}
