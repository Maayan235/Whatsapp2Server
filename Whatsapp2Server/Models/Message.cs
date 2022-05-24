namespace Whatsapp2Server.Models
{
    public class Message
    {

        private static int currentId = 0;
public Message()
        {
            id = currentId;
            currentId++;
            this.content = "";
            this.time = DateTime.Now;
            this.fromMe = false;
            this.senderId = "";
            this.chat = null;
        }

        public int id { get; set; }
        public string content { get; set; }
        public DateTime time { get; set; }
        public bool fromMe { get; set; }

        public string senderId { get; set; }
        public Chat chat{ get; set; } 
    
    }
}
