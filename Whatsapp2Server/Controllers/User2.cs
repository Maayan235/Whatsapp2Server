using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Whatsapp2Server.Models
{
    public class User2
    {
        public User2()
        {
            //Id = 0;
            id = "";
            password = "";
            name = "";
            server = "localhost:5286";
            contacts = new Collection<User2>();
            chats = new Collection<Chat>();
            profilePicSrc = "https://www.history.ox.ac.uk/sites/default/files/history/images/person/unknown_9.gif";
            last =null;
            lastdate = null;
            message = "";
            lastMessage = null;
            token = "";
        }

        //[Required]
        // public int Id { get; set; }
        public string token { get; set; }
        public string message { get; set; }
        public string id { get; set; }
/*        [Required] 
*/        [DataType(DataType.Password)]
        public string password { get; set; }
        
        public Message1 lastMessage { get; set; }
        public string name { get; set; }
        public string server { get; set; }
        public ICollection<User2> contacts { get; set; }

        public string last { get; set; }

        public string lastdate { get; set; }  
        public ICollection<Chat> chats { get; set; }
        public string profilePicSrc { get; set; }


    }
}
