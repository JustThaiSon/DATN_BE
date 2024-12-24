using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Room;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Movies.Req;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.Room.Req;
using DATN_Models.DTOS.Room.Res;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly IRoomDAO _roomDAO;

        public RoomController(IConfiguration configuration, IUltil ultils, IMapper mapper, IRoomDAO roomDAO)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _roomDAO = roomDAO;
        }

        [HttpPost]
        [Route("CreateRoom")]

        public async Task<CommonResponse<dynamic>> CreateRoom(CreateRoomReq rq)
        {
            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<CreateRoomDAL>(rq);
            _roomDAO.CreateRoom(resultMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpGet]
        [Route("GetAllRoom")]

        public async Task<CommonPagination<List<GetListRoomRes>>> GetAllRoom(int currentPage, int recordPerPage)
        {

            var res = new CommonPagination<List<GetListRoomRes>>();

            var result = _roomDAO.GetListRoom(currentPage, recordPerPage, out int TotalRecord, out int response);

            var resultMapper = _mapper.Map<List<GetListRoomRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = TotalRecord;

            return res;
        }


    }
}
