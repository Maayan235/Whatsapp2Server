using Whatsapp2Server.Models;

namespace Whatsapp2Server.Services
{
    public class UsersApiService
    {
        private static ICollection<User> users = new List<User>();


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

        public void AddToContacts(string username, int id, string contactname)
        {
            User user = users.FirstOrDefault(x => x.UserName == username);
            User contact = users.FirstOrDefault(x => x.UserName == username && x.Id == id);
            user.Contacts.Add(contact);
        }

        /*public bool Create()
        {
            return users.FirstOrDefault(x => x.Id == id);
        }*/

    }
}