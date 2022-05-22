using Whatsapp2Server.Models;

namespace Whatsapp2Server.Services
{
    public class UsersApiService
    {
        private static ICollection<User> users = new List<User>();

        public UsersApiService()
        {
            if (users.Count == 0)
            {
                User user = new User() { UserName = "Yarin", ServerName = "5286", Id = 100, NickName = "Yerin", Password = "123456", ProfilePicSrc = "" };
                User defUser2 = new User() { UserName = "Maayan", ServerName = "5286", Password = "123456", NickName = "Shakira Shakira", ProfilePicSrc = "" };
                User defUser3 = new User() { UserName = "Maayan1", ServerName = "5286", Password = "123456", NickName = "Shakira Shakira", ProfilePicSrc = "" };
                users.Add(user);
                users.Add(defUser2);
                users.Add(defUser3);
                AddToContacts(user.UserName, defUser2.Id, defUser2.UserName);
            }
        }

        public User GetUser(string username)
        {
            User user= users.FirstOrDefault(x => x.UserName == username);
            if(user != null){
                if (user.UserName == username)
                {
                    return user;
                }
               
            }
            return null;
        }
        public void Add(User user)
        {
            users.Add(user);
        }

        public ICollection<User> Contacts(string username)
        {
            User user = users.FirstOrDefault(x => x.UserName == username);
            return user.Contacts;
        }

        public void AddToContacts(string username,User newContact)
        {
            User user = users.FirstOrDefault(x => x.UserName == username);
            User contact = users.FirstOrDefault(x => x.UserName == contactname);
            user.Contacts.Add(contact);
/*            User  bdika = user.Contacts.FirstOrDefault(x => x.UserName == contactname);
*/        }

        /*public bool Create()
        {
            return users.FirstOrDefault(x => x.Id == id);
        }*/

    }
}