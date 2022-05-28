using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Whatsapp2Server.Models;

namespace Whatsapp2Server.Hubs
{
    public class ChatHub : Hub
    {

        public async Task SendMessage(string message)
        {
            await Clients.Group(message).SendAsync("ReceiveMessage", message);

        }


        public async Task joinToListeners([Bind("myId")] ChatObject chatObject)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatObject.myId);

        }
    }
}
