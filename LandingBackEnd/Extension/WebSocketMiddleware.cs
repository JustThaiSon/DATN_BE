using AutoMapper;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Constants;
using DATN_LandingPage.Handlers;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Services.WebSockets;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
namespace DATN_LandingPage.Extension
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebSocketManager _webSocketManager;
        private readonly IMapper _mapper;
        private readonly ISeatDAO _seatDAO;
        private readonly SeatStatusService _seatStatusService = new();
        private readonly ILogger<WebSocketMiddleware> _logger;

        public WebSocketMiddleware(RequestDelegate next, IWebSocketManager webSocketManager, IMapper mapper, ISeatDAO seatDAO, ILogger<WebSocketMiddleware> logger)
        {
            _next = next;
            _webSocketManager = webSocketManager;
            _mapper = mapper;
            _seatDAO = seatDAO;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var token = context.Request.Query["access_token"].ToString();
                var utilsServices = context.RequestServices.GetService<IUltil>();

                if (utilsServices == null)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    return;
                }

                var (userId, ListRole) = utilsServices.ValidateToken(token);
                if (userId == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                var displayName = GetDisplayNameById(userId.Value);
                WebSocket webSocket = null;

                try
                {
                    webSocket = await context.WebSockets.AcceptWebSocketAsync();

                    // Handle WebSocket routes
                    switch (context.Request.Path)
                    {
                        case "/ws/KeepSeat":
                            {
                                var chatHandler = new SeatStatusShowHandler(webSocket, _webSocketManager, _mapper, _seatStatusService, _seatDAO);
                                await chatHandler.ReceiveMessages("KeepSeat", userId.Value.ToString());
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