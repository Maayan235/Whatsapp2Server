using Whatsapp2Server.Models;

namespace Whatsapp2Server.services
{
    public interface IMessageService 
    {
        
        public ICollection<Message> getAll();
        public Message GetMessage(int? Id);

        public DateTime getDate(int Id);

        public void Add(Message message);

        public Message Create(string content, DateTime date, int senderId, Chat chat);
        
       
    }
}
