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
            this.time = DateTime.UtcNow;
            this.fromMe = false;
            this.from = "";
            this.to = "";
            

        }

        public string to { get; set; }
        public int id { get; set; }
        public string content { get; set; }
        public DateTime time { get; set; }
        public bool fromMe { get; set; }

        public string from { get; set; }
    
    }
}
