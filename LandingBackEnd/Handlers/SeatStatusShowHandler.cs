using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Constants;
using DATN_Models.DAL.Seat;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Seat.Req;
using DATN_Services.WebSockets;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace DATN_LandingPage.Handlers
{
    public class SeatStatusShowHandler
    {
        private readonly WebSocket _webSocket;
        private readonly IWebSocketManager _webSocketManager;
        private readonly IMapper _mapper;
        private readonly SeatStatusService _seatStatusService;
        private readonly ISeatDAO _seatDAO;

        public SeatStatusShowHandler(WebSocket webSocket, IWebSocketManager webSocketManager, IMapper mapper, SeatStatusService seatStatusService, ISeatDAO seatDAO)
        {
            _webSocket = webSocket;
            _webSocketManager = webSocketManager;
            _mapper = mapper;
            _seatStatusService = seatStatusService;
            _seatDAO = seatDAO;
        }

        public async Task HandleUpdateSeatStatusAsync(List<SeatStatusUpdateRequest> seatStatusUpdateRequests, string hub)
        {
            try
            {
                foreach (var updateRequest in seatStatusUpdateRequests)
                {
                    if (updateRequest.SeatId != null)
                    {
                        // Cập nhật trạng thái ghế trong cơ sở dữ liệu
                        var updateStatus = new UpdateSeatByShowTimeStatusReq
                        {
                            Id = Guid.Parse(updateRequest.SeatId),
                            Status = updateRequest.Status
                        };
                        var reqMapper = _mapper.Map<UpdateSeatByShowTimeStatusDAL>(updateStatus);
                        _seatDAO.UpdateSeatByShowTimeStatus(reqMapper, out int responseCode);
                        if (responseCode != 200)
                        {
                            await SendErrorMessage("Failed to update seat status.");
                            return;
                        }

                        // Cập nhật trạng thái trong lớp dịch vụ
                        _seatStatusService.AddOrUpdateSeatStatus(Guid.Parse(updateRequest.SeatId), updateRequest.Status);
                    }

                    // Thông báo cho các khách hàng kết nối
                    await SendStatusUpdate(updateRequest.SeatId, (int)updateRequest.Status, hub);

                    // Bắt đầu quá trình chờ và hủy sau 30 giây nếu không có hành động nào khác
                    if (updateRequest.SeatId != null && updateRequest.Status == SeatStatusEnum.UnAvailable)
                    {
                        _ = WaitAndCancelSeat(updateRequest.SeatId, hub); // Fire and forget approach
                    }
                }
            }
            catch (Exception ex)
            {
                await SendErrorMessage($"An error occurred: {ex.Message}");
            }
        }


        public async Task ReceiveMessages(string hub, string userId)
        {
            var buffer = new byte[1024 * 4];
            while (_webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        await _webSocketManager.RemoveUserSocketAsync(hub, userId);
                        break;
                    }

                    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var updateRequests = JsonConvert.DeserializeObject<List<SeatStatusUpdateRequest>>(receivedMessage);

                    if (updateRequests != null)
                    {
                        await HandleUpdateSeatStatusAsync(updateRequests, hub);
                    }
                }
                catch (WebSocketException wsEx)
                {
                    await SendErrorMessage($"WebSocket error: {wsEx.Message}");
                    await _webSocketManager.RemoveUserSocketAsync(hub, userId);
                    break; // Exit the loop if a WebSocketException occurs
                }
                catch (Exception ex)
                {
                    await SendErrorMessage($"An error occurred while receiving messages: {ex.Message}");
                    await _webSocketManager.RemoveUserSocketAsync(hub, userId);
                    break; // Exit the loop if a general exception occurs
                }
            }
        }

        private async Task WaitAndCancelSeat(string seatId, string hub)
        {
            try
            {
                await Task.Delay(30 * 1000); // Chờ 30 giây

                var currentStatus = _seatDAO.GetStatusById(Guid.Parse(seatId), out int response);

                if (currentStatus.Status != SeatStatusEnum.UnAvailable) return;

                _seatDAO.UpdateSeatByShowTimeStatus(new UpdateSeatByShowTimeStatusDAL
                {
                    Id = Guid.Parse(seatId),
                    Status = SeatStatusEnum.Available
                }, out int responseCode);

                if (responseCode == 200)
                {
                    await SendStatusUpdate(seatId, (int)SeatStatusEnum.Available, hub);
                }
                else
                {
                    await SendErrorMessage("Failed to cancel seat reservation after 30 seconds.");
                }
            }
            catch (Exception ex)
            {
                await SendErrorMessage($"An error occurred during the cancellation process: {ex.Message}");
            }
        }

        private async Task SendStatusUpdate(string seatId, int status, string hub)
        {
            try
            {
                var response = new
                {
                    SeatId = seatId,
                    Status = status,
                    Message = "Seat status updated successfully"
                };

                var responseMessage = JsonConvert.SerializeObject(response);
                var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);

                // Send message to all clients in the specified hub
                await _webSocketManager.SendMessageToAllUserAsync(hub, responseMessage);

                // Send message back to the client who initiated the request
                await _webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                await SendErrorMessage($"Failed to send status update: {ex.Message}");
            }
        }
        private async Task SendErrorMessage(string error)
        {
            var errorResponse = new
            {
                Error = error
            };

            var errorMessage = JsonConvert.SerializeObject(errorResponse);
            var errorBuffer = Encoding.UTF8.GetBytes(errorMessage);
            await _webSocket.SendAsync(new ArraySegment<byte>(errorBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public class SeatStatusUpdateRequest
    {
        public string SeatId { get; set; }
        public SeatStatusEnum Status { get; set; }
    }
}
