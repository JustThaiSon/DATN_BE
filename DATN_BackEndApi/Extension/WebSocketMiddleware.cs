using AutoMapper;
using DATN_BackEndApi.Handlers;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Constants;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Services.WebSockets;
using System.Net.WebSockets;
using System.Text;
namespace DATN_BackEndApi.Extension
{
    namespace DATN_BackEndApi.Extension
    {
        public class WebSocketMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly IWebSocketManager _webSocketManager;
            private readonly IMapper _mapper;
            private readonly ISeatDAO _seatDAO;
            private readonly SeatStatusService _seatStatusService = new();

            public WebSocketMiddleware(RequestDelegate next, IWebSocketManager webSocketManager, IMapper mapper, ISeatDAO seatDAO)
            {
                _next = next;
                _webSocketManager = webSocketManager;
                _mapper = mapper;
                _seatDAO = seatDAO;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var token = context.Request.Query["access_token"].ToString();
                    var utilsServices = context.RequestServices.GetService<IUltil>();

                    var (userId, ListRole) = utilsServices.ValidateToken(token);
                    if (userId == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }

                    var seatId = context.Request.Query["seatId"].ToString();
                    var displayName = GetDisplayNameById(userId.Value);
                    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                    // Handle WebSocket routes
                    switch (context.Request.Path)
                    {
                        case "/ws/KeepSeat":
                            {
                                var chatHandler = new SeatStatusShowHandler(webSocket, _webSocketManager, _mapper, _seatStatusService, _seatDAO);
                                await chatHandler.HandleUpdateSeatStatusAsync(seatId, (int)SeatStatusEnum.Available, "KeepSeat");
                                break;
                            }

                        case "/ws/Test":
                            {
                                await HandleTestWebSocketAsync(webSocket);
                                break;
                            }

                        default:
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                break;
                            }
                    }
                }
                else
                {
                    await _next(context);
                }
            }
            private async Task HandleTestWebSocketAsync(WebSocket webSocket)
            {
                var buffer = new byte[1024 * 4];
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        break;
                    }

                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Received: {message}");

                    var responseMessage = $"Server Echo: {message}";
                    var responseBytes = Encoding.UTF8.GetBytes(responseMessage);
                    await webSocket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
                private string GetDisplayNameById(Guid id)
                {
                    return "ThaiSonDepTrai";
                }
            }
        }
    }