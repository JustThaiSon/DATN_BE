using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Room;
using DATN_Models.DAL.Seat;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Room.Req;
using DATN_Models.DTOS.Room.Res;
using DATN_Models.DTOS.Seat.Req;
using DATN_Models.DTOS.Seat.Res;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly ISeatDAO _seatDAO;

        public SeatController(IConfiguration configuration, IUltil ultils, IMapper mapper, ISeatDAO seatDAO)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _seatDAO = seatDAO;
        }

        [HttpGet]
        [Route("GetAllSeat")]

        public async Task<CommonPagination<List<GetListSeatRes>>> GetAllSeat(Guid id, int currentPage, int recordPerPage)
        {

            var res = new CommonPagination<List<GetListSeatRes>>();

            var result = _seatDAO.GetListSeat(id, currentPage, recordPerPage, out int TotalRecord, out int response);

            var resultMapper = _mapper.Map<List<GetListSeatRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = TotalRecord;

            return res;
        }

        [HttpPost]
        [Route("UpdateStatusSeat")]

        public async Task<CommonResponse<dynamic>> UpdateStatusSeat(UpdateSeatStatusReq rq)
        {
            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<UpdateSeatStatusDAL>(rq);
            _seatDAO.UpdateSeatStatus(resultMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;

        }

        [HttpPost]
        [Route("UpdateTypeSeat")]

        public async Task<CommonResponse<dynamic>> UpdateTypeSeat(UpdateSeatTypeReq rq)
        {
            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<UpdateSeatTypeDAL>(rq);
            _seatDAO.UpdateSeatType(resultMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;

        }
    }
}
