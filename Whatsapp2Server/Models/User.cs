using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Whatsapp2Server.Models
{
    public class User
    {
        public User()
        {
            Id = 0;
            UserName = "";
            Password = "";
            NickName = "";
            ServerName = "";
            Contacts = new Collection<User>();
            Chats = new Collection<Chat>();
            ProfilePicSrc = "";
        }

        //[Required]
        public int Id { get; set; }
        //[Required]
        public string UserName { get; set; }
/*        [Required] 
*/        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public string NickName { get; set; }
        public string ServerName { get; set; }
        public ICollection<User> Contacts { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public string ProfilePicSrc { get; set; }

    }
}
