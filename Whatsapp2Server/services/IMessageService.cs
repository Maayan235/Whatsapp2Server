using Whatsapp2Server.Models;

namespace Whatsapp2Server.services
{
    public interface IMessageService 
    {
        
        public ICollection<Message1> getAll();
        public Message1 GetMessage(int? Id);

        public DateTime getDate(int Id);

        public void Add(Message1 message);

        public Message1 Create(string content, DateTime date, int senderId, Chat chat);
        
       
    }
}
