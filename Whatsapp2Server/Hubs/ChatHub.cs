using Microsoft.AspNetCore.SignalR;

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


        public async Task joinToListeners( string id)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId,id);
            _chats[Context.ConnectionId] = id;
            /*await Clients.Group(id).SendAsync("ReceiveMessage", content);*/
        }
    }
}
