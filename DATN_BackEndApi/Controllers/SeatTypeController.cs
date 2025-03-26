using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.SeatType;
using DATN_Models.DAO.Interface.SeatAbout;
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
        private readonly ISeatTypeDAO _seatTypeDAO;
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;

        public SeatTypeController(ISeatTypeDAO seatTypeDAO, IConfiguration configuration, IUltil ultils, IMapper mapper)
        {
            _seatTypeDAO = seatTypeDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("CreateSeatType")]

        public async Task<CommonResponse<dynamic>> CreateSeatType(CreateSeatTypeReq rq)
        {
            var res = new CommonResponse<dynamic>();

            var reqmapper = _mapper.Map<CreateSeatTypeDAL>(rq);
            _seatTypeDAO.CreateSeatType(reqmapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }


        [HttpPost]
        [Route("UpdateSeatType")]

        public async Task<CommonResponse<dynamic>> UpdateSeatType(UpdateSeatTypeMultiplierReq rq)
        {
            var reqmapper = _mapper.Map<UpdateSeatTypeMultiplierDAL>(rq);

            var res = new CommonResponse<dynamic>();
            _seatTypeDAO.UpdateSeatTypeMultiplier(reqmapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }



        [HttpPost]
        [Route("DeleteSeatType")]

        public async Task<CommonResponse<dynamic>> DeleteSeatType(Guid id)
        {

            var res = new CommonResponse<dynamic>();
            _seatTypeDAO.DeleteSeatType(id, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }



        [HttpGet]
        [Route("GetListSeatType")]

        public async Task<CommonPagination<List<GetListSeatTypeRes>>> GetListSeatType(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetListSeatTypeRes>>();

            var result = _seatTypeDAO.GetListSeatType(currentPage, recordPerPage, out int TotalRecord, out int response);

            var resultMapper = _mapper.Map<List<GetListSeatTypeRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = TotalRecord;

            return res;
        }



    }
}
