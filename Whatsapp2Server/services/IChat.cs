using Whatsapp2Server.Models;

namespace Whatsapp2Server.services
{ 
    public interface IChat
    {
        public ICollection<Chat> getAll();
        public Chat GetChat(User user1,User user2);

        public Message getLastMessage(User user1, User user2);

        public void Add(Chat chat);

        public Chat Create(User user1, User user2);


    }
}
