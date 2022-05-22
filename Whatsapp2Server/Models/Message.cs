namespace Whatsapp2Server.Models
{
    public class Message
    {
public Message()
        {
            id = 0;
            this.message = "";
            this.time = DateTime.Now;
            this.fromMe = true;
            this.senderId = "";
            this.chat = null;
        }

        public int id { get; set; }
        public string message { get; set; }
        public DateTime time { get; set; }
        public bool fromMe { get; set; }

        public string senderId { get; set; }
        public Chat chat{ get; set; } 
    
    }
}
