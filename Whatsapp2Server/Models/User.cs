using System.ComponentModel.DataAnnotations;

namespace Whatsapp2Server.Models
{
    public class User
    {
        //[Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public string nickName { get; set; }

        public string serverName { get; set; }
        public ICollection<User> contacts { get; set; }
        public ICollection<Chat> chats { get; set; }
        public string profilePicSrc { get; set; }

    }
}
