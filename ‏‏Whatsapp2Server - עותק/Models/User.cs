using System.ComponentModel.DataAnnotations;

namespace Whatsapp2Server.Models
{
    public class User
    {
        //[Required]
        public int Id { get; set; }
        //[Required]
        public string UserName { get; set; }
        //[Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
//<<<<<<< HEAD
        public string nickName { get; set; }

        public string serverName { get; set; }
        public ICollection<User> contacts { get; set; }
        public ICollection<Chat> chats { get; set; }
        public string profilePicSrc { get; set; }
//=======
        public string NickName { get; set; }
        public ICollection<User> Contacts { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public string ProfilePicSrc { get; set; }
//>>>>>>> 15cc45c944c4343d6ad9ba66fe7d36d7d5506f6e

    }
}
