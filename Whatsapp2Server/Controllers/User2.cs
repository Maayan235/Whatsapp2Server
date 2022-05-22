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
            server = "5286";
            contacts = new Collection<User2>();
            chats = new Collection<Chat>();
            profilePicSrc = "";
        }
         
        //[Required]
       // public int Id { get; set; }
        
        public string id { get; set; }
/*        [Required] 
*/        [DataType(DataType.Password)]
        public string password { get; set; }
        
        public string name { get; set; }
        public string server { get; set; }
        public ICollection<User2> contacts { get; set; }
        public ICollection<Chat> chats { get; set; }
        public string profilePicSrc { get; set; }

    }
}
