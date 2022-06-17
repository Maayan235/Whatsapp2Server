namespace Whatsapp2Server.Models
{
    public class Post
    {
        public Post(int id, string author, string content, int likes)
        {
            Id = id;
            Author = author;
            Content = content;
            Likes = likes;
        }

        public int Id { get; set; }

        public string Author{ get; set; }

        public string Content { get; set; }

        public int Likes { get; set; }
    }
}
