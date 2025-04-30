using AutoMapper;
using DATN_Helpers.Constants;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Services.WebSockets;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        // Lưu thời gian countdown còn lại của từng user
        private static ConcurrentDictionary<string, int> userCountdownRemaining
            = new ConcurrentDictionary<string, int>();

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
                                _ = StartOrExtendCountdownAsync(hub, roomId, currentUserId, 600, true); // Default 120 seconds
                                break;

                            case "ExtendCountdown":
                                if (request.ExtensionDuration.HasValue)
                                {
                                    _ = StartOrExtendCountdownAsync(hub, roomId, currentUserId, request.ExtensionDuration.Value);
                                }
                                break;

                            case "Payment":
                                if (request.SeatStatusUpdateRequests != null)
                                {

                                    await HandlePaymentStatusAction(request.SeatStatusUpdateRequests, hub, roomId);
                                }
                                break;
                            case "Refund":
                                if (request.SeatStatusUpdateRequests != null)
                                {
                                    await HandleRefundAction(request.SeatStatusUpdateRequests, hub, roomId);
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

            // Xử lý khi client bị ngắt kết nối
            Console.WriteLine($"[WebSocket] Client disconnected: {currentUserId}");
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            await _webSocketManager.RemoveUserSocketAsync(hub, currentUserId);

            // Hủy countdown khi user thoát
            CancelUserCountdown(currentUserId);
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
            userCountdownRemaining.TryRemove(userId, out _);
        }

        /// <summary>
        /// Tạo hoặc gia hạn countdown riêng cho user, sau thời gian quy định sẽ hủy ghế nếu chưa thanh toán
        /// </summary>
        private async Task StartOrExtendCountdownAsync(string hub, Guid roomId, string userId, int durationInSeconds, bool isJoinRoom = false)
        {
            int newDuration = durationInSeconds;

            if (userCountdownRemaining.TryGetValue(userId, out int remainingTime))
            {
                if (isJoinRoom)
                {
                    newDuration = remainingTime;
                }
                else
                {
                    newDuration += remainingTime;
                }
            }
            else
            {
                userCountdownRemaining[userId] = newDuration;
            }

            CancelUserCountdown(userId);

            var newTokenSource = new CancellationTokenSource();
            userCountdownTokens[userId] = newTokenSource;
            var cancellationToken = newTokenSource.Token;

            if (isJoinRoom)
            {
                userPaymentStatus[userId] = false;
            }

            try
            {
                for (int i = newDuration; i >= 0; i--)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine($"[WebSocket] Countdown cancelled for user={userId}");
                        break;
                    }

                    userCountdownRemaining[userId] = i;
                    var countdownData = new { i };
                    await SendSeatsCountdownToClient(roomId, hub, countdownData);

                    await Task.Delay(1000, cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine($"[WebSocket] Countdown task cancelled due to disconnect for user={userId}");
            }
            finally
            {
                bool paid = userPaymentStatus.TryGetValue(userId, out var isPaid) && isPaid;

                if (!paid)
                {
                    await RevertSeatsIfUnpaidAsync(hub, roomId, userId);
                }

                userCountdownTokens.TryRemove(userId, out _);
                userCountdownRemaining.TryRemove(userId, out _);
                userPaymentStatus.TryRemove(userId, out _);
            }
        }
        private async Task HandleRefundAction(
            List<SeatStatusUpdateRequest> seatStatusUpdateRequests,
            string hub,
            Guid roomId)
        {
            bool isFirstSeatUpdate = !userSeatUpdates.Values.Any(seatUpdates => seatUpdates.Count > 0);
            bool onlyCurrentUser = (userSeatUpdates.Count == 1 && userSeatUpdates.ContainsKey(currentUserId));
            foreach (var updateRequest in seatStatusUpdateRequests)
            {
                if (updateRequest.SeatId != null)
                {
                    var seatGuid = Guid.Parse(updateRequest.SeatId);

                    // Update the seat status to Available
                    _seatStatusService.AddOrUpdateSeatStatus(seatGuid, (int)SeatStatusEnum.Available);

                    // Remove the seat from user updates
                    if (userSeatUpdates.ContainsKey(currentUserId))
                    {
                        userSeatUpdates[currentUserId].RemoveAll(r => r.SeatId == updateRequest.SeatId);
                    }
                }
            }

            if (isFirstSeatUpdate || onlyCurrentUser)
            {
                var seatListForOthers = GenerateSeatList(roomId);
                await SendSeatsToOthers(roomId, hub, seatListForOthers);
            }
            else
            {
                await SendSeatStatusToAllUsersExceptSelf(roomId, hub);
            }
            await SendUpdatedStatusToClient(roomId, hub, userSeatUpdates[currentUserId]);
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            await _webSocketManager.RemoveUserSocketAsync(hub, currentUserId);
        }
        private async Task RevertSeatsIfUnpaidAsync(string hub, Guid roomId, string userId)
        {
            if (userSeatUpdates.ContainsKey(userId))
            {
                foreach (var update in userSeatUpdates[userId])
                {
                    var seatGuid = Guid.Parse(update.SeatId);
                    _seatStatusService.AddOrUpdateSeatStatus(seatGuid, (int)SeatStatusEnum.Available);
                }

                userSeatUpdates.TryRemove(userId, out _);

                // Gửi danh sách ghế mới nhất cho tất cả user
                var updatedSeatList = GenerateSeatList(roomId);
                await SendMessageToAllUsers(hub, updatedSeatList);

                Console.WriteLine($"[WebSocket] Ghế đã được revert do timeout/disconnect cho user={userId}");
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


        private async Task HandlePaymentStatusAction(
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
            // Duyệt qua các yêu cầu update trạng thái thanh toán
            foreach (var updateRequest in seatStatusUpdateRequests)
            {
                if (updateRequest.SeatId != null)
                {
                    var seatGuid = Guid.Parse(updateRequest.SeatId);
                    var currentStatus = _seatStatusService.GetSeatStatus(seatGuid);

                    // Nếu trạng thái ghế mới khác trạng thái hiện tại => cập nhật
                    if (currentStatus == null || currentStatus.Status != (int)updateRequest.Status)
                    {
                        // Kiểm tra xem ghế có phải đang được người dùng hiện tại giữ không
                        var isHeldByCurrentUser = _seatStatusService.GetHeldSeatsByUser(currentUserId)
                            .Contains(updateRequest.SeatId);

                        // Tránh cập nhật nếu ghế đang được giữ bởi user hiện tại
                        if (!isHeldByCurrentUser)
                        {
                            // Cập nhật trạng thái ghế nếu không phải đang được giữ bởi người dùng hiện tại
                            _seatStatusService.AddOrUpdateSeatStatus(seatGuid, (int)updateRequest.Status);

                            // Nếu ghế đã thanh toán (Paid) => kiểm tra lại và cập nhật trạng thái
                            if (updateRequest.Status == SeatStatusEnum.Paied)
                            {
                                // Nếu ghế không phải của người dùng, gán trạng thái Paid và bỏ ghế khỏi các bản cập nhật của người dùng
                                _seatStatusService.RemoveSeat(seatGuid);
                                userSeatUpdates[currentUserId].RemoveAll(r => r.SeatId == updateRequest.SeatId);
                                userSeatUpdates[currentUserId].Add(updateRequest);  // Add update request after removal
                            }
                            else
                            {
                                // Cập nhật các trạng thái khác (trạng thái không phải Paid)
                                await SendUpdatedStatusToClient(roomId, hub, new List<SeatStatusUpdateRequest> { updateRequest });
                            }
                        }
                        else
                        {
                            // Giữ nguyên trạng thái cho ghế đã thanh toán nếu là của người dùng hiện tại
                            // Giữ ghế đang được giữ trong trạng thái "Held" hoặc "Paid" nếu người dùng đã thanh toán
                            userSeatUpdates[currentUserId].Add(updateRequest);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[Payment] No update needed for SeatId={seatGuid} (already {currentStatus?.Status})");
                    }
                }
            }

            // Lần cập nhật đầu tiên hoặc chỉ 1 user => gửi toàn bộ cho others
            if (isFirstSeatUpdate || onlyCurrentUser)
            {
                // Tạo danh sách ghế để gửi cho các user khác
                var seatListForOthers = GenerateSeatList(roomId);
                await SendSeatsToOthers(roomId, hub, seatListForOthers);
            }
            else
            {
                // Nếu nhiều user => gửi update cho các user khác
                await SendSeatStatusToAllUsersExceptSelf(roomId, hub);
            }
            await SendUpdatedStatusToClient(roomId, hub, userSeatUpdates[currentUserId]);
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            await _webSocketManager.RemoveUserSocketAsync(hub, currentUserId);

        }


        /// <summary>
        /// Sinh danh sách ghế, kèm trạng thái (Reserved, Selected, UnAvailable, ...)
        /// </summary>
        private object GenerateSeatList(Guid roomId, List<SeatStatusUpdateRequest>? updatedSeats = null)
        {
            var seats = _seatStatusService.GenerateSeats(roomId);

            var modifiedSeats = seats.Select(seat =>
            {
                var seatIdStr = seat.SeatStatusByShowTimeId.ToString();
                var seatStatus = _seatStatusService.GetSeatStatus(seat.SeatStatusByShowTimeId);

                if (seatStatus != null)
                {
                    // ✅ Nếu trạng thái là Paid
                    if (seatStatus.Status == (int)SeatStatusEnum.Paied)
                    {
                        // Kiểm tra xem ghế đó có đang được giữ bởi user hiện tại không
                        var heldByCurrentUser = _seatStatusService
                            .GetHeldSeatsByUser(currentUserId)
                            .Contains(seatIdStr);

                        // Nếu không phải của user hiện tại => cập nhật Paid
                        if (!heldByCurrentUser)
                        {
                            _seatStatusService.RemoveSeat(seat.SeatStatusByShowTimeId);

                            if (updatedSeats != null)
                            {
                                var itemToRemove = updatedSeats.FirstOrDefault(x => x.SeatId == seatIdStr);
                                if (itemToRemove != null)
                                {
                                    updatedSeats.Remove(itemToRemove);
                                }
                            }

                            seat.Status = (int)SeatStatusEnum.Paied;
                        }
                        else
                        {
                            // Nếu là của user hiện tại => vẫn giữ nguyên trạng thái đang giữ
                            seat.Status = seatStatus.Status;
                        }
                    }
                    else
                    {
                        if (updatedSeats != null)
                        {
                            var updatedSeat = updatedSeats.FirstOrDefault(us => us.SeatId == seatIdStr);

                            if (updatedSeat != null)
                            {
                                seat.Status = (int)updatedSeat.Status;
                            }
                            else if (_seatStatusService
                                .GetHeldSeatsByUser(currentUserId)
                                .Contains(seatIdStr))
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
                        else
                        {
                            if (_seatStatusService
                                .GetHeldSeatsByUser(currentUserId)
                                .Contains(seatIdStr))
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
            public int? ExtensionDuration { get; set; }
        }

        public class SeatStatusUpdateRequest
        {
            public string SeatId { get; set; } = string.Empty;
            public SeatStatusEnum Status { get; set; }
        }
    }
}
