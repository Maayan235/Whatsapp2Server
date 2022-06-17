    using Microsoft.AspNetCore.Mvc;
    using Whatsapp2Server.Models;
using Whatsapp2Server.Services;

namespace Whatsapp2Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _service;


        private static readonly List<Post> posts = new() { };

        public PostController(IConfiguration configuration)
        {
            _service = new PostService();

        }
            [HttpGet]
        public IEnumerable<Post> Get()
        {
            //return posts;
            return _service.get();
        }

        [HttpPost]
        public void Add([FromBody] Post post)
        {
            posts.Add(post);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            try
            {
                posts.Remove(posts.First(x => x.Id == id));
            }
            catch (Exception e) { }
        }
    }
}
