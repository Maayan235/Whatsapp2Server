namespace Whatsapp2Server.Models
{
    public class Message
    {
        public int Id { get; set; } 
        public string content { get; set; }
        public DateTime Date { get; set; }
        public int senderID { get; set; }
    
    }
}
