namespace Whatsapp2Server.Models
{
    public class Chat
    {
        public int Id { get; set; }

        public HashSet<User> contacts { get; set; }

        public ICollection<Message> messages { get; set; }

        public Message LastMessage { get; set; }
    }
}
