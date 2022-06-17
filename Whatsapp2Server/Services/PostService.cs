using Microsoft.AspNetCore.Mvc;
using Whatsapp2Server.Models;

namespace Whatsapp2Server.Services
{
    public class PostService
    {
        private static List<Post> posts = new List<Post>();



        public PostService()
        {
            //List<Post> hardCoded = new() { };
            Post post1 = new Post(0, "ss", "hi", 10);
            Post post2 = new Post(1, "ss!", "hi!", 10);
            posts.Add(post1);
            posts.Add(post2);
        }

        public IEnumerable<Post> get()
        {
            return posts;
        }
    }
}
