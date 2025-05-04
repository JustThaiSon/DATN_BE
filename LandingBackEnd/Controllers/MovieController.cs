using AutoMapper;
using DATN_BackEndApi.VNPay;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Constants;
using DATN_Helpers.Extensions;
using DATN_LandingPage.Extension;
using DATN_LandingPage.Handlers;
using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Orders;
using DATN_Models.DAL.Service;
using DATN_Models.DAL.ServiceType;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.Order.Req;
using DATN_Models.DTOS.Order.Res;
using DATN_Models.DTOS.Seat.Res;
using DATN_Models.DTOS.SeatType.Res;
using DATN_Models.DTOS.Service.Response;
using DATN_Models.DTOS.ServiceType.Res;
using DATN_Services.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace DATN_LandingPage.Controllers
{
    //[BAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieDAO _movieDAO;
        private readonly ISeatDAO _seatDAO;
        private readonly IServiceDAO _serviceDAO;
        private readonly IServiceTypeDAO _serviceTypeDAO;
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IOrderDAO _orderDAO;
        private readonly IVNPayService _vnPayService;

        public MovieController(IMovieDAO movieDAO, IConfiguration configuration, IUltil ultils, IMapper mapper, IMailService mailService, ISeatDAO seatDAO, IServiceDAO serviceDAO, IOrderDAO orderDAO, IVNPayService vnPayService, IServiceTypeDAO seatTypeDAO)
        {
            _movieDAO = movieDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _mailService = mailService;
            _seatDAO = seatDAO;
            _serviceDAO = serviceDAO;
            _orderDAO = orderDAO;
            _vnPayService = vnPayService;
            _serviceTypeDAO = seatTypeDAO;
        }
        [HttpGet]
        [Route("GetMovie")]
        public async Task<CommonPagination<List<GetMovieLandingRes>>> GetMovie(Guid? movieType, int type, int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetMovieLandingRes>>();
            var data = _movieDAO.GetMovieLanding(movieType, type, currentPage, recordPerPage, out int totalRecord, out int responseCode);
            var resultMapper = _mapper.Map<List<GetMovieLandingRes>>(data);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            res.TotalRecord = totalRecord;
            return res;
        }
        [HttpGet]
        [Route("GetDetailMovie")]
        public async Task<CommonResponse<GetMovieRes>> GetDetailMovie(Guid movieId)
        {
            var res = new CommonResponse<GetMovieRes>();
            var data = _movieDAO.GetMovieDetail(movieId, out int responseCode);
            var resultMapper = _mapper.Map<GetMovieRes>(data);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            return res;
        }
        [HttpGet]
        [Route("GetShowTimeLanding")]
        public async Task<CommonPagination<List<GetShowTimeLangdingRes>>> GetShowTimeLanding(Guid? cinemaId, Guid? movieId, string? location, DateTime? date, int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetShowTimeLangdingRes>>();
            var data = _movieDAO.GetShowTimeLanding(cinemaId, movieId, location, date, currentPage, recordPerPage, out int totalRecord, out int responseCode);
            var resultMapper = _mapper.Map<List<GetShowTimeLangdingRes>>(data);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            res.TotalRecord = totalRecord;
            return res;
        }
        [HttpGet]
        [Route("GetService")]
        public async Task<CommonPagination<List<GetServiceRes>>> GetService(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetServiceRes>>();
            var result = _serviceDAO.GetService(currentPage, recordPerPage, out int totalRecord, out int response);
            var resultMapper = _mapper.Map<List<GetServiceRes>>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;
            return res;
        }
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<CommonResponse<string>> CreateOrder(CreateOrderReq req)
        {
            var res = new CommonResponse<string>();
            var reqpMapper = _mapper.Map<CreateOrderDAL>(req);
            var result = _orderDAO.CreateOrder(reqpMapper, out int responseCode);
            var resultMapper = _mapper.Map<OrderMailResultRes>(result);
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.ResponseCode = responseCode;
            if (resultMapper != null)
            {
                await _mailService.SendQrCodeEmail(resultMapper);
            }
            return res;
        }
        [HttpGet]
        [Route("GetAllNameMovie")]
        public async Task<CommonResponse<List<GetAllNameMovieRes>>> GetAllNameMovie()
        {
            var res = new CommonResponse<List<GetAllNameMovieRes>>();
            var data = _movieDAO.GetAllNameMovie(out int responseCode);
            var resultMapper = _mapper.Map<List<GetAllNameMovieRes>>(data);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            return res;
        }
        [HttpGet]
        [Route("GetSeatByShowTime")]
        public async Task<CommonResponse<List<GetSeatByShowTimeRes>>> GetSeatByShowTime(Guid showTimeId)
        {
            var res = new CommonResponse<List<GetSeatByShowTimeRes>>();
            var data = _seatDAO.GetSeatByShowTime(showTimeId, out int totalRecord, out int responseCode);
            var resultMapper = _mapper.Map<List<GetSeatByShowTimeRes>>(data);
            res.ResponseCode = responseCode;
            res.Message = MessageUtils.GetMessage(responseCode, _langCode);
            res.Data = resultMapper;
            return res;
        }


        [HttpGet("GetMovieGenres")]
        public async Task<CommonResponse<List<MovieGenreRes>>> GetMovieGenres(Guid id)
        {
            var res = new CommonResponse<List<MovieGenreRes>>();

            var result = _movieDAO.GetMovieGenres(id, out int response);
            var resultMapper = _mapper.Map<List<MovieGenreRes>>(result);

            res.Data = resultMapper;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }
        [HttpGet]
        [Route("GetServiceTypeList")]
        public async Task<CommonPagination<List<ServiceTypeRes>>> GetServiceTypeList(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<ServiceTypeRes>>();
            var result = _serviceTypeDAO.GetServiceTypeList(currentPage, recordPerPage, out int totalRecord, out int response);
            var resultMapper = _mapper.Map<List<ServiceTypeRes>>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;
            return res;
        }

        [HttpGet]
        [Route("GetMovieByShowTime")]
        public async Task<CommonResponse<GetMovieByShowTimeRes>> GetMovieByShowTime(Guid showtimeId)
        {
            var res = new CommonResponse<GetMovieByShowTimeRes>();
            var result = _movieDAO.GetMovieByShowTime(showtimeId, out int response);
            var resultMapper = _mapper.Map<GetMovieByShowTimeRes>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpGet]
        [Route("GetPayment")]
        public async Task<CommonResponse<List<GetPaymentRes>>> GetPayment()
        {
            var res = new CommonResponse<List<GetPaymentRes>>();
            var result = _orderDAO.GetPayment(out int response);
            var resultMapper = _mapper.Map<List<GetPaymentRes>>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost]
        [Route("create-payment")]
        public async Task<CommonResponse<string>> CreatePayment([FromBody] OrderInfo orderInfo)
        {
            var res = new CommonResponse<string>();
            try
            {
                var paymentUrl = _vnPayService.CreatePaymentUrl(orderInfo, HttpContext);
                res.Data = paymentUrl;
                res.ResponseCode = 1;
                res.Message = "Success";
            }
            catch (Exception ex)
            {
                res.ResponseCode = -99;
                res.Message = ex.Message;
            }
            return res;
        }
        [HttpGet]
        [Route("payment-callback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.ProcessPaymentCallback(Request.Query);
            // Redirect back to Angular with payment result
            var redirectUrl = "http://localhost:4200/payment-callback";
            var queryString = $"?vnp_ResponseCode={response.VnPayResponseCode}&vnp_TxnRef={response.OrderId}";
            return Redirect(redirectUrl + queryString);
        }
        [BAuthorize]
        [HttpGet]
        [Route("GetListHistoryOrderByUser")]
        public async Task<CommonPagination<List<GetListHistoryOrderByUserRes>>> GetListHistoryOrderByUser(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetListHistoryOrderByUserRes>>();
            var userId = HttpContextHelper.GetUserId();
            var result = _orderDAO.GetListHistoryOrderByUser(userId, currentPage, recordPerPage, out int totalRecord, out int response);
            res.Data = result;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.TotalRecord = totalRecord;
            return res;
        }
        [BAuthorize]
        [HttpGet]
        [Route("GetPastShowTimesByTimeFilter")]
        public async Task<CommonPagination<List<GetListHistoryOrderByUserRes>>> GetPastShowTimesByTimeFilter(string filter, int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetListHistoryOrderByUserRes>>();
            var userId = HttpContextHelper.GetUserId();
            var result = _orderDAO.GetPastShowTimesByTimeFilter(userId, filter, currentPage, recordPerPage, out int totalRecord, out int response);
            res.Data = result;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.TotalRecord = totalRecord;
            return res;
        }
        [BAuthorize]
        [HttpGet]
        [Route("GetOrderDetailLangding")]
        public async Task<CommonResponse<GetOrderDetailLangdingRes>> GetOrderDetailLangdingRes(Guid orderId)
        {
            var res = new CommonResponse<GetOrderDetailLangdingRes>();
            var result = _orderDAO.GetOrderDetailLangding(orderId, out int response);
            var resultMapper = _mapper.Map<GetOrderDetailLangdingRes>(result);
            resultMapper.OrderCodeB64 = _mailService.GenerateQrCode(resultMapper.OrderCode);
            res.Data = resultMapper;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            return res;
        }
        [HttpGet]
        [Route("CheckRefund")]
        public async Task<CommonResponse<CheckRefundRes>> CheckRefund(Guid orderId)
        {
            var res = new CommonResponse<CheckRefundRes>();
            var result = _orderDAO.CheckRefund(orderId, out int response);
            var resultMapper = _mapper.Map<CheckRefundRes>(result);
            res.Data = resultMapper;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            return res;
        }
        [HttpGet]
        [Route("GetCinemaByLocation")]
        public async Task<CommonResponse<List<GetCinemaByLocationRes>>> GetCinemaByLocation(string? location)
        {
            var res = new CommonResponse<List<GetCinemaByLocationRes>>();
            var result = _movieDAO.GetCinemaByLocation(location, out int response);
            res.Data = result;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            return res;
        }

        [HttpGet]
        [Route("GetCinemaAll")]
        public async Task<CommonResponse<List<GetCinemaByLocationRes>>> GetCinemaAll()
        {
            var res = new CommonResponse<List<GetCinemaByLocationRes>>();
            var result = _movieDAO.GetCinemaAll(out int response);
            res.Data = result;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            return res;
        }
        [HttpGet]
        [Route("GetMovieType")]
        public async Task<CommonResponse<List<GetMovieTypeRes>>> GetMovieType()
        {
            var res = new CommonResponse<List<GetMovieTypeRes>>();
            var result = _movieDAO.GetMovieType(out int response);
            res.Data = result;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            return res;
        }
        [BAuthorize]
        [HttpPost]
        [Route("RefundOrder")]
        public async Task<CommonResponse<GetInfoRefundRes>> RefundOrder(RefundOrderByIdReq req)
        {
            var res = new CommonResponse<GetInfoRefundRes>();
            var result = _orderDAO.RefundOrderById(req, out int response);

            if (result != null && response == 200)
            {
                if (result.Email != null)
                {
                    await _mailService.SendMailRefund(result);
                }

                var userId = HttpContextHelper.GetUserId();

                List<Guid> seatStatusByShowTimeIds = result.SeatStatusByShowTimeIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => Guid.Parse(id.Trim()))
                    .ToList();

                using (var webSocket = new ClientWebSocket())
                {
                    try
                    {
                        var uri = new Uri($"wss://localhost:7105/ws/KeepSeat?roomId={result.ShowTimeId}&userId={userId}");
                        await webSocket.ConnectAsync(uri, CancellationToken.None);

                        // Prepare the seat status update requests
                        var seatStatusUpdateRequests = seatStatusByShowTimeIds
                            .Select(seatId => new SeatStatusShowHandler.SeatStatusUpdateRequest
                            {
                                SeatId = seatId.ToString(),
                                Status = SeatStatusEnum.Available
                            })
                            .ToList();

                        var refundRequest = new SeatStatusShowHandler.SeatActionRequest
                        {
                            Action = "Refund",
                            SeatStatusUpdateRequests = seatStatusUpdateRequests
                        };

                        // Send the refund request via WebSocket
                        var message = JsonConvert.SerializeObject(refundRequest);
                        var buffer = Encoding.UTF8.GetBytes(message);
                        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

                        var responseBuffer = new byte[1024];
                        var webSocketResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(responseBuffer), CancellationToken.None);
                        var responseMessage = Encoding.UTF8.GetString(responseBuffer, 0, webSocketResult.Count);
                        Console.WriteLine($"WebSocket Response: {responseMessage}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"WebSocket Error: {ex.Message}");
                    }
                    finally
                    {
                        // Ensure the WebSocket is properly closed
                        if (webSocket.State == WebSocketState.Open)
                        {
                            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Operation completed", CancellationToken.None);
                        }
                    }
                }
            }

            res.ResponseCode = response;
            res.Data = result;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            return res;
        }
        [BAuthorize]
        [HttpPost]
        [Route("RefundByShowtime")]
        public async Task<CommonResponse<List<GetInfoRefundRes>>> RefundByShowtime(RefundByShowtimeReq req)
        {
            var res = new CommonResponse<List<GetInfoRefundRes>>();
            var result = _orderDAO.RefundByShowtime(req, out int response);
            if (result != null && response == 200)
            {
                var userId = HttpContextHelper.GetUserId();
                foreach (var item in result)
                {
                    if (item.Email != null)
                    {
                        await _mailService.SendMailRefundAll(item);
                    }

                    List<Guid> seatStatusByShowTimeIds = item.SeatStatusByShowTimeIds
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => Guid.Parse(id.Trim()))
                        .ToList();

                    using (var webSocket = new ClientWebSocket())
                    {
                        try
                        {
                            var uri = new Uri($"wss://localhost:7105/ws/KeepSeat?roomId={item.ShowTimeId}&userId={userId}");
                            await webSocket.ConnectAsync(uri, CancellationToken.None);

                            var seatStatusUpdateRequests = seatStatusByShowTimeIds
                                .Select(seatId => new SeatStatusShowHandler.SeatStatusUpdateRequest
                                {
                                    SeatId = seatId.ToString(),
                                    Status = SeatStatusEnum.Available
                                })
                                .ToList();

                            var refundRequest = new SeatStatusShowHandler.SeatActionRequest
                            {
                                Action = "Refund",
                                SeatStatusUpdateRequests = seatStatusUpdateRequests
                            };

                            var message = JsonConvert.SerializeObject(refundRequest);
                            var buffer = Encoding.UTF8.GetBytes(message);
                            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

                            var responseBuffer = new byte[1024];
                            var webSocketResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(responseBuffer), CancellationToken.None);
                            var responseMessage = Encoding.UTF8.GetString(responseBuffer, 0, webSocketResult.Count);
                            Console.WriteLine($"WebSocket Response: {responseMessage}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"WebSocket Error: {ex.Message}");
                        }
                        finally
                        {
                            if (webSocket.State == WebSocketState.Open)
                            {
                                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Operation completed", CancellationToken.None);
                            }
                        }
                    }
                }
            }

            res.Data = result;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            return res;
        }
    }
}
