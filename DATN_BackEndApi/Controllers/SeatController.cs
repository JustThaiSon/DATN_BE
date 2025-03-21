using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Seat;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Models.DTOS.Seat.Req;
using DATN_Models.DTOS.Seat.Res;
using DATN_Models.HandleData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly DATN_Context _db;

        public SeatController(IConfiguration configuration, IUltil ultils, IMapper mapper, ISeatDAO seatDAO)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _seatDAO = seatDAO;
            _db = new DATN_Context();
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


        [HttpGet]
        [Route("GetAllSeatByShowTime")]

        public async Task<CommonPagination<List<GetListSeatByShowTimeRes>>> GetAllSeatByShowTime(Guid showTimeId, int currentPage, int recordPerPage)
        {

            var res = new CommonPagination<List<GetListSeatByShowTimeRes>>();

            var result = _seatDAO.GetListSeatByShowTime(showTimeId, currentPage, recordPerPage, out int TotalRecord, out int response);

            var resultMapper = _mapper.Map<List<GetListSeatByShowTimeRes>>(result);

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
        [Route("UpdateStatusSeatByShowTime")]
        public async Task<CommonResponse<dynamic>> UpdateStatusSeatByShowTime(UpdateSeatByShowTimeStatusReq rq)
        {
            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<UpdateSeatByShowTimeStatusDAL>(rq);
            _seatDAO.UpdateSeatByShowTimeStatus(resultMapper, out int response);
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

        [HttpPost]
        [Route("SetupPair")]
        public async Task<CommonResponse<dynamic>> SetupPairSeat(SetupPairReq rq)
        {
            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<SetupPair>(rq);
            _seatDAO.SetupPair(resultMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;

        }

        [HttpPost]
        [Route("UnSetupPair")]
        public async Task<IActionResult> UnSetupPairSeat(Guid id1, Guid id2)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    // Tạm thời vô hiệu hóa triggers
                    await _db.Database.ExecuteSqlRawAsync("DISABLE TRIGGER ALL ON Seats;");

                    // Tìm và cập nhật ghế
                    var seat1 = await _db.Seats.FindAsync(id1);
                    var seat2 = await _db.Seats.FindAsync(id2);

                    if (seat1 == null || seat2 == null)
                    {
                        return NotFound(new { success = false, message = "Không tìm thấy ghế." });
                    }

                    seat1.PairId = null;
                    seat1.SeatTypeId = null;
                    seat2.PairId = null;
                    seat2.SeatTypeId = null;

                    await _db.SaveChangesAsync();

                    // Kích hoạt lại triggers
                    await _db.Database.ExecuteSqlRawAsync("ENABLE TRIGGER ALL ON Seats;");

                    // Commit transaction
                    await transaction.CommitAsync();

                    return Ok(new { success = true, message = "Hủy liên kết ghế đôi thành công." });
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi
                    await transaction.RollbackAsync();
                    return StatusCode(500, new { success = false, message = $"Lỗi khi hủy liên kết ghế đôi: {ex.Message}" });
                }
            }
        }
    }
}