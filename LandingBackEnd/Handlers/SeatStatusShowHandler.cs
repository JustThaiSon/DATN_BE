using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Constants;
using DATN_Models.DAO.Interface;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Services.WebSockets;
using Newtonsoft.Json;
using System.Collections.Concurrent;
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
        private CancellationTokenSource countdownCancellationTokenSource = new CancellationTokenSource();
        private static ConcurrentDictionary<string, List<SeatStatusUpdateRequest>> userSeatUpdates = new();
        private static ConcurrentDictionary<string, bool> userPaymentStatus = new ConcurrentDictionary<string, bool>();
        private string currentUserId;

        public SeatStatusShowHandler(WebSocket webSocket, IWebSocketManager webSocketManager, IMapper mapper, SeatStatusService seatStatusService, ISeatDAO seatDAO)
        {
            _webSocket = webSocket;
            _webSocketManager = webSocketManager;
            _mapper = mapper;
            _seatStatusService = seatStatusService;
            _seatDAO = seatDAO;
        }

        public async Task HandleRequestAsync(string hub, Guid roomId, Guid userId)
        {
            currentUserId = userId.ToString();
            Console.WriteLine($"[WebSocket] New client connected: Hub={hub}, UserId={currentUserId}");

            await _webSocketManager.AddUserSocketAsync(hub, currentUserId, _webSocket);

            var buffer = new byte[1024 * 4];
            while (_webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Console.WriteLine($"[WebSocket] Client disconnected: {currentUserId}");
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        await _webSocketManager.RemoveUserSocketAsync(hub, currentUserId);
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
                                // Sử dụng TryGetValue để an toàn truy cập userSeatUpdates
                                if (!userSeatUpdates.ContainsKey(userId.ToString()))
                                {
                                    var seatList = GenerateSeatList(roomId);
                                    await SendMessageToClient(seatList);
                                }
                                else
                                {
                                    await SendUpdatedStatusToClient(roomId, hub, userSeatUpdates[currentUserId]);
                                }
                                break;
                            case "UpdateStatus":
                                Console.WriteLine("[WebSocket] UpdateStatus action received.");
                                if (request.SeatStatusUpdateRequests != null)
                                {
                                    await HandleUpdateStatusAction(request.SeatStatusUpdateRequests, hub, roomId);
                                }
                                break;

                            case "JoinRoom":
                                await StartCountdownAsync(hub, roomId, currentUserId);
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

            var originalStatuses = new ConcurrentDictionary<string, SeatStatusEnum>();
            if (userSeatUpdates.ContainsKey(userId))
            {
                foreach (var update in userSeatUpdates[userId])
                {
                    var seatGuid = Guid.Parse(update.SeatId);
                    originalStatuses[seatGuid.ToString()] = (SeatStatusEnum)(_seatStatusService.GetSeatStatus(seatGuid)?.Status ?? (int)SeatStatusEnum.UnAvailable);
                }
            }

            for (int i = 60; i >= 0; i--)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var countdownData = new {  i };
                await SendSeatsCountdownToClient(roomId, hub, countdownData);

                await Task.Delay(1000, cancellationToken);
            }
            if (!userPaymentStatus.ContainsKey(userId) || !userPaymentStatus[userId])
            {
                if (userSeatUpdates.ContainsKey(userId))
                {
                    foreach (var update in userSeatUpdates[userId])
                    {
                        var seatGuid = Guid.Parse(update.SeatId);
                        _seatStatusService.AddOrUpdateSeatStatus(seatGuid, (int)SeatStatusEnum.Available);
                    }
                    userSeatUpdates.TryRemove(userId, out _); // Remove the user's seat updates after reverting

                    // Send updated seat list to all users
                    var updatedSeatList = GenerateSeatList(roomId);
                    await SendMessageToAllUsers(hub, updatedSeatList);
                }
            }
        }
        private async Task SendSeatsCountdownToClient(Guid roomId, string hub, object countdownData)
        {
            var responseJson = JsonConvert.SerializeObject(countdownData);
            await _webSocketManager.SendMessageToUserAsync(hub, currentUserId, responseJson);
        }
        private async Task HandleUpdateStatusAction(List<SeatStatusUpdateRequest> seatStatusUpdateRequests, string hub, Guid roomId)
        {
            // Nếu chưa có user update nào, khởi tạo danh sách cho currentUserId
            if (!userSeatUpdates.ContainsKey(currentUserId))
            {
                userSeatUpdates[currentUserId] = new List<SeatStatusUpdateRequest>();
            }

            // Kiểm tra nếu chưa có update nào từ bất kỳ user nào
            bool isFirstSeatUpdate = !userSeatUpdates.Values.Any(seatUpdates => seatUpdates.Count > 0);
            bool onlyCurrentUser = (userSeatUpdates.Count == 1 && userSeatUpdates.ContainsKey(currentUserId));

            foreach (var updateRequest in seatStatusUpdateRequests)
            {
                if (updateRequest.SeatId != null)
                {
                    var seatGuid = Guid.Parse(updateRequest.SeatId);
                    var currentStatus = _seatStatusService.GetSeatStatus(seatGuid);

                    // Nếu trạng thái mới khác trạng thái hiện tại, cập nhật
                    if (currentStatus == null || currentStatus.Status != (int)updateRequest.Status)
                    {
                        _seatStatusService.AddOrUpdateSeatStatus(seatGuid, (int)updateRequest.Status);

                        // Nếu cập nhật thành Available (bỏ chọn), loại bỏ update cũ nếu có
                        if (updateRequest.Status == (int)SeatStatusEnum.Available)
                        {
                            userSeatUpdates[currentUserId].RemoveAll(r => r.SeatId == updateRequest.SeatId);
                        }
                        else
                        {
                            // Nếu cập nhật trạng thái khác (ví dụ: Selected), đảm bảo loại bỏ update cũ rồi thêm update mới
                            userSeatUpdates[currentUserId].RemoveAll(r => r.SeatId == updateRequest.SeatId);
                            userSeatUpdates[currentUserId].Add(updateRequest);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[WebSocket] No update needed for SeatId={seatGuid} as status is already {currentStatus.Status}");
                    }
                }
            }

            // Tạo danh sách ghế đã cập nhật từ roomId
            var seatListForOthers = GenerateSeatList(roomId);

            // Chỉ gửi danh sách ghế nếu đây là lần cập nhật đầu tiên hoặc chỉ có currentUser trong dictionary
            if (isFirstSeatUpdate || onlyCurrentUser)
            {
                await SendSeatsToOthers(roomId, hub, seatListForOthers);
            }

            await SendSeatStatusToAllUsersExceptSelf(roomId, hub);
            // Gửi trạng thái ghế đã cập nhật cho client của currentUser
            await SendUpdatedStatusToClient(roomId, hub, userSeatUpdates[currentUserId]);
        }



        private object GenerateSeatList(Guid roomId, List<SeatStatusUpdateRequest>? updatedSeats = null)
        {
            var seats = _seatStatusService.GenerateSeats(roomId);

            var modifiedSeats = seats.Select(seat =>
            {
                var seatStatus = _seatStatusService.GetSeatStatus(seat.SeatStatusByShowTimeId);
                if (seatStatus != null)
                {
                    if (updatedSeats != null)
                    {
                        var updatedSeat = updatedSeats.FirstOrDefault(us => us.SeatId == seat.SeatStatusByShowTimeId.ToString());
                        if (updatedSeat != null)
                        {
                            seat.Status = (int)updatedSeat.Status;
                        }
                        else if (_seatStatusService.GetHeldSeatsByUser(currentUserId).Contains(seat.SeatStatusByShowTimeId.ToString()))
                        {
                            seat.Status = seatStatus.Status;
                        }
                        else
                        {
                            seat.Status = seatStatus.Status == (int)SeatStatusEnum.UnAvailable ? (int)SeatStatusEnum.Reserved : seatStatus.Status; // Reserved for others
                        }
                    }
                    else
                    {
                        if (_seatStatusService.GetHeldSeatsByUser(currentUserId).Contains(seat.SeatStatusByShowTimeId.ToString()))
                        {
                            seat.Status = seatStatus.Status;
                        }
                        else
                        {
                            seat.Status = seatStatus.Status == (int)SeatStatusEnum.UnAvailable ? (int)SeatStatusEnum.Reserved : seatStatus.Status; // Reserved for others
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
            await _webSocketManager.SendMessageToAllExceptUserAsync(hub, currentUserId, responseJson);
        }

        private async Task SendSeatStatusToAllUsersExceptSelf(Guid roomId, string hub)
        {
            // Lấy danh sách ghế của phòng từ dịch vụ
            var seats = _seatStatusService.GenerateSeats(roomId);

            // Duyệt qua tất cả các user trong bộ nhớ và gửi thông tin ghế cho từng người dùng
            foreach (var userEntry in userSeatUpdates)
            {
                var userId = userEntry.Key;

                // Kiểm tra nếu userId là chính người gọi (bản thân mình) thì bỏ qua
                if (userId == currentUserId)
                {
                    continue;
                }

                var userSeatUpdates = userEntry.Value;

                // Nếu người dùng chưa đặt ghế, gửi danh sách ghế mới cho họ
                if (!userSeatUpdates.Any()) // Kiểm tra nếu người dùng chưa có thông tin cập nhật ghế
                {
                    var seatListForOthers = GenerateSeatList(roomId);
                    await SendSeatsToOthers(roomId, hub, seatListForOthers);
                }
                else
                {
                    // Nếu người dùng đã đặt ghế, tiếp tục xử lý như cũ
                    var modifiedSeats = seats.Select(seat =>
                    {
                        var seatStatus = _seatStatusService.GetSeatStatus(seat.SeatStatusByShowTimeId);
                        if (seatStatus != null)
                        {
                            var updatedSeat = userSeatUpdates.FirstOrDefault(us => us.SeatId == seat.SeatStatusByShowTimeId.ToString());
                            if (updatedSeat != null)
                            {
                                seat.Status = (int)updatedSeat.Status;
                            }
                            else
                            {
                                var heldSeat = _seatStatusService.GetHeldSeatsByUser(userId).Contains(seat.SeatStatusByShowTimeId.ToString());
                                if (heldSeat)
                                {
                                    seat.Status = seatStatus.Status;
                                }
                                else
                                {
                                    seat.Status = seatStatus.Status == (int)SeatStatusEnum.UnAvailable ? (int)SeatStatusEnum.Reserved : seatStatus.Status;
                                }
                            }
                        }
                        return seat;
                    }).ToList();

                    // Tạo thông điệp để gửi cho người dùng
                    var response = new { Seats = modifiedSeats };

                    // Chuyển đối tượng thành chuỗi JSON hoặc định dạng bạn muốn gửi
                    var message = JsonConvert.SerializeObject(response);

                    // Gửi thông điệp cho tất cả người dùng trừ bản thân mình
                    await _webSocketManager.SendMessageToAllExceptUserAsync(hub, currentUserId, message);
                }
            }
        }
        private async Task SendMessageToAllUsers(string hub, object message)
        {
            var responseJson = JsonConvert.SerializeObject(message);
            await _webSocketManager.SendMessageToAllUserAsync(hub, responseJson);
        }

        public class SeatActionRequest
        {
            public string Action { get; set; } = string.Empty;
            public string? SeatId { get; set; }
            public List<SeatStatusUpdateRequest>? SeatStatusUpdateRequests { get; set; }
        }

        public class SeatStatusUpdateRequest
        {
            public string SeatId { get; set; } = string.Empty;
            public SeatStatusEnum Status { get; set; }
        }
    }
}
