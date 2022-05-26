using System.Collections.ObjectModel;

namespace Whatsapp2Server.Models
{
    public class Invitation
    {
        //include!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
       /* public Invitation()
        {
            id = 0;
            contacts = new string[3];
            messages = new Collection<Message>();
            lastMessage = null;
        }*/
        //public int id { get; set; }

        public  string from { get; set; }

        public  string to { get; set; } 

        public string server{ get; set; }
    }
}
