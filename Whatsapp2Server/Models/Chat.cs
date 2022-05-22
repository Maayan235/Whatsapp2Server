using System.Collections.ObjectModel;

namespace Whatsapp2Server.Models
{
    public class Chat
    {

        public Chat()
        {
            id = 0;
            contacts = new Collection<User2>();
            messages = new Collection<Message>();
            lastMessage = new Message();
        }
        public int id { get; set; }

        public  ICollection<User2> contacts { get; set; }

        public ICollection<Message> messages { get; set; } 

        public Message lastMessage { get; set; }
    }
}
