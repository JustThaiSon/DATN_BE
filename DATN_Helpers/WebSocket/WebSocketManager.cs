using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

public class WebSocketManager : IWebSocketManager
{
    private readonly ConcurrentDictionary<string, WebSocket> _userSockets = new();
    private readonly ConcurrentDictionary<string, List<WebSocket>> _groupSockets = new();

    #region Users
    public async Task AddUserSocketAsync(string hub, string userId, WebSocket webSocket)
    {
        string key = $"{hub}_{userId}";
        _userSockets[key] = webSocket;
    }
    public async Task RemoveUserSocketAsync(string hub, string userId)
    {
        string key = $"{hub}_{userId}";
        _userSockets.TryRemove(key, out _);
    }
    public async Task SendMessageToUserAsync(string hub, string userId, string message)
    {
        string key = $"{hub}_{userId}";
        if (_userSockets.TryGetValue(key, out WebSocket webSocket) && webSocket.State == WebSocketState.Open)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
    public async Task SendMessageToAllUserAsync(string hub, string message)
    {
        var hubSockets = _userSockets
        .Where(kvp => kvp.Key.StartsWith(hub))
        .Select(kvp => kvp.Value)
        .ToList();
        if (hubSockets.Any())
        {
            foreach (var hubSocket in hubSockets)
            {
                if (hubSocket.State == WebSocketState.Open)
                {
                    var buffer = Encoding.UTF8.GetBytes(message);
                    await hubSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

    }
    public async Task SendMessageToNewUserAsync(string hub, string userId, string message)
    {
        var hubSockets = _userSockets
        .Where(kvp => kvp.Key.StartsWith($"{hub}_{userId}"))
        .Select(kvp => kvp.Value)
        .ToList();
        if (hubSockets.Any())
        {
            foreach (var hubSocket in hubSockets)
            {
                if (hubSocket.State == WebSocketState.Open)
                {
                    var buffer = Encoding.UTF8.GetBytes(message);
                    await hubSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

    }

    #endregion

    #region Group
    public async Task AddToGroupAsync(string groupId, WebSocket webSocket)
    {
        _groupSockets.AddOrUpdate(groupId,
            _ => new List<WebSocket> { webSocket },
            (_, list) =>
            {
                lock (list)
                {
                    list.Add(webSocket);
                }
                return list;
            });
    }
    public async Task SendMessageToGroupAsync(string groupId, string message)
    {
        if (_groupSockets.TryGetValue(groupId, out var webSockets))
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            List<WebSocket> toRemove = new();

            foreach (var webSocket in webSockets)
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    toRemove.Add(webSocket);
                }
            }

            lock (webSockets)
            {
                foreach (var ws in toRemove)
                {
                    webSockets.Remove(ws);
                }
            }
        }
    }

    public async Task HandleDisconnectAsync(WebSocket webSocket)
    {
        foreach (var (userId, socket) in _userSockets)
        {
            if (socket == webSocket)
            {
                _userSockets.TryRemove(userId, out _);
                break;
            }
        }

        foreach (var (groupId, webSockets) in _groupSockets)
        {
            if (webSockets.Contains(webSocket))
            {
                lock (webSockets)
                {
                    webSockets.Remove(webSocket);
                }

                if (webSockets.Count == 0)
                {
                    _groupSockets.TryRemove(groupId, out _);
                }
            }
        }
    }

    public async Task PingToAllClientAsync(string hub)
    {
        var hubSockets = _userSockets
       .Where(kvp => kvp.Key.StartsWith($"{hub}"))
       .Select(kvp => kvp.Value)
       .ToList();


    }

    #endregion
}
