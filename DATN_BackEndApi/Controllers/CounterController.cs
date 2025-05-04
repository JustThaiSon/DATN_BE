using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Counter;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Counter.Req;
using DATN_Models.DTOS.Customer.Req;
using DATN_Models.HandleData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;

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
        [Route("GetUserByEmail")]
        public IActionResult GetUserByEmail(string email)
        {
            try
            {
                // Gọi phương thức DAO để lấy thông tin người dùng theo email
                var user = _counterDAO.GetUserByEmail(
                    email,
                    out int response,
                    out string message);

                // Kiểm tra kết quả
                if (response != 200) // Nếu không thành công
                {
                    return StatusCode(response == -128 ? 404 : 500,
                        new
                        {
                            ResponseCode = response,
                            Message = message
                        });
                }

                // Trả về kết quả thành công
                return Ok(new
                {
                    ResponseCode = response,
                    Message = message,
                    User = user
                });
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
        [Route("RefundOrder/{orderCode}")]
        public async Task<IActionResult> RefundOrder(string orderCode)
        {
            try
            {
                // Gọi phương thức DAO để hoàn vé dựa trên OrderCode
                var result = _counterDAO.RefundOrderByOrderCode(
                    orderCode,
                    out int response,
                    out string message);

                // Kiểm tra kết quả
                if (response != 200) // Nếu không thành công
                {
                    return StatusCode(response == -128 || response == -129 ? 400 : 500,
                        new
                        {
                            ResponseCode = response,
                            Message = message
                        });
                }

                // Định dạng dữ liệu để trả về client
                var refundInfo = new
                {
                    ResponseCode = response,
                    Message = message,
                    RefundDetails = result.Tables.Count > 0 ? FormatRefundData(result.Tables[0]) : null
                };

                return Ok(refundInfo);
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

        // Helper method để định dạng dữ liệu hoàn vé
        private object FormatRefundData(DataTable refundData)
        {
            if (refundData.Rows.Count == 0)
                return null;

            var row = refundData.Rows[0];
            return new
            {
                Email = ConvertUtil.ToString(row["Email"]),
                RefundAmount = ConvertUtil.ToDecimal(row["RefundAmount"]),
                OrderCode = ConvertUtil.ToString(row["OrderCode"]),
                ShowTimeId = ConvertUtil.ToGuid(row["ShowTimeId"]),
                SeatStatusByShowTimeIds = ConvertUtil.ToString(row["SeatStatusByShowTimeIds"])?.Split(',')
                    .Select(id => string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id))
                    .Where(id => id != Guid.Empty)
                    .ToList()
            };
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

        [HttpPost]
        [Route("Service/ManageOrder")]
        public async Task<IActionResult> ManageServiceOrder(int action, string orderCode, [FromBody] JsonElement requestData)
        {
            try
            {
                // Kiểm tra đầu vào
                if (string.IsNullOrEmpty(orderCode) && (action == 2 || action == 3))
                {
                    return BadRequest(new { ResponseCode = 400, Message = "Mã đơn hàng không được để trống" });
                }

                switch (action)
                {
                    case 1: // Tạo đơn hàng
                        string serviceListJson = string.Empty;
                        string email = "customer@example.com"; // Giá trị mặc định
                        bool isAnonymous = false;

                        // Trích xuất dữ liệu từ requestData
                        if (requestData.TryGetProperty("serviceListJson", out JsonElement serviceListElem) &&
                            serviceListElem.ValueKind != JsonValueKind.Null)
                        {
                            serviceListJson = serviceListElem.GetString();
                        }

                        if (requestData.TryGetProperty("email", out JsonElement emailElem) &&
                            emailElem.ValueKind != JsonValueKind.Null)
                        {
                            email = emailElem.GetString();
                        }

                        if (requestData.TryGetProperty("isAnonymous", out JsonElement anonymousElem) &&
                            anonymousElem.ValueKind != JsonValueKind.False)
                        {
                            isAnonymous = anonymousElem.GetBoolean();
                        }

                        if (string.IsNullOrEmpty(serviceListJson))
                        {
                            return BadRequest(new { ResponseCode = 400, Message = "Danh sách dịch vụ không được để trống" });
                        }

                        // Gọi method tạo đơn hàng
                        int response1;
                        string message1;
                        string orderCode1;
                        var result1 = _counterDAO.CreateServiceOrder(email, null, isAnonymous, serviceListJson, out response1, out message1, out orderCode1);

                        return Ok(new { ResponseCode = response1, Message = message1, OrderCode = orderCode1 });

                    case 2: // Xác nhận thanh toán đơn hàng
                        Guid userId = Guid.Empty;

                        if (requestData.TryGetProperty("userId", out JsonElement userIdElem) &&
                            userIdElem.ValueKind != JsonValueKind.Null)
                        {
                            string userIdStr = userIdElem.GetString();
                            if (!string.IsNullOrEmpty(userIdStr))
                            {
                                userId = Guid.Parse(userIdStr);
                            }
                        }

                        if (userId == Guid.Empty)
                        {
                            return BadRequest(new { ResponseCode = 400, Message = "UserId không được để trống" });
                        }

                        // Gọi method xác nhận thanh toán
                        int response2;
                        string message2;
                        var result2 = _counterDAO.ConfirmServicePayment(orderCode, userId, out response2, out message2);

                        return Ok(new { ResponseCode = response2, Message = message2, OrderCode = orderCode });

                    case 3: // Xác nhận sử dụng dịch vụ
                        Guid confirmUserId = Guid.Empty;

                        if (requestData.TryGetProperty("userId", out JsonElement confirmUserIdElem) &&
                            confirmUserIdElem.ValueKind != JsonValueKind.Null)
                        {
                            string userIdStr = confirmUserIdElem.GetString();
                            if (!string.IsNullOrEmpty(userIdStr))
                            {
                                confirmUserId = Guid.Parse(userIdStr);
                            }
                        }

                        if (confirmUserId == Guid.Empty)
                        {
                            return BadRequest(new { ResponseCode = 400, Message = "UserId không được để trống" });
                        }

                        // Gọi method xác nhận sử dụng
                        int response3;
                        string message3;
                        var result3 = _counterDAO.ConfirmServiceUsage(orderCode, confirmUserId, out response3, out message3);

                        return Ok(new { ResponseCode = response3, Message = message3, OrderCode = orderCode });

                    // Bỏ case 4 - không xử lý nữa

                    default:
                        return BadRequest(new { ResponseCode = 400, Message = "Hành động không hợp lệ" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ResponseCode = 500, Message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("Service/GetOrderInfo/{orderCode}")]
        public async Task<IActionResult> GetServiceOrderInfo(string orderCode)
        {
            try
            {
                if (string.IsNullOrEmpty(orderCode))
                {
                    return BadRequest(new
                    {
                        ResponseCode = 400,
                        Message = "Mã đơn hàng không được để trống"
                    });
                }

                // Gọi phương thức DAO để lấy thông tin đơn hàng
                var orderInfo = _counterDAO.GetServiceOrderInfo(orderCode);

                // Kiểm tra kết quả
                if (orderInfo == null || orderInfo.Tables.Count < 2 || orderInfo.Tables[0].Rows.Count == 0)
                {
                    return NotFound(new
                    {
                        ResponseCode = 404,
                        Message = "Không tìm thấy thông tin đơn hàng"
                    });
                }

                // Định dạng dữ liệu
                var orderData = FormatServiceOrderData(orderInfo.Tables[0]);
                var serviceDetails = FormatServiceDetailsData(orderInfo.Tables[1]);

                // Trả về kết quả
                return Ok(new
                {
                    ResponseCode = 200,
                    Message = "Lấy thông tin đơn hàng thành công",
                    OrderInfo = orderData,
                    Services = serviceDetails
                });
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
        [Route("Service/QuickSale")]
        public async Task<IActionResult> QuickServiceSale([FromBody] QuickServiceSaleReq request)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (request == null || string.IsNullOrEmpty(request.ServiceListJson))
                {
                    return BadRequest(new
                    {
                        ResponseCode = 400,
                        Message = "Danh sách dịch vụ không được để trống"
                    });
                }

                if (request.UserId == Guid.Empty)
                {
                    return BadRequest(new
                    {
                        ResponseCode = 400,
                        Message = "ID nhân viên không được để trống"
                    });
                }

                // Gọi phương thức DAO để bán nhanh dịch vụ
                string orderCode = _counterDAO.QuickServiceSale(
                    request.ServiceListJson,
                    request.UserId,
                    request.CustomerEmail,
                    request.MarkAsUsed,
                    out int response,
                    out string message,
                    out decimal totalAmount);

                // Trả về kết quả
                if (response == 200)
                {
                    return Ok(new
                    {
                        ResponseCode = response,
                        Message = message,
                        OrderCode = orderCode,
                        TotalAmount = totalAmount,
                        FormattedTotalAmount = _ultils.FormatMoney(Convert.ToInt64(totalAmount))
                    });
                }
                else
                {
                    return StatusCode(response >= 400 && response < 500 ? response : 500, new
                    {
                        ResponseCode = response,
                        Message = message
                    });
                }
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

        // Phương thức hỗ trợ định dạng dữ liệu đơn hàng dịch vụ
        private dynamic FormatServiceOrderData(DataTable orderTable)
        {
            if (orderTable.Rows.Count == 0)
                return null;

            var row = orderTable.Rows[0];
            return new
            {
                OrderId = row["OrderId"].ToString(),
                OrderCode = row["OrderCode"].ToString(),
                Email = row["Email"].ToString(),
                TotalPrice = Convert.ToInt64(row["TotalPrice"]),
                DiscountPrice = Convert.ToInt64(row["DiscountPrice"]),
                FinalPrice = Convert.ToInt64(row["FinalPrice"]),
                FormattedTotalPrice = _ultils.FormatMoney(Convert.ToInt64(row["TotalPrice"])),
                FormattedDiscountPrice = _ultils.FormatMoney(Convert.ToInt64(row["DiscountPrice"])),
                FormattedFinalPrice = _ultils.FormatMoney(Convert.ToInt64(row["FinalPrice"])),
                Status = Convert.ToInt32(row["Status"]),
                StatusText = GetOrderStatusText(Convert.ToInt32(row["Status"])),
                OrderDate = row.Table.Columns.Contains("CreatedDate") ? Convert.ToDateTime(row["CreatedDate"]) : DateTime.MinValue,
                FormattedOrderDate = row.Table.Columns.Contains("CreatedDate")
                    ? Convert.ToDateTime(row["CreatedDate"]).ToString("dd/MM/yyyy HH:mm")
                    : string.Empty
            };
        }

        // Phương thức hỗ trợ định dạng dữ liệu chi tiết dịch vụ
        private List<dynamic> FormatServiceDetailsData(DataTable servicesTable)
        {
            var result = new List<dynamic>();

            foreach (DataRow row in servicesTable.Rows)
            {
                result.Add(new
                {
                    ServiceName = row["ServiceName"].ToString(),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                    UnitPrice = Convert.ToInt64(row["UnitPrice"]),
                    FormattedUnitPrice = _ultils.FormatMoney(Convert.ToInt64(row["UnitPrice"])),
                    TotalPrice = Convert.ToInt64(row["TotalPrice"]),
                    FormattedTotalPrice = _ultils.FormatMoney(Convert.ToInt64(row["TotalPrice"]))
                });
            }

            return result;
        }

        // Phương thức hỗ trợ lấy trạng thái đơn hàng dưới dạng text
        private string GetOrderStatusText(int status)
        {
            switch (status)
            {
                case 0: return "Chưa thanh toán";
                case 1: return "Đã thanh toán";
                case 2: return "Đã sử dụng";
                default: return "Không xác định";
            }
        }

        // Thêm vào CounterController.cs
        [HttpPost]
        [Route("Booking/TicketAndService")]
        public async Task<IActionResult> CreateTicketAndServiceOrder([FromBody] TicketOrderReq request)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (request == null || request.ShowTimeId == Guid.Empty || request.SelectedSeats == null || !request.SelectedSeats.Any())
                {
                    return BadRequest(new
                    {
                        ResponseCode = 400,
                        Message = "Vui lòng chọn suất chiếu và ghế ngồi"
                    });
                }

                // Chuyển đổi danh sách ghế thành JSON
                var seatList = new List<object>();
                foreach (var seat in request.SelectedSeats)
                {
                    seatList.Add(new
                    {
                        SeatByShowTimeId = seat.SeatByShowTimeId
                    });
                }
                string seatListJson = JsonConvert.SerializeObject(seatList);

                // Chuyển đổi danh sách dịch vụ thành JSON (nếu có)
                string serviceListJson = null;
                if (request.SelectedServices != null && request.SelectedServices.Any())
                {
                    var serviceList = new List<object>();
                    foreach (var service in request.SelectedServices)
                    {
                        serviceList.Add(new
                        {
                            ServiceId = service.ServiceId,
                            Quantity = service.Quantity
                        });
                    }
                    serviceListJson = JsonConvert.SerializeObject(serviceList);
                }

                // Gọi phương thức DAO để tạo đơn hàng
                string orderCode = _counterDAO.CreateTicketAndServiceOrder(
                    request.Email,
                    request.UserId,
                    request.IsAnonymous,
                    request.ShowTimeId,
                    seatListJson,
                    serviceListJson,
                    out int response,
                    out string message);

                // Trả về kết quả
                if (response == 200)
                {
                    return Ok(new
                    {
                        ResponseCode = response,
                        Message = message,
                        OrderCode = orderCode
                    });
                }
                else
                {
                    return StatusCode(response >= 400 && response < 500 ? response : 500, new
                    {
                        ResponseCode = response,
                        Message = message
                    });
                }
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

        // Cập nhật phương thức trong CounterController
        [HttpPost]
        [Route("Booking/Payment")]
        public async Task<IActionResult> ConfirmTicketAndServicePayment([FromBody] PaymentConfirmationReq request)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (request == null || string.IsNullOrEmpty(request.OrderCode))
                {
                    return BadRequest(new
                    {
                        ResponseCode = 400,
                        Message = "Mã đơn hàng không được để trống"
                    });
                }

                // Gọi phương thức DAO để xác nhận thanh toán - chỉ truyền orderCode và userId
                bool success = _counterDAO.ConfirmTicketAndServicePayment(
                    request.OrderCode,
                    request.UserId,
                    out int response,
                    out string message);

                // Trả về kết quả
                if (success)
                {
                    return Ok(new
                    {
                        ResponseCode = response,
                        Message = message,
                        OrderCode = request.OrderCode
                    });
                }
                else
                {
                    return StatusCode(response >= 400 && response < 500 ? response : 500, new
                    {
                        ResponseCode = response,
                        Message = message
                    });
                }
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

        [HttpGet]
        [Route("Booking/GetOrderInfo/{orderCode}")]
        public async Task<IActionResult> GetTicketAndServiceOrderInfo(string orderCode)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrEmpty(orderCode))
                {
                    return BadRequest(new
                    {
                        ResponseCode = 400,
                        Message = "Mã đơn hàng không được để trống"
                    });
                }

                // Gọi phương thức DAO để lấy thông tin đơn hàng
                var orderData = _counterDAO.GetTicketAndServiceOrderInfo(
                    orderCode,
                    out int response,
                    out string message);

                // Kiểm tra kết quả
                if (response != 200 || orderData == null || orderData.Tables.Count < 3)
                {
                    return StatusCode(response >= 400 && response < 500 ? response : 500, new
                    {
                        ResponseCode = response,
                        Message = message
                    });
                }

                // Xử lý và định dạng dữ liệu
                var orderInfo = FormatTicketOrderInfo(orderData.Tables[0]);
                var tickets = FormatTicketDetails(orderData.Tables[1]);
                var services = orderData.Tables.Count > 2 ? FormatServiceDetails(orderData.Tables[2]) : new List<dynamic>();
                var payments = orderData.Tables.Count > 3 ? FormatPaymentDetails(orderData.Tables[3]) : new List<dynamic>();

                // Trả về kết quả
                return Ok(new
                {
                    ResponseCode = response,
                    Message = message,
                    OrderInfo = orderInfo,
                    Tickets = tickets,
                    Services = services,
                    PaymentHistory = payments
                });
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
        [Route("Booking/CancelOrder")]
        public async Task<IActionResult> CancelUnpaidOrder([FromBody] CancelOrderReq request)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (request == null || string.IsNullOrEmpty(request.OrderCode))
                {
                    return BadRequest(new
                    {
                        ResponseCode = 400,
                        Message = "Mã đơn hàng không được để trống"
                    });
                }

                // Gọi phương thức DAO để hủy đơn hàng
                bool success = _counterDAO.CancelUnpaidOrder(
                    request.OrderCode,
                    request.UserId,
                    out int response,
                    out string message);

                // Trả về kết quả
                if (success)
                {
                    return Ok(new
                    {
                        ResponseCode = response,
                        Message = message,
                        OrderCode = request.OrderCode
                    });
                }
                else
                {
                    return StatusCode(response >= 400 && response < 500 ? response : 500, new
                    {
                        ResponseCode = response,
                        Message = message
                    });
                }
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

        // Phương thức hỗ trợ định dạng dữ liệu đơn hàng vé
        private dynamic FormatTicketOrderInfo(DataTable orderTable)
        {
            if (orderTable.Rows.Count == 0)
                return null;

            var row = orderTable.Rows[0];
            return new
            {
                OrderId = row["OrderId"].ToString(),
                OrderCode = row["OrderCode"].ToString(),
                Email = row["Email"].ToString(),
                TotalPrice = Convert.ToInt64(row["TotalPrice"]),
                DiscountPrice = Convert.ToInt64(row["DiscountPrice"]),
                FinalPrice = Convert.ToInt64(row["FinalPrice"]),
                FormattedTotalPrice = _ultils.FormatMoney(Convert.ToInt64(row["TotalPrice"])),
                FormattedDiscountPrice = _ultils.FormatMoney(Convert.ToInt64(row["DiscountPrice"])),
                FormattedFinalPrice = _ultils.FormatMoney(Convert.ToInt64(row["FinalPrice"])),
                Status = Convert.ToInt32(row["Status"]),
                StatusName = row["StatusName"].ToString(),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                FormattedCreatedDate = Convert.ToDateTime(row["CreatedDate"]).ToString("dd/MM/yyyy HH:mm"),
                UpdatedDate = row["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(row["UpdatedDate"]) : (DateTime?)null,
                FormattedUpdatedDate = row["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(row["UpdatedDate"]).ToString("dd/MM/yyyy HH:mm") : null
            };
        }

        // Phương thức hỗ trợ định dạng dữ liệu vé
        private List<dynamic> FormatTicketDetails(DataTable ticketsTable)
        {
            var result = new List<dynamic>();

            foreach (DataRow row in ticketsTable.Rows)
            {
                result.Add(new
                {
                    TicketCode = row["TickeCode"].ToString(),
                    SeatPrice = Convert.ToInt64(row["SeatPrice"]),
                    FormattedSeatPrice = _ultils.FormatMoney(Convert.ToInt64(row["SeatPrice"])),
                    SeatName = row["SeatName"].ToString(),
                    RoomName = row["RoomName"].ToString(),
                    CinemaName = row["CinemaName"].ToString(),
                    MovieTitle = row["MovieTitle"].ToString(),
                    StartTime = Convert.ToDateTime(row["StartTime"]),
                    EndTime = Convert.ToDateTime(row["EndTime"]),
                    FormattedStartTime = Convert.ToDateTime(row["StartTime"]).ToString("dd/MM/yyyy HH:mm"),
                    FormattedEndTime = Convert.ToDateTime(row["EndTime"]).ToString("HH:mm"),
                    Status = Convert.ToInt32(row["Status"]),
                    StatusName = row["StatusName"].ToString()
                });
            }

            return result;
        }

        // Phương thức hỗ trợ định dạng dữ liệu dịch vụ
        private List<dynamic> FormatServiceDetails(DataTable servicesTable)
        {
            var result = new List<dynamic>();

            foreach (DataRow row in servicesTable.Rows)
            {
                result.Add(new
                {
                    ServiceName = row["ServiceName"].ToString(),
                    Description = row.Table.Columns.Contains("Description") ? row["Description"].ToString() : null,
                    ImageUrl = row.Table.Columns.Contains("ImageUrl") ? row["ImageUrl"].ToString() : null,
                    Quantity = Convert.ToInt32(row["Quantity"]),
                    UnitPrice = Convert.ToInt64(row["UnitPrice"]),
                    FormattedUnitPrice = _ultils.FormatMoney(Convert.ToInt64(row["UnitPrice"])),
                    TotalPrice = Convert.ToInt64(row["TotalPrice"]),
                    FormattedTotalPrice = _ultils.FormatMoney(Convert.ToInt64(row["TotalPrice"]))
                });
            }

            return result;
        }

        // Phương thức hỗ trợ định dạng dữ liệu thanh toán
        private List<dynamic> FormatPaymentDetails(DataTable paymentsTable)
        {
            var result = new List<dynamic>();

            foreach (DataRow row in paymentsTable.Rows)
            {
                result.Add(new
                {
                    TransactionCode = row["TransactionCode"].ToString(),
                    AmountPaid = Convert.ToInt64(row["AmountPaid"]),
                    FormattedAmountPaid = _ultils.FormatMoney(Convert.ToInt64(row["AmountPaid"])),
                    PaymentDate = Convert.ToDateTime(row["PaymentDate"]),
                    FormattedPaymentDate = Convert.ToDateTime(row["PaymentDate"]).ToString("dd/MM/yyyy HH:mm"),
                    PaymentMethodName = row["PaymentMethodName"].ToString(),
                    LogoUrl = row.Table.Columns.Contains("LogoUrl") ? row["LogoUrl"].ToString() : null
                });
            }

            return result;
        }


        [HttpGet]
        [Route("Order/GetDetails/{orderCode}")]
        public async Task<IActionResult> GetOrderDetailsByOrderCode(string orderCode)
        {
            try
            {
                // Gọi phương thức DAO để lấy chi tiết đơn hàng
                var result = _counterDAO.GetOrderDetailsByOrderCode(
                    orderCode,
                    out int response);

                // Kiểm tra kết quả
                if (response != 200) // Nếu không thành công
                {
                    return StatusCode(response >= 400 && response < 500 ? response : 500,
                        new
                        {
                            ResponseCode = response,
                            Message = MessageUtils.GetMessage(response, _langCode)
                        });
                }

                // Kiểm tra nếu không có dữ liệu
                if (result == null || result.Tables.Count == 0)
                {
                    return NotFound(new
                    {
                        ResponseCode = 404,
                        Message = "Không tìm thấy thông tin đơn hàng"
                    });
                }

                // Định dạng dữ liệu trả về
                var orderDetails = new
                {
                    ResponseCode = response,
                    Message = "Lấy thông tin đơn hàng thành công",
                    OrderInfo = result.Tables[0].Rows.Count > 0 ? FormatOrderInfo(result.Tables[0]) : null,
                    MovieShowtimeInfo = result.Tables.Count > 1 && result.Tables[1].Rows.Count > 0 ? FormatMovieShowtimeInfo(result.Tables[1]) : null,
                    SeatDetails = result.Tables.Count > 2 && result.Tables[2].Rows.Count > 0 ? FormatSeatDetails(result.Tables[2]) : null,
                    ServiceDetails = result.Tables.Count > 3 && result.Tables[3].Rows.Count > 0 ? FormatServiceInfo(result.Tables[3]) : null
                };

                return Ok(orderDetails);
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

        // Format thông tin đơn hàng (Table 0)
        private dynamic FormatOrderInfo(DataTable orderTable)
        {
            var row = orderTable.Rows[0];
            return new
            {
                OrderId = row["OrderId"].ToString(),
                OrderCode = row["OrderCode"].ToString(),
                Email = row["Email"].ToString(),
                TotalPrice = Convert.ToInt64(row["TotalPrice"]),
                Status = Convert.ToInt32(row["Status"]),
                IsAnonymous = Convert.ToBoolean(row["IsAnonymous"]),
                DiscountPrice = Convert.ToInt64(row["DiscountPrice"]),
                CreatedDate = Convert.ToDateTime(row["OrderCreatedDate"]),
                UpdatedDate = row["OrderUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(row["OrderUpdatedDate"]) : (DateTime?)null,
                UserInfo = row["IsAnonymous"].ToString() == "True" ? null : new
                {
                    UserName = row["UserName"].ToString(),
                    FullName = row["FullName"].ToString(),
                    PhoneNumber = row["PhoneNumber"].ToString(),
                    Address = row["Address"].ToString(),
                    Dob = row["Dob"] != DBNull.Value ? Convert.ToDateTime(row["Dob"]) : (DateTime?)null
                },
                FormattedTotalPrice = _ultils.FormatMoney(Convert.ToInt64(row["TotalPrice"])),
                FormattedDiscountPrice = _ultils.FormatMoney(Convert.ToInt64(row["DiscountPrice"])),
                FormattedCreatedDate = Convert.ToDateTime(row["OrderCreatedDate"]).ToString("dd/MM/yyyy HH:mm"),
                FormattedUpdatedDate = row["OrderUpdatedDate"] != DBNull.Value ?
                    Convert.ToDateTime(row["OrderUpdatedDate"]).ToString("dd/MM/yyyy HH:mm") : null
            };
        }

        // Format thông tin phim và suất chiếu (Table 1)
        private dynamic FormatMovieShowtimeInfo(DataTable movieTable)
        {
            var row = movieTable.Rows[0];
            return new
            {
                MovieId = row["MovieId"].ToString(),
                MovieName = row["MovieName"].ToString(),
                MovieDescription = row["MovieDescription"].ToString(),
                Duration = Convert.ToInt32(row["MovieDuration"]),
                Thumbnail = row["Thumbnail"].ToString(),
                Banner = row["Banner"].ToString(),
                Trailer = row["Trailer"].ToString(),
                AgeRatingCode = row["AgeRatingCode"].ToString(),
                AverageRating = row["AverageRating"] != DBNull.Value ? Convert.ToDouble(row["AverageRating"]) : 0,
                ShowTimeInfo = new
                {
                    ShowTimeId = row["ShowTimeId"].ToString(),
                    StartTime = Convert.ToDateTime(row["StartTime"]),
                    EndTime = Convert.ToDateTime(row["EndTime"]),
                    Status = Convert.ToInt32(row["ShowTimeStatus"]),
                    FormattedStartTime = Convert.ToDateTime(row["StartTime"]).ToString("dd/MM/yyyy HH:mm"),
                    FormattedEndTime = Convert.ToDateTime(row["EndTime"]).ToString("HH:mm"),
                    FormattedDuration = $"{Convert.ToInt32(row["MovieDuration"])} phút"
                },
                RoomInfo = new
                {
                    RoomId = row["RoomId"].ToString(),
                    RoomName = row["Name"].ToString(),
                    RoomType = row["RoomTypeName"].ToString()
                },
                CinemaInfo = new
                {
                    CinemaId = row["CinemaId"].ToString(),
                    CinemaName = row["CinemaName"].ToString(),
                    Address = row["CinemaAddress"].ToString(),
                    PhoneNumber = row["CinemaPhoneNumber"].ToString()
                }
            };
        }

        // Format thông tin ghế (Table 2)
        private List<dynamic> FormatSeatDetails(DataTable seatTable)
        {
            var result = new List<dynamic>();

            foreach (DataRow row in seatTable.Rows)
            {
                result.Add(new
                {
                    TicketId = row["TicketId"].ToString(),
                    TicketCode = row["TickeCode"].ToString(),
                    SeatInfo = new
                    {
                        SeatId = row["SeatId"].ToString(),
                        SeatName = row["SeatName"].ToString(),
                        RowNumber = row["RowNumber"].ToString(),
                        ColumnNumber = row["ColNumber"].ToString(),
                        SeatType = row["SeatTypeName"].ToString(),
                        BasePrice = Convert.ToInt64(row["BasePrice"]),
                        ActualPrice = Convert.ToInt64(row["ActualPrice"]),
                        Status = Convert.ToInt32(row["SeatStatus"]),
                        FormattedBasePrice = _ultils.FormatMoney(Convert.ToInt64(row["BasePrice"])),
                        FormattedActualPrice = _ultils.FormatMoney(Convert.ToInt64(row["ActualPrice"]))
                    }
                });
            }

            return result;
        }

        // Format thông tin dịch vụ (Table 3)
        private List<dynamic> FormatServiceInfo(DataTable serviceTable)
        {
            var result = new List<dynamic>();

            foreach (DataRow row in serviceTable.Rows)
            {
                result.Add(new
                {
                    OrderServiceId = row["OrderServiceId"].ToString(),
                    ServiceId = row["ServiceId"].ToString(),
                    ServiceName = row["ServiceName"].ToString(),
                    Description = row["ServiceDescription"].ToString(),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                    UnitPrice = Convert.ToInt64(row["UnitPrice"]),
                    TotalPrice = Convert.ToInt64(row["TotalPrice"]),
                    ServiceType = row["ServiceTypeName"].ToString(),
                    FormattedUnitPrice = _ultils.FormatMoney(Convert.ToInt64(row["UnitPrice"])),
                    FormattedTotalPrice = _ultils.FormatMoney(Convert.ToInt64(row["TotalPrice"]))
                });
            }

            return result;
        }





    }
}