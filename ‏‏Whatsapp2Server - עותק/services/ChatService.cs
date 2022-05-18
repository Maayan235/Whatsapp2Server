using Whatsapp2Server.Models;

namespace Whatsapp2Server.services
{
    public class ChatService : IChat
    {
        private static ICollection<Chat> _chats;
        public Chat GetChat(User user1, User user2)
        {
            //throw new NotImplementedException();
            return findChat(user1, user2); 
        }
        public ICollection<Chat> getAll()
        {
            return _chats;
        }

        public void Add(Chat chat)
        {
            _chats.Add(chat);
        }

        /*public void Add(User user1, User user2)
        {

        }*/

        public Message getLastMessage(User user1,User user2)
        {
            
            //            throw new NotImplementedException();
            Chat chat = findChat(user1, user2);
            return chat.LastMessage;

        }
        private Chat findChat(User user1, User user2)
        {
            HashSet<User> contacts = new HashSet<User>();
            contacts.Add(user1);
            contacts.Add(user2);

            Chat temp = _chats.FirstOrDefault(chat => chat.contacts.SetEquals(contacts));
            return temp;
        }
        public Chat Create(User user1,User user2)
        {
            HashSet<User> contacts = new HashSet<User>();
            contacts.Add(user1);
            contacts.Add(user2);
            Chat chat = new Chat();
            chat.contacts = contacts;
            chat.Id = _chats.Max(x => x.Id) + 1;
            return chat;
        }

    }
}
