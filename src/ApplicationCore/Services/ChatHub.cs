using Microsoft.AspNetCore.SignalR;

namespace Microsoft.Nnn.ApplicationCore.Services
{
    public class ChatHub : Hub
    {
        public void SendToAll(string name, string message)
        {
            Clients.All.InvokeAsync("sendToAll", name, message);
        }
    } 
}