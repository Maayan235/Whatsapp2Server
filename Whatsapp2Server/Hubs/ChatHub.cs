using Microsoft.AspNetCore.SignalR;
using Whatsapp2Server.Models;

namespace Whatsapp2Server.Hubs
{
    public class ChatHub : Hub
    {

        private readonly IDictionary<string, string> _chats;

        public async Task SendMessage(string message)
        {
            if (_chats.TryGetValue(Context.ConnectionId, out string other))
            {
                await Clients.Group(other).SendAsync("ReceiveMessage", message);
            }
        }


        public async Task joinToListeners(ChatObject chatObject)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatObject.chatMember);
            _chats[Context.ConnectionId] = chatObject.chatMember;
            /*await Clients.Group(id).SendAsync("ReceiveMessage", content);*/
        }
    }
}
