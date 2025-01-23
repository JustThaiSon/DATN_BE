using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Constants;
using DATN_Models.DAL.Seat;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Models.DTOS.Seat.Req;
using DATN_Services.WebSockets;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace DATN_BackEndApi.Handlers
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

        public async Task HandleUpdateSeatStatusAsync(string seatId, int status, string hub)
        {
            try
            {
                // Update seat status in the database
                var updateStatus = new UpdateSeatByShowTimeStatusReq
                {
                    Id = Guid.Parse(seatId),
                    Status = (SeatStatusEnum)status
                };
                var reqMapper = _mapper.Map<UpdateSeatByShowTimeStatusDAL>(updateStatus);
                _seatDAO.UpdateSeatByShowTimeStatus(reqMapper, out int responseCode);
                if (responseCode != 200)
                {
                    await SendErrorMessage("Failed to update seat status.");
                    return;
                }

                // Update status in the service layer
                _seatStatusService.AddOrUpdateSeatStatus(Guid.Parse(seatId), (SeatStatusEnum)status);

                // Notify connected clients
                await SendStatusUpdate(seatId, status, hub);

                // Start the wait and cancel process after 30 seconds if no further action is taken
                _ = WaitAndCancelSeat(seatId); // Fire and forget approach
            }
            catch (Exception ex)
            {
                await SendErrorMessage($"An error occurred: {ex.Message}");
            }
        }

        public async Task ReceiveMessages(string hub, string seatId)
        {
            var buffer = new byte[1024 * 4];

            while (_webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnected", CancellationToken.None);
                        await _webSocketManager.RemoveUserSocketAsync(hub, seatId);
                        break;
                    }

                    // Process received message
                    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var updateRequest = JsonConvert.DeserializeObject<SeatStatusUpdateRequest>(receivedMessage);

                    if (updateRequest != null)
                    {
                        await HandleUpdateSeatStatusAsync(updateRequest.SeatId, (int)updateRequest.Status, hub);
                    }
                }
                catch (Exception ex)
                {
                    await SendErrorMessage($"An error occurred while receiving messages: {ex.Message}");
                }
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
            }
            catch (Exception ex)
            {
                await SendErrorMessage($"Failed to send status update: {ex.Message}");
            }
        }

        private async Task WaitAndCancelSeat(string seatId)
        {
            try
            {
                await Task.Delay(30 * 1000); // Wait for 30 seconds

                var currentStatus = _seatDAO.GetStatusById(Guid.Parse(seatId), out int response);

                if (currentStatus.Status != SeatStatusEnum.UnAvailable) return;

                _seatDAO.UpdateSeatByShowTimeStatus(new UpdateSeatByShowTimeStatusDAL
                {
                    Id = Guid.Parse(seatId),
                    Status = SeatStatusEnum.Available
                }, out int responseCode);

                if (responseCode == 200)
                {
                    await SendStatusUpdate(seatId, (int)SeatStatusEnum.Available, "seatHub");
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
