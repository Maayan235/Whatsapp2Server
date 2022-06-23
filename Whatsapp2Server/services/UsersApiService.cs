using Whatsapp2Server.Models;

namespace Whatsapp2Server.Services
{
    public class UsersApiService
    {
        private static ICollection<User2> users = new List<User2>();
        

        
        public UsersApiService()
        {
            /*if (users.Count == 0)
            {
                User2 user = new User2() {id = "Yarin", server = "localhost:5286", name = "Yerin", password = "123456", profilePicSrc = "" };
                User2 defUser2 = new User2() { id = "Maayan", server = "localhost:5286", name = "SaTla", password = "123456", profilePicSrc = "" };
                User2 defUser3 = new User2() { id = "Avital", server = "localhost:5286", name = "vita", password = "123456", profilePicSrc = "" };
                users.Add(user);
                users.Add(defUser2);
                users.Add(defUser3);
                Message1 m1 = new Message1() {id = -1, fromMe = true, content = "hiiiiiii", from = "Yarin", time = DateTime.Now };
                Chat chat = new Chat();
                if (chat.contacts.Count() == 0)
                {
                    chat.contacts.Add("Yarin");
                    chat.contacts.Add("Maayan");
                    chat.messages.Add(m1);
                }
                    user.chats.Add(chat);
                AddToContacts(user.id, defUser2);
            }*/
        }
        
        public string getToken(string id)
        {
            //return users.FirstOrDefault(x => x.id == id).token;
            return "g";
        }
        public Chat getChat(string username, string contactName)
        {
            User2 user = GetUser(username);
            User2 contact = GetUser(contactName);
            Chat chat =user.chats.FirstOrDefault(x => x.contacts.Contains(user.id)&& x.contacts.Contains( contact.id));
            return chat;
        }
        public void editContact(User2 contact)
        {
            User2 user = GetUser(contact.id);
            user.server = contact.server;
            user.name = contact.name;
            Console.WriteLine("hi");

        }
        public void setToken (Token token)
        {
            //User2 user = users.FirstOrDefault(x => x.id == token.id);
            //user.token = token.token;
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

        public User2 Create(User2 user)
        {
            if (users.FirstOrDefault(x=> x.id == user.id) != null)
            {
                return null;
            }
            User2 newUser = new User2();
            newUser.id = user.id;   
            newUser.server = "5286";
            newUser.name = user.name;
            newUser.password = user.password;
            return newUser;
        }
        public Contact convertToContact(User2 user)
        {
            Contact contact = new Contact();
            contact.id = user.id;
            contact.server = user.server;
            contact.name = user.name;
            //contact.prifilePicSrc = user.profilePicSrc;
            return contact;
        }

        public Contact convertToContactWithToken(User2 user)
        {
            Contact contact = new Contact();
            contact.id = user.id;
            contact.server = user.server;
            contact.name = user.name;
            contact.jwtToken = user.jwtToken;
            //contact.prifilePicSrc = user.profilePicSrc;
            return contact;
        }

    }
}