using System.Collections.ObjectModel;

namespace Whatsapp2Server.Models
{
    public class Contacts
    {
public Contacts()
        {
            id = "";
            contacts = new Collection<User2>();
        }

        public string id { get; set; }
        public ICollection<User2> contacts;
        
    
    }
}
