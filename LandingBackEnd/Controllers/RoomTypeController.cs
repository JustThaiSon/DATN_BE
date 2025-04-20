using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.SeatType;
using DATN_Models.DAO.Interface;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Models.DTOS.RoomType.Res;
using DATN_Models.DTOS.SeatType.Req;
using DATN_Models.DTOS.SeatType.Res;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeDAO _roomTypeDAO;
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;

        public RoomTypeController(IRoomTypeDAO roomTypeDAO, IConfiguration configuration, IUltil ultils, IMapper mapper)
        {
            _roomTypeDAO = roomTypeDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
        }


        [HttpGet]
        [Route("GetListRoomType")]

        public async Task<CommonPagination<List<RoomTypeGetListRes>>> GetListRoomType(int currentPage, int recordPerPage)
        {

            var res = new CommonPagination<List<RoomTypeGetListRes>>();

            var result = _roomTypeDAO.GetListRoomType(currentPage, recordPerPage, out int TotalRecord, out int response);

            var resultMapper = _mapper.Map<List<RoomTypeGetListRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = TotalRecord;

            return res;
        }


    }
}
