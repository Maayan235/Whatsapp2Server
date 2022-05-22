/*using Whatsapp2Server.Models;

namespace Whatsapp2Server.services
{
    public class MessageService : IMessageService
    { 
        private static ICollection<Message> users = new List<Message>();

        public ICollection<Message> getAll()
        {
            return users;
        }
        public Message GetMessage(int? Id)
        {
            return users.FirstOrDefault(x => x.Id == Id);
        }

        public DateTime getDate(int Id)
        {
            Message m = users.FirstOrDefault(x => x.Id == Id);
            return m.Date;
        }

        public void Add(Message message)
        {
            users.Add(message);
        }

        public Message Create(string content, DateTime date, int senderId, Chat chat)
        {
            int nextId = 0;
            if (users.Count > 0)
            {
                nextId = users.Max(x => x.Id) + 1;
            }
            Message message = new Message() { Id = nextId, content = content, Date = date, senderID = senderId, chat = chat };
            users.Add(message);
            return message;

        }
    }
}
*/