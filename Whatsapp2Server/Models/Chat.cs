using System.Collections.ObjectModel;

namespace Whatsapp2Server.Models
{
    public class Chat
    {
        //include!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public Chat()
        {
            id = 0;
            contacts = new string[3];
            messages = new Collection<Message>();
            lastMessage = new Message();
        }
        public int id { get; set; }

        public  string[] contacts { get; set; }

        public ICollection<Message> messages { get; set; } 

        public Message lastMessage { get; set; }
    }
}
