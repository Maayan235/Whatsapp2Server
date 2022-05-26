using Microsoft.AspNetCore.SignalR;

namespace Whatsapp2Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string id, string message)
        {

        }
    }
}
