namespace Whatsapp2Server.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public ICollection<User> contacts { get; set; }
        public ICollection<Message> messages { get; set; }
    }
}
