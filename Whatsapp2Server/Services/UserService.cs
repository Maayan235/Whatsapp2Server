using Whatsapp2Server.Models;

namespace Whatsapp2Server.Services
{
    public class UserService : IUserService
    {
        private static ICollection<User> users = new List<User>();

        public ICollection<User> GetAll()
        {
            return users;
        }

        public User Get(int id)
        {
            return users.FirstOrDefault(x => x.Id == id);
        }

        public void Delete(int id)
        {
           users.Remove(Get(id));
        }

        public void Create(string userName, string password, string nickName, string profilePicSrc)
        {
            int nextId = 0;
            if (users.Count > 0)
            {
                nextId = users.Max(x => x.Id) + 1;
            } 
            users.Add(new User() { Id = nextId, UserName = userName, Password = password, NickName = nickName, ProfilePicSrc = profilePicSrc});
        }


        public void Edit(int id, string userName, string password, string nickName, string profilePicSrc)
        {
            User user = Get(id);
            user.UserName = userName;
            user.Password = password;
            user.NickName = nickName;
            user.ProfilePicSrc = profilePicSrc;
        }

    }
}
