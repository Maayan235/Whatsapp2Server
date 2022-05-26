namespace Whatsapp2Server.Models
{
    public class MessageRet
    {

        private static int currentId = 0;
public MessageRet()
        {
            id = currentId;
            currentId++;
            this.content = "";
            this.created = DateTime.Now;
            this.sent = false;
           

        }
        
        public int id { get; set; }
        public string content { get; set; }
        public DateTime created { get; set; }
        public bool sent { get; set; }

       
    
    }
}
