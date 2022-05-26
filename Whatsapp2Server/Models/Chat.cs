using System.Collections.ObjectModel;

namespace Whatsapp2Server.Models
{
    public class Chat
    {
        //include!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public Chat()
        {
            id = 0;
            contacts = new List<string>();
            messages = new Collection<Message>();
            lastMessage = null;
        }
        public int id { get; set; }

        public  List<string> contacts { get; set; }

        public ICollection<Message> messages { get; set; } 

        public Message lastMessage { get; set; }
    }
}
