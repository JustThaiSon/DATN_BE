using AutoMapper;
using DATN_BackEndApi.Extension;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Cinemas;
using DATN_Models.DTOS.ShowTime.Req;
using DATN_Models.DTOS.ShowTime.Res;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[BAuthorize]
    public class ShowTimeController : ControllerBase
    {
        private readonly IShowTimeDAO _showtimeDAO;
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;

        public ShowTimeController(IShowTimeDAO movieDAO, IConfiguration configuration, IUltil ultils, IMapper mapper)
        {
            _showtimeDAO = movieDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("CreateShowTime")]

        public async Task<CommonResponse<dynamic>> CreateShowTime(ShowTimeReq rq)
        {
            var res = new CommonResponse<dynamic>();
            _showtimeDAO.CreateShowTime(rq, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost]
        [Route("UpdateShowTime")]

        public async Task<CommonResponse<dynamic>> UpdateShowTime(Guid ShowTimeId, ShowTimeReq rq)
        {
            var res = new CommonResponse<dynamic>();
            _showtimeDAO.UpdateShowTime(ShowTimeId,rq,out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost]
        [Route("DeleteShowTime")]
        public async Task<CommonResponse<dynamic>> DeleteShowTime(Guid showTimeId)
        {
            var res = new CommonResponse<dynamic>();
            _showtimeDAO.DeleteShowTime(showTimeId, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpGet]
        [Route("GetListShowTimes")]
        public async Task<CommonPagination<List<ShowTimeRes>>> GetListShowTimes(Guid movieId, Guid roomId, int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<ShowTimeRes>>();
            var result = _showtimeDAO.GetListShowTime(movieId, roomId, currentPage, recordPerPage, out int totalRecord, out int response);
            var resultMapper = _mapper.Map<List<ShowTimeRes>>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;

            return res;
        }

    }
}
