using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Constants;
using DATN_Models.DAL.Seat;
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
        private string userId = Guid.NewGuid().ToString();
        private Dictionary<string, SeatStatusEnum> initialSeatStatuses = new Dictionary<string, SeatStatusEnum>();
        private CancellationTokenSource countdownCancellationTokenSource;
        private static Dictionary<string, List<SeatStatusUpdateRequest>> userSeatUpdates = new Dictionary<string, List<SeatStatusUpdateRequest>>();
        private static Dictionary<string, bool> userPaymentStatus = new Dictionary<string, bool>();

        public SeatStatusShowHandler(WebSocket webSocket, IWebSocketManager webSocketManager, IMapper mapper, SeatStatusService seatStatusService, ISeatDAO seatDAO)
        {
            _webSocket = webSocket;
            _webSocketManager = webSocketManager;
            _mapper = mapper;
            _seatStatusService = seatStatusService;
            _seatDAO = seatDAO;
        }

        public async Task HandleRequestAsync(string hub, Guid roomId)
        {
            Console.WriteLine($"[WebSocket] New client connected: Hub={hub}, UserId={userId}");

            await _webSocketManager.AddUserSocketAsync(hub, userId, _webSocket);

            var buffer = new byte[1024 * 4];
            while (_webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Console.WriteLine($"[WebSocket] Client disconnected: {userId}");
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        await _webSocketManager.RemoveUserSocketAsync(hub, userId);
                        if (userSeatUpdates.ContainsKey(userId))
                        {
                            userSeatUpdates.Remove(userId);
                        }
                        countdownCancellationTokenSource?.Cancel();
                        break;
                    }
                    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var request = JsonConvert.DeserializeObject<SeatActionRequest>(receivedMessage);

                    if (request != null)
                    {
                        Console.WriteLine($"[WebSocket] Received action: {request.Action}");
                        switch (request.Action)
                        {
                            case "GetList":
                                var seatList = GenerateSeatList(roomId);
                                await SendMessageToClient(seatList);
                                break;

                            case "UpdateStatus":
                                Console.WriteLine("[WebSocket] UpdateStatus action received.");
                                if (request.SeatStatusUpdateRequests != null)
                                {
                                    await HandleUpdateStatusAction(request.SeatStatusUpdateRequests, hub, roomId);
                                }
                                break;

                            case "HoldSeat":
                                await StartCountdownAsync(hub, roomId, userId);
                                break;

                            case "Paymented":
                                await CompletePaymentAsync(userId);
                                break;

                            default:
                                Console.WriteLine($"[WebSocket] Unknown action received: {request.Action}");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[WebSocket] Error: {ex.Message}");
                    await SendErrorMessage($"An error occurred: {ex.Message}");
                }
            }
        }

        private async Task StartCountdownAsync(string hub, Guid roomId, string userId)
        {
            countdownCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = countdownCancellationTokenSource.Token;

            // Store original statuses before countdown
            var originalStatuses = new Dictionary<string, SeatStatusEnum>();
            if (userSeatUpdates.ContainsKey(userId))
            {
                foreach (var update in userSeatUpdates[userId])
                {
                    var seatGuid = Guid.Parse(update.SeatId);
                    originalStatuses[seatGuid.ToString()] = _seatStatusService.GetSeatStatus(seatGuid)?.Status ?? SeatStatusEnum.UnAvailable;
                }
            }

            for (int i = 60; i >= 0; i--)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var countdownData = new { Countdown = i };
                await SendSeatsCountdownToClient(roomId, hub, countdownData);

                await Task.Delay(1000, cancellationToken);
            }

            // After countdown, check if payment is completed
            if (!userPaymentStatus.ContainsKey(userId) || !userPaymentStatus[userId])
            {
                // Revert statuses if payment is not completed
                if (userSeatUpdates.ContainsKey(userId))
                {
                    foreach (var update in userSeatUpdates[userId])
                    {
                        var seatGuid = Guid.Parse(update.SeatId);
                        if (originalStatuses.ContainsKey(update.SeatId))
                        {
                            _seatStatusService.AddOrUpdateSeatStatus(seatGuid, originalStatuses[update.SeatId]);
                        }
                    }
                }
            }
        }

        public async Task CompletePaymentAsync(string userId)
        {
            if (!userPaymentStatus.ContainsKey(userId))
            {
                userPaymentStatus[userId] = true; // Mark payment as completed for the user
            }

            // Optionally, inform the user about payment success
            await SendMessageToClient(new { Message = "Payment completed successfully!" });
        }

        private async Task SendSeatsCountdownToClient(Guid roomId, string hub, object countdownData)
        {
            var responseJson = JsonConvert.SerializeObject(countdownData);
            await _webSocketManager.SendMessageToUserAsync(hub, userId, responseJson);
        }

        private async Task HandleUpdateStatusAction(List<SeatStatusUpdateRequest> seatStatusUpdateRequests, string hub, Guid roomId)
        {
            if (!userSeatUpdates.ContainsKey(userId))
            {
                userSeatUpdates[userId] = new List<SeatStatusUpdateRequest>();
            }

            foreach (var updateRequest in seatStatusUpdateRequests)
            {
                if (updateRequest.SeatId != null)
                {
                    var seatGuid = Guid.Parse(updateRequest.SeatId);
                    var currentStatus = _seatStatusService.GetSeatStatus(seatGuid);

                    if (currentStatus == null || currentStatus.Status != updateRequest.Status)
                    {
                        _seatStatusService.AddOrUpdateSeatStatus(seatGuid, updateRequest.Status);
                        userSeatUpdates[userId].Add(updateRequest);
                    }
                    else
                    {
                        Console.WriteLine($"[WebSocket] No update needed for SeatId={seatGuid} as status is already {currentStatus.Status}");
                    }
                }
            }

            var seatListForOthers = GenerateSeatList(roomId);
            await SendSeatsToOthers(roomId, hub, seatListForOthers);
            await SendUpdatedStatusToClient(roomId, hub, userSeatUpdates[userId]);
        }

        private object GenerateSeatList(Guid roomId, List<SeatStatusUpdateRequest>? updatedSeats = null)
        {
            var seats = _seatStatusService.GenerateSeats(roomId);

            var modifiedSeats = seats.Select(seat =>
            {
                var seatStatus = _seatStatusService.GetSeatStatus(seat.Id);
                if (seatStatus != null)
                {
                    if (updatedSeats != null)
                    {
                        var updatedSeat = updatedSeats.FirstOrDefault(us => us.SeatId == seat.Id.ToString());
                        if (updatedSeat != null)
                        {
                            seat.Status = updatedSeat.Status;
                        }
                        else if (_seatStatusService.GetHeldSeatsByUser(userId).Contains(seat.Id.ToString()))
                        {
                            seat.Status = seatStatus.Status;
                        }
                        else
                        {
                            seat.Status = seatStatus.Status == SeatStatusEnum.UnAvailable ? SeatStatusEnum.Reserved : seatStatus.Status; // Reserved for others
                        }
                    }
                    else
                    {
                        if (_seatStatusService.GetHeldSeatsByUser(userId).Contains(seat.Id.ToString()))
                        {
                            seat.Status = seatStatus.Status;
                        }
                        else
                        {
                            seat.Status = seatStatus.Status == SeatStatusEnum.UnAvailable ? SeatStatusEnum.Reserved : seatStatus.Status; // Reserved for others
                        }
                    }
                }
                return seat;
            }).ToList();

            return new { Seats = modifiedSeats };
        }

        private async Task SendMessageToClient(object message)
        {
            var responseJson = JsonConvert.SerializeObject(message);
            var buffer = Encoding.UTF8.GetBytes(responseJson);
            await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task SendErrorMessage(string error)
        {
            var errorResponse = new { Error = error };
            await SendMessageToClient(errorResponse);
        }

        private async Task SendUpdatedStatusToClient(Guid roomId, string hub, List<SeatStatusUpdateRequest> updatedSeats)
        {
            var seatList = GenerateSeatList(roomId, updatedSeats);

            var responseJson = JsonConvert.SerializeObject(seatList, Formatting.Indented);
            var buffer = Encoding.UTF8.GetBytes(responseJson);
            await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task SendSeatsToOthers(Guid roomId, string hub, object seatList)
        {
            var responseJson = JsonConvert.SerializeObject(seatList);
            await _webSocketManager.SendMessageToAllExceptUserAsync(hub, userId, responseJson);
        }

        public class SeatActionRequest
        {
            public string Action { get; set; } = string.Empty;
            public string? SeatId { get; set; }
            public List<SeatStatusUpdateRequest>? SeatStatusUpdateRequests { get; set; }
        }

        public class SeatStatusUpdateRequest
        {
            public string SeatId { get; set; }
            public SeatStatusEnum Status { get; set; }
        }
    }
}
