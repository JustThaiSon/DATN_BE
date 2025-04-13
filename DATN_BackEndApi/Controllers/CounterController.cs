using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Counter;
using DATN_Models.DAO.Interface;
using DATN_Models.HandleData;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        private readonly ICounterDAO _counterDAO;
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly DATN_Context _db;

        public CounterController(ICounterDAO counterDAO, IConfiguration configuration, IUltil ultils, IMapper mapper)
        {
            _counterDAO = counterDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _db = new DATN_Context();
        }

        [HttpGet]
        [Route("GetNowPlayingMoviesFormatted")]
        public async Task<CommonPagination<List<dynamic>>> GetNowPlayingMoviesFormatted(
            int currentPage = 1,
            int recordPerPage = 10,
            [FromQuery] DateTime? showDate = null,
            [FromQuery] Guid? cinemaId = null,
            [FromQuery] Guid? genreId = null)
        {
            var res = new CommonPagination<List<dynamic>>();

            // Gọi phương thức DAO để lấy dữ liệu
            var result = _counterDAO.GetNowPlayingMovies(
                currentPage,
                recordPerPage,
                showDate,
                cinemaId,
                genreId,
                out int totalRecord,
                out int response);

            // Định dạng lại kết quả
            var formattedResult = result.Select(movie => new
            {
                movie.MovieId,
                movie.MovieName,
                movie.Description,
                movie.Thumbnail,
                movie.Banner,
                movie.Trailer,
                movie.Duration,
                movie.Status,
                movie.ReleaseDate,
                Genres = movie.GenresList,
                Showtimes = movie.ShowtimesList
            }).ToList<dynamic>();

            // Trả về kết quả
            res.Data = formattedResult;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;

            return res;
        }


        [HttpGet]
        [Route("QRScanner/GetTicketInfo/{orderCode}")]
        public async Task<IActionResult> GetTicketInfoByQRCode(string orderCode)
        {
            try
            {
                // Gọi phương thức DAO để lấy thông tin vé từ mã QR
                var ticketInfo = _counterDAO.GetTicketInfoFromQR(
                    orderCode,
                    out int response,
                    out string message);

                // Kiểm tra kết quả
                if (response != 200) // Nếu không thành công
                {
                    return StatusCode(response >= 400 && response < 500 ? response : 500,
                        new
                        {
                            ResponseCode = response,
                            Message = message
                        });
                }

                // Định dạng lại dữ liệu để trả về client
                var result = new
                {
                    ResponseCode = response,
                    Message = message,
                    OrderInfo = FormatOrderData(ticketInfo.Tables[0]),
                    Tickets = FormatTicketsData(ticketInfo.Tables[1]),
                    Services = ticketInfo.Tables.Count > 2 ? FormatServicesData(ticketInfo.Tables[2]) : new List<object>()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ResponseCode = -99,
                    Message = "Lỗi hệ thống: " + ex.Message
                });
            }
        }

        [HttpPost]
        [Route("QRScanner/ConfirmTicket/{orderCode}")]
        public async Task<IActionResult> ConfirmTicketUsage(string orderCode)
        {
            try
            {
                // Gọi phương thức DAO để xác nhận sử dụng vé
                bool success = _counterDAO.ConfirmTicketUsage(
                    orderCode,
                    out int response,
                    out string message);

                // Định dạng kết quả trả về
                var result = new
                {
                    ResponseCode = response,
                    Message = message,
                    Success = success
                };

                // Trả về kết quả
                if (success)
                    return Ok(result);
                else
                    return StatusCode(response >= 400 && response < 500 ? response : 500, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ResponseCode = -99,
                    Message = "Lỗi hệ thống: " + ex.Message,
                    Success = false
                });
            }
        }

        // Phương thức hỗ trợ định dạng dữ liệu
        private dynamic FormatOrderData(DataTable orderTable)
        {
            if (orderTable.Rows.Count == 0)
                return null;

            var row = orderTable.Rows[0];
            return new
            {
                OrderId = row["OrderId"].ToString(),
                OrderCode = row["OrderCode"].ToString(),
                CustomerName = row["CustomerName"].ToString(),
                Email = row["Email"].ToString(),
                TotalPrice = Convert.ToInt64(row["OrderTotalPrice"]),
                FormattedTotalPrice = _ultils.FormatMoney(Convert.ToInt64(row["OrderTotalPrice"])),
                OrderDate = Convert.ToDateTime(row["OrderDate"]),
                FormattedOrderDate = Convert.ToDateTime(row["OrderDate"]).ToString("dd/MM/yyyy HH:mm")
            };
        }

        private List<dynamic> FormatTicketsData(DataTable ticketsTable)
        {
            var result = new List<dynamic>();

            foreach (DataRow row in ticketsTable.Rows)
            {
                result.Add(new
                {
                    TicketId = row["TicketId"].ToString(),
                    TicketCode = row["TickeCode"].ToString(),
                    MovieName = row["MovieName"].ToString(),
                    CinemaName = row["CinemaName"].ToString(),
                    RoomName = row["RoomName"].ToString(),
                    SeatInfo = row["SeatName"].ToString(),
                    SeatName = row["SeatName"].ToString(),
                    StartTime = Convert.ToDateTime(row["StartTime"]),
                    EndTime = Convert.ToDateTime(row["EndTime"]),
                    FormattedStartTime = Convert.ToDateTime(row["StartTime"]).ToString("dd/MM/yyyy HH:mm"),
                    FormattedEndTime = Convert.ToDateTime(row["EndTime"]).ToString("HH:mm"),
                    Duration = Convert.ToInt32(row["Duration"]),
                    FormattedDuration = $"{Convert.ToInt32(row["Duration"])} phút",
                    MovieThumbnail = row["Thumbnail"].ToString()
                });
            }

            return result;
        }

        private List<dynamic> FormatServicesData(DataTable servicesTable)
        {
            var result = new List<dynamic>();

            foreach (DataRow row in servicesTable.Rows)
            {
                result.Add(new
                {
                    OrderServiceId = row["OrderServiceId"].ToString(),
                    ServiceName = row["ServiceName"].ToString(),
                    ServiceType = row["ServiceTypeName"].ToString(),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                    UnitPrice = Convert.ToInt64(row["UnitPrice"]),
                    FormattedUnitPrice = _ultils.FormatMoney(Convert.ToInt64(row["UnitPrice"])),
                    TotalPrice = Convert.ToInt64(row["ServiceTotalPrice"]),
                    FormattedTotalPrice = _ultils.FormatMoney(Convert.ToInt64(row["ServiceTotalPrice"])),
                    ImageUrl = row["ImageUrl"].ToString()
                });
            }

            return result;
        }
    }
}