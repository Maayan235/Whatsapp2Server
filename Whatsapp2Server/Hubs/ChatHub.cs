using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Whatsapp2Server.Models;

namespace Whatsapp2Server.Hubs
{
    public class ChatHub : Hub
    {

        private readonly IDictionary<string, string> _chats;

        public async Task SendMessage(string message)
        {
            await Clients.Group(message).SendAsync("ReceiveMessage", message);

/*            if (_chats.TryGetValue(Context.ConnectionId, out string other))
            {
                await Clients.Group(other).SendAsync("ReceiveMessage", message);
            }*/
        }


        public async Task joinToListeners([Bind("myId")] ChatObject chatObject)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatObject.myId);
            _chats[Context.ConnectionId] = chatObject.myId;
            //await Clients.Group(chatObject.myId).SendAsync("ReceiveMessage", chatObject.chatMember);
        }
    }
}
