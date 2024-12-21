using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Room;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Movies.Req;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.Room.Req;
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
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi"; ;
            _ultils = ultils;
            _mapper = mapper;
            _roomDAO = roomDAO;
        }

        [HttpPost]
        [Route("CreateRoom")]

        public async Task<CommonResponse<dynamic>> CreateActor(CreateRoomReq rq)
        {
            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<CreateRoomDAL>(rq);
            _roomDAO.CreateRoom(resultMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }


    }
}
