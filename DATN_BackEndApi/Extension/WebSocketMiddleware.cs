using DATN_Helpers.Common.interfaces;
using DATN_Models.DTOS.Account;

namespace DATN_BackEndApi.Extension
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebSocketManager _webSocketManager;

        public WebSocketMiddleware(IWebSocketManager webSocketManager, RequestDelegate next)
        {
            _webSocketManager = webSocketManager;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var token = context.Request.Query["access_token"].ToString();
                var utilsServices = context.RequestServices.GetService<IUltil>();

                var(userId, ListRole)  = utilsServices.ValidateToken(token);
                if (userId == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                var displayName = GetDisplayNameById(userId.Value);
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                // Options URL handler
                switch (context.Request.Path)
                {
                    //case "/ws/chat":
                    //    {
                    //        var chatHandler = new ChatWebSocketHandler(webSocket);
                    //        await chatHandler.HandleConnection(userId.Value);
                    //        break;
                    //    }
                }

            }
            else
            {
                await _next(context);
            }
        }

        private string GetDisplayNameById(Guid id)
        {
            //var playerProfile = _livestreamDAO.GetUserProfile(id, out int resProfile);
            //if (playerProfile == Guid.Empty)
            //{
            //    return "ThaiSonDepTrai";
            //}
            //return playerProfile.DisplayName;
            return "ThaiSonDepTrai";
        }
    }

}
