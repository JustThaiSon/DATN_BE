using AutoMapper;
using DATN_LandingPage.Handlers;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Services.WebSockets;
using System.Net.WebSockets;
namespace DATN_LandingPage.Extension
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebSocketManager _webSocketManager;
        private readonly IMapper _mapper;
        private readonly ISeatDAO _seatDAO;
        private readonly SeatStatusService _seatStatusService;
        private readonly ILogger<WebSocketMiddleware> _logger;

        public WebSocketMiddleware(RequestDelegate next, IWebSocketManager webSocketManager, IMapper mapper, ISeatDAO seatDAO, ILogger<WebSocketMiddleware> logger, SeatStatusService seatStatusService)
        {
            _next = next;
            _webSocketManager = webSocketManager;
            _mapper = mapper;
            _seatDAO = seatDAO;
            _logger = logger;
            _seatStatusService = seatStatusService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = null;

                try
                {
                    webSocket = await context.WebSockets.AcceptWebSocketAsync();

                    // Handle WebSocket routes
                    switch (context.Request.Path)
                    {
                        case "/ws/KeepSeat":
                            {
                                var roomIdString = context.Request.Query["roomId"].ToString();
                                var userIdString = context.Request.Query["userId"].ToString();
                                if (Guid.TryParse(roomIdString, out Guid roomId) && Guid.TryParse(userIdString, out Guid userId))
                                {
                                    var seatStatusHandler = new SeatStatusShowHandler(webSocket, _webSocketManager, _mapper, _seatStatusService, _seatDAO);
                                    await seatStatusHandler.HandleRequestAsync("KeepSeat", roomId, userId);
                                }
                                else
                                {
                                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                    await context.Response.WriteAsync("Invalid roomId or userId parameter.");
                                }
                                break;
                            }
                        default:
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while accepting the WebSocket connection.");
                    if (webSocket != null)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Internal Server Error", CancellationToken.None);
                    }
                }
                finally
                {
                    if (webSocket != null && webSocket.State != WebSocketState.Closed)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        _logger.LogInformation("WebSocket connection closed.");
                    }
                }
            }
            else
            {
                await _next(context);
            }
        }

        private string GetDisplayNameById(Guid id)
        {
            return "ThaiSonDepTrai";
        }
    }
}