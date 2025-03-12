using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class TestWebSocket
{
    private static ConcurrentDictionary<string, WebSocket> _sockets = new();
    private static List<Seat> _seats = new();

    public TestWebSocket()
    {
        // Khởi tạo danh sách ghế mẫu
        _seats = new List<Seat>
        {
            new Seat { Id = "A1", IsBooked = false },
            new Seat { Id = "A2", IsBooked = false },
            new Seat { Id = "B1", IsBooked = false },
            new Seat { Id = "B2", IsBooked = false },
            new Seat { Id = "C1", IsBooked = false },
            new Seat { Id = "C2", IsBooked = false }
        };
    }

    public async Task HandleWebSocketAsync(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }

        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
        string socketId = Guid.NewGuid().ToString();
        _sockets[socketId] = webSocket;

        Console.WriteLine($"Client {socketId} connected");

        // Gửi danh sách ghế hiện tại khi client kết nối
        await SendSeatListAsync(webSocket);

        await ReceiveMessageAsync(socketId, webSocket);
    }

    private async Task ReceiveMessageAsync(string socketId, WebSocket webSocket)
    {
        byte[] buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                _sockets.TryRemove(socketId, out _);
                Console.WriteLine($"Client {socketId} disconnected");
            }
            else
            {
                string seatId = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received: {seatId}");

                // Cập nhật trạng thái ghế
                var seat = _seats.Find(s => s.Id == seatId);
                if (seat != null && !seat.IsBooked)
                {
                    seat.IsBooked = true;
                    await BroadcastSeatListAsync();
                }
            }
        }
    }

    private async Task SendSeatListAsync(WebSocket socket)
    {
        string jsonSeats = JsonSerializer.Serialize(_seats);
        byte[] buffer = Encoding.UTF8.GetBytes(jsonSeats);
        await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private async Task BroadcastSeatListAsync()
    {
        string jsonSeats = JsonSerializer.Serialize(_seats);
        byte[] buffer = Encoding.UTF8.GetBytes(jsonSeats);

        foreach (var socket in _sockets.Values)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}

// Định nghĩa lớp ghế
public class Seat
{
    public string Id { get; set; }
    public bool IsBooked { get; set; }
}
