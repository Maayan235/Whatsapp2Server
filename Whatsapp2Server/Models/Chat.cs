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
            messages = new Collection<Message1>();
            lastMessage = null;
        }
        public int id { get; set; }

        public  List<string> contacts { get; set; }

        public ICollection<Message1> messages { get; set; } 

        public Message1 lastMessage { get; set; }
    }
}
