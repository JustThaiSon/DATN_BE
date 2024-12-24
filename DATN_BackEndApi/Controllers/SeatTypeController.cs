using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Room;
using DATN_Models.DAL.Seat;
using DATN_Models.DAL.SeatType;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Room.Req;
using DATN_Models.DTOS.Seat.Req;
using DATN_Models.DTOS.Seat.Res;
using DATN_Models.DTOS.SeatType.Req;
using DATN_Models.DTOS.SeatType.Res;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatTypeController : ControllerBase
    {
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly ISeatTypeDAO _seatTypeDAO;

        public SeatTypeController(IConfiguration configuration, IUltil ultils, IMapper mapper, ISeatTypeDAO seatTypeDAO)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _seatTypeDAO = seatTypeDAO;
        }


        [HttpGet]
        [Route("GetAllSeatType")]

        public async Task<CommonPagination<List<GetListSeatTypeRes>>> GetAllSeatType( int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetListSeatTypeRes>>();

            var result = _seatTypeDAO.GetListSeatType( currentPage, recordPerPage, out int TotalRecord, out int response);

            var resultMapper = _mapper.Map<List<GetListSeatTypeRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = TotalRecord;

            return res;
        }

        [HttpPost]
        [Route("CreateSeatType")]

        public async Task<CommonResponse<dynamic>> CreateRoom(CreateSeatTypeReq rq)
        {
            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<CreateSeatTypeDAL>(rq);
            _seatTypeDAO.CreateSeatType(resultMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost]
        [Route("UpdateSeatTypeMultiplier")]

        public async Task<CommonResponse<dynamic>> UpdateSeatTypeMultiplier(UpdateSeatTypeMultiplierReq rq)
        {
            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<UpdateSeatTypeMultiplierDAL>(rq);
            _seatTypeDAO.UpdateSeatTypeMultiplier(resultMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;

        }

        [HttpPost]
        [Route("DeleteSeatType")]

        public async Task<CommonResponse<dynamic>> DeleteSeatType(Guid id )
        {
            var res = new CommonResponse<dynamic>();
            _seatTypeDAO.DeleteSeatType(id, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }
    }
}
