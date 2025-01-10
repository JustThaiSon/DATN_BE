using NekBigCore.Models.DTOs.WebSockets;
using System.Net.WebSockets;
using System.Text;

namespace NekBigCore.Services.WebSockets
{
    public class WebSocketService
    {
        public async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                // Xử lý tin nhắn nhận được
                var webSocketMessage = new WebSocketMessage { Type = "Received", Content = receivedMessage };

                // Gửi phản hồi tới client
                var responseMessage = Encoding.UTF8.GetBytes($"Server received: {receivedMessage}");
                await webSocket.SendAsync(new ArraySegment<byte>(responseMessage, 0, responseMessage.Length), WebSocketMessageType.Text, true, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
