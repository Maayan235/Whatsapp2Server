using Whatsapp2Server.Models;

namespace Whatsapp2Server.Services
{
    public class UsersApiService
    {
        private static ICollection<User2> users = new List<User2>();


        
        public UsersApiService()
        {
            if (users.Count == 0)
            {
                User2 user = new User2() {id = "Yarin", server = "5286", name = "Yerin", password = "123456", profilePicSrc = "" };
                User2 defUser2 = new User2() { id = "Maayan", server = "5286", name = "satla", password = "123456", profilePicSrc = "" };
                User2 defUser3 = new User2() { id = "Avital", server = "5286", name = "vita", password = "123456", profilePicSrc = "" };
                users.Add(user);
                users.Add(defUser2);
                users.Add(defUser3);
                Message m1 = new Message() {id = -1, fromMe = true, content = "hiiiiiii", senderId = "Yarin", time = DateTime.Now };
                Chat chat = new Chat();
                chat.contacts[0] = "Yarin";
                chat.contacts[1] = "Maayan";
                chat.messages.Add(m1);
                user.chats.Add(chat);
                AddToContacts(user.id, defUser2);
            }
        }
        public Chat getChat(string username, string contactName)
        {
            User2 user = GetUser(username);
            User2 contact = GetUser(contactName);
            Chat chat =user.chats.FirstOrDefault(x => Array.Exists( x.contacts,element => element == user.id) && Array.Exists(x.contacts, element => element == contact.id));
            return chat;
        }
        public void editContact(User2 contact)
        {
            User2 user = GetUser(contact.id);
            user.server = contact.server;
            user.name = contact.name;
            Console.WriteLine("hi");

        }
        public void deleteContact(User2 thisUser, string contactId)
        {
            User2 contact = thisUser.contacts.FirstOrDefault(x => x.id == contactId);
            thisUser.contacts.Remove(contact);
        }

        public User2 GetUser(string username)
        {
            User2 user= users.FirstOrDefault(x => x.id == username);
            if(user != null){
                if (user.id == username)
                {
                    return user;
                }
               
            }
            return null;
        }
        public void Add(User2 user)
        {
            users.Add(user);
        }

        public ICollection<User2> Contacts(string username)
        {
            User2 user = users.FirstOrDefault(x => x.id == username);
            return user.contacts;
        }

        public void AddToContacts(string username,User2 newContact)
        {
            User2 user = users.FirstOrDefault(x => x.id == username);
            //User2 contact = users.FirstOrDefault(x => x.id == newContact.id);
            user.contacts.Add(newContact);
/*            User  bdika = user.Contacts.FirstOrDefault(x => x.UserName == contactname);
*/        }

        /*public bool Create()
        {
            return users.FirstOrDefault(x => x.Id == id);
        }*/

    }
}