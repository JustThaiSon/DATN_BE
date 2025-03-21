using AutoMapper;
using DATN_Helpers.Constants;
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

        // Quản lý countdown riêng cho từng user
        private static ConcurrentDictionary<string, CancellationTokenSource> userCountdownTokens
            = new ConcurrentDictionary<string, CancellationTokenSource>();

        // Lưu các cập nhật ghế của từng user
        private static ConcurrentDictionary<string, List<SeatStatusUpdateRequest>> userSeatUpdates
            = new ConcurrentDictionary<string, List<SeatStatusUpdateRequest>>();

        // Lưu trạng thái thanh toán của từng user
        private static ConcurrentDictionary<string, bool> userPaymentStatus
            = new ConcurrentDictionary<string, bool>();

        private string currentUserId;

        public SeatStatusShowHandler(
            WebSocket webSocket,
            IWebSocketManager webSocketManager,
            IMapper mapper,
            SeatStatusService seatStatusService,
            ISeatDAO seatDAO)
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

            // Đăng ký socket này vào WebSocketManager
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

                        // Hủy countdown khi user thoát
                        CancelUserCountdown(currentUserId);
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
                                // Kiểm tra userSeatUpdates
                                if (!userSeatUpdates.ContainsKey(currentUserId))
                                {
                                    var seatList = GenerateSeatList(roomId);
                                    await SendMessageToClient(seatList);
                                }
                                else
                                {
                                    // Gửi danh sách ghế kèm trạng thái update cũ
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
                                // Chạy countdown dưới dạng background task để không chặn vòng lặp xử lý message
                                _ = StartCountdownAsync(hub, roomId, currentUserId);
                                break;
                            case "Payment":
                                if (userSeatUpdates.ContainsKey(currentUserId))
                                {
                                    userSeatUpdates.TryRemove(currentUserId, out _);
                                }

                                if (!userSeatUpdates.ContainsKey(currentUserId))
                                {
                                    var seatList = GenerateSeatList(roomId);
                                    await SendMessageToAllUsers(hub, seatList);
                                }
                                else
                                {
                                    // Gửi danh sách ghế kèm trạng thái update cũ
                                    await SendUpdatedStatusToClient(roomId, hub, userSeatUpdates[currentUserId]);
                                }
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

        /// <summary>
        /// Hủy countdown cho user
        /// </summary>
        private void CancelUserCountdown(string userId)
        {
            if (userCountdownTokens.TryGetValue(userId, out var tokenSource))
            {
                if (!tokenSource.IsCancellationRequested)
                    tokenSource.Cancel();

                tokenSource.Dispose();
            }
            userCountdownTokens.TryRemove(userId, out _);
        }

        /// <summary>
        /// Tạo countdown riêng cho user, sau thời gian quy định sẽ hủy ghế nếu chưa thanh toán
        /// </summary>
        private async Task StartCountdownAsync(string hub, Guid roomId, string userId)
        {
            // 1. Hủy token cũ (nếu có) để tránh chạy song song
            CancelUserCountdown(userId);

            // 2. Tạo token mới cho user
            var newTokenSource = new CancellationTokenSource();
            userCountdownTokens[userId] = newTokenSource;
            var cancellationToken = newTokenSource.Token;

            // (Tuỳ chọn) Reset cờ thanh toán = false mỗi lần JoinRoom
            userPaymentStatus[userId] = false;

            // 3. Bắt đầu đếm ngược 60 giây
            for (int i = 120; i >= 0; i--)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"[WebSocket] Countdown cancelled for user={userId}");
                    break;
                }

                var countdownData = new { i };
                await SendSeatsCountdownToClient(roomId, hub, countdownData);

                await Task.Delay(1000, cancellationToken);
            }

            // 4. Hết thời gian => revert ghế (nếu user chưa thanh toán)
            if (!cancellationToken.IsCancellationRequested)
            {
                // Kiểm tra trạng thái thanh toán
                if (!userPaymentStatus.ContainsKey(userId) || !userPaymentStatus[userId])
                {
                    if (userSeatUpdates.ContainsKey(userId))
                    {
                        foreach (var update in userSeatUpdates[userId])
                        {
                            var seatGuid = Guid.Parse(update.SeatId);
                            _seatStatusService.AddOrUpdateSeatStatus(seatGuid, (int)SeatStatusEnum.Available);
                        }
                        // Xóa thông tin ghế của user
                        userSeatUpdates.TryRemove(userId, out _);
                        // Gửi danh sách ghế mới nhất cho tất cả user
                        var updatedSeatList = GenerateSeatList(roomId);
                        await SendMessageToAllUsers(hub, updatedSeatList);
                    }
                }
            }
        }

        /// <summary>
        /// Gửi thông tin đếm ngược (countdown) cho chính user
        /// </summary>
        private async Task SendSeatsCountdownToClient(Guid roomId, string hub, object countdownData)
        {
            var responseJson = JsonConvert.SerializeObject(countdownData);
            await _webSocketManager.SendMessageToUserAsync(hub, currentUserId, responseJson);
        }

        /// <summary>
        /// Xử lý action UpdateStatus
        /// </summary>
        private async Task HandleUpdateStatusAction(
            List<SeatStatusUpdateRequest> seatStatusUpdateRequests,
            string hub,
            Guid roomId)
        {
            // Nếu chưa có user update nào, khởi tạo
            if (!userSeatUpdates.ContainsKey(currentUserId))
            {
                userSeatUpdates[currentUserId] = new List<SeatStatusUpdateRequest>();
            }

            // Kiểm tra nếu chưa có update nào từ bất kỳ user nào
            bool isFirstSeatUpdate = !userSeatUpdates.Values.Any(seatUpdates => seatUpdates.Count > 0);
            bool onlyCurrentUser = (userSeatUpdates.Count == 1 && userSeatUpdates.ContainsKey(currentUserId));

            // Duyệt qua các yêu cầu update
            foreach (var updateRequest in seatStatusUpdateRequests)
            {
                if (updateRequest.SeatId != null)
                {
                    var seatGuid = Guid.Parse(updateRequest.SeatId);
                    var currentStatus = _seatStatusService.GetSeatStatus(seatGuid);

                    // Nếu trạng thái mới khác trạng thái hiện tại => cập nhật
                    if (currentStatus == null || currentStatus.Status != (int)updateRequest.Status)
                    {
                        _seatStatusService.AddOrUpdateSeatStatus(seatGuid, (int)updateRequest.Status);

                        // Nếu ghế "Available" => xóa bản ghi cũ
                        if (updateRequest.Status == SeatStatusEnum.Available)
                        {
                            userSeatUpdates[currentUserId].RemoveAll(r => r.SeatId == updateRequest.SeatId);
                        }
                        else
                        {
                            // Cập nhật ghế "Selected" hoặc trạng thái khác
                            userSeatUpdates[currentUserId].RemoveAll(r => r.SeatId == updateRequest.SeatId);
                            userSeatUpdates[currentUserId].Add(updateRequest);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[WebSocket] No update needed for SeatId={seatGuid} (already {currentStatus?.Status})");
                    }
                }
            }

            // Tạo danh sách ghế để gửi cho user khác
            var seatListForOthers = GenerateSeatList(roomId);

            // Lần cập nhật đầu tiên hoặc chỉ 1 user => gửi toàn bộ cho others
            if (isFirstSeatUpdate || onlyCurrentUser)
            {
                await SendSeatsToOthers(roomId, hub, seatListForOthers);
            }
            else
            {
                // Nếu nhiều user => gửi update cho các user khác
                await SendSeatStatusToAllUsersExceptSelf(roomId, hub);
            }

            // Gửi trạng thái ghế đã cập nhật cho chính user
            await SendUpdatedStatusToClient(roomId, hub, userSeatUpdates[currentUserId]);
        }

        /// <summary>
        /// Sinh danh sách ghế, kèm trạng thái (Reserved, Selected, UnAvailable, ...)
        /// </summary>
        private object GenerateSeatList(Guid roomId, List<SeatStatusUpdateRequest>? updatedSeats = null)
        {
            var seats = _seatStatusService.GenerateSeats(roomId);

            var modifiedSeats = seats.Select(seat =>
            {
                var seatStatus = _seatStatusService.GetSeatStatus(seat.SeatStatusByShowTimeId);
                if (seatStatus != null)
                {
                    // Nếu có danh sách ghế cập nhật, check xem ghế này có trong updatedSeats không
                    if (updatedSeats != null)
                    {
                        var updatedSeat = updatedSeats.FirstOrDefault(
                            us => us.SeatId == seat.SeatStatusByShowTimeId.ToString()
                        );

                        if (updatedSeat != null)
                        {
                            seat.Status = (int)updatedSeat.Status;
                        }
                        else if (_seatStatusService
                            .GetHeldSeatsByUser(currentUserId)
                            .Contains(seat.SeatStatusByShowTimeId.ToString()))
                        {
                            seat.Status = seatStatus.Status;
                        }
                        else
                        {
                            // Nếu là UnAvailable => Reserved (cho user khác)
                            seat.Status = seatStatus.Status == (int)SeatStatusEnum.UnAvailable
                                ? (int)SeatStatusEnum.Reserved
                                : seatStatus.Status;
                        }
                    }
                    else
                    {
                        // Nếu không có danh sách cập nhật => lấy theo seatStatus
                        if (_seatStatusService
                            .GetHeldSeatsByUser(currentUserId)
                            .Contains(seat.SeatStatusByShowTimeId.ToString()))
                        {
                            seat.Status = seatStatus.Status;
                        }
                        else
                        {
                            seat.Status = seatStatus.Status == (int)SeatStatusEnum.UnAvailable
                                ? (int)SeatStatusEnum.Reserved
                                : seatStatus.Status;
                        }
                    }
                }
                return seat;
            }).ToList();

            return new { Seats = modifiedSeats };
        }

        /// <summary>
        /// Gửi message cho chính user hiện tại
        /// </summary>
        private async Task SendMessageToClient(object message)
        {
            var responseJson = JsonConvert.SerializeObject(message);
            var buffer = Encoding.UTF8.GetBytes(responseJson);
            await _webSocket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }



        /// <summary>
        /// Gửi message báo lỗi cho chính user
        /// </summary>
        private async Task SendErrorMessage(string error)
        {
            var errorResponse = new { Error = error };
            await SendMessageToClient(errorResponse);
        }

        /// <summary>
        /// Gửi trạng thái ghế (updatedSeats) cho chính user
        /// </summary>
        private async Task SendUpdatedStatusToClient(Guid roomId, string hub, List<SeatStatusUpdateRequest> updatedSeats)
        {
            var seatList = GenerateSeatList(roomId, updatedSeats);

            var responseJson = JsonConvert.SerializeObject(seatList, Formatting.Indented);
            var buffer = Encoding.UTF8.GetBytes(responseJson);
            await _webSocket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }

        /// <summary>
        /// Gửi danh sách ghế cho tất cả user khác (trừ currentUser)
        /// </summary>
        private async Task SendSeatsToOthers(Guid roomId, string hub, object seatList)
        {
            var responseJson = JsonConvert.SerializeObject(seatList);
            await _webSocketManager.SendMessageToAllExceptUserAsync(hub, currentUserId, responseJson);
        }

        /// <summary>
        /// Gửi trạng thái ghế cho tất cả user trừ currentUser (khi có nhiều user)
        /// </summary>
        private async Task SendSeatStatusToAllUsersExceptSelf(Guid roomId, string hub)
        {
            // Lấy danh sách ghế
            var seats = _seatStatusService.GenerateSeats(roomId);

            // Gửi cho từng user
            foreach (var userEntry in userSeatUpdates)
            {
                var userId = userEntry.Key;

                // Bỏ qua user hiện tại
                if (userId == currentUserId) continue;

                var userSeatUpdateList = userEntry.Value;

                // Nếu user chưa chọn ghế nào => gửi toàn bộ danh sách
                if (!userSeatUpdateList.Any())
                {
                    var seatListForOthers = GenerateSeatList(roomId);
                    await SendSeatsToOthers(roomId, hub, seatListForOthers);
                }
                else
                {
                    // User có cập nhật ghế => cập nhật status phù hợp
                    var modifiedSeats = seats.Select(seat =>
                    {
                        var seatStatus = _seatStatusService.GetSeatStatus(seat.SeatStatusByShowTimeId);
                        if (seatStatus != null)
                        {
                            var updatedSeat = userSeatUpdateList
                                .FirstOrDefault(us => us.SeatId == seat.SeatStatusByShowTimeId.ToString());

                            if (updatedSeat != null)
                            {
                                seat.Status = (int)updatedSeat.Status;
                            }
                            else
                            {
                                // Kiểm tra ghế có bị user này hold không
                                var heldSeat = _seatStatusService
                                    .GetHeldSeatsByUser(userId)
                                    .Contains(seat.SeatStatusByShowTimeId.ToString());

                                if (heldSeat)
                                {
                                    seat.Status = seatStatus.Status;
                                }
                                else
                                {
                                    seat.Status = seatStatus.Status == (int)SeatStatusEnum.UnAvailable
                                        ? (int)SeatStatusEnum.Reserved
                                        : seatStatus.Status;
                                }
                            }
                        }
                        return seat;
                    }).ToList();

                    var response = new { Seats = modifiedSeats };
                    var message = JsonConvert.SerializeObject(response);
                    await _webSocketManager.SendMessageToAllExceptUserAsync(hub, currentUserId, message);
                }
            }
        }

        /// <summary>
        /// Gửi message cho tất cả user trong hub
        /// </summary>
        private async Task SendMessageToAllUsers(string hub, object message)
        {
            var responseJson = JsonConvert.SerializeObject(message);
            await _webSocketManager.SendMessageToAllUserAsync(hub, responseJson);
        }

        // ----------------------------
        // Các class request hỗ trợ
        // ----------------------------
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
