using Whatsapp2Server.Models;

namespace Whatsapp2Server.services
{
    public class LoggedInApiService
    {
        private static User loggedIn = new User { Id = 0, UserName = "", Password = "", NickName = "", ProfilePicSrc = "" };
        

        public User Get()
        {
            return loggedIn;
        }
        public void Update(User user)
        {
            loggedIn = user;
        }

        public ICollection<User> Contacts()
        {
            return loggedIn.Contacts;
        }
    }
}
