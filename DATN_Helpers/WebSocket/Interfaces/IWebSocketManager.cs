using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;


    public interface IWebSocketManager
    {
        Task AddUserSocketAsync(string hub, string userId, WebSocket webSocket);
        Task RemoveUserSocketAsync(string hub, string userId);
        Task AddToGroupAsync(string groupId, WebSocket webSocket);
        Task SendMessageToUserAsync(string hub, string userId, string message);
        Task SendMessageToGroupAsync(string groupId, string message);
        Task HandleDisconnectAsync(WebSocket webSocket);
        Task SendMessageToAllUserAsync(string hub, string message);
        Task SendMessageToNewUserAsync(string hub, string userId, string message);
        Task SendMessageToAllExceptUserAsync(string hub, string userId, string message); 

}

