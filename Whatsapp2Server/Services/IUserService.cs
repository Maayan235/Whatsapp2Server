using Whatsapp2Server.Models;

namespace Whatsapp2Server.Services
{
    public interface IUserService
    {
        public ICollection<User> GetAll();
        public User Get(int id);

        public void Delete(int id);

        public void Create(string userName, string password, string nickName, string profilePicSrc);

        public void Edit(int id, string userName, string password, string nickName, string profilePicSrc);
    }
}
