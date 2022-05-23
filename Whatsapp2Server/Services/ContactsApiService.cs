using System.Collections.ObjectModel;
using Whatsapp2Server.Models;

namespace Whatsapp2Server.Services
{
    public class ContactsApiService
    {
        private static ICollection<Contacts> contacts = new List<Contacts>();
        private static ICollection<Chat> chats = new List<Chat>();


        public ContactsApiService()
        {
            if (contacts.Count == 0)
            {
                User2 user = new User2() {id = "Yarin", server = "5286", name = "Yerin", password = "123456", profilePicSrc = "" };
                User2 defUser2 = new User2() { id = "Maayan", server = "5286", name = "satla", password = "123456", profilePicSrc = "" };
                User2 defUser3 = new User2() { id = "Avital", server = "5286", name = "vita", password = "123456", profilePicSrc = "" };

                Contacts contact = new Contacts() { id = "Yarin", contacts = new Collection<User2>() { (defUser2) } };
                contacts.Add(contact);
                Message m1 = new Message() {id = -1, fromMe = true, content = "hiiiiiii", senderId = "Yarin", time = DateTime.Now };
                Chat chat = new Chat();
                chat.contacts[0] = "Yarin";
                chat.contacts[1] = "Maayan";
                chat.messages.Add(m1);
                chats.Add(chat);
                
            }
        }


        public ICollection<User2> getContacts(string id)
        {
            Contacts myContacts = contacts.FirstOrDefault(x => x.id == id);
            return myContacts.contacts;
        }
        public void addContact(string id, User2 contact)
        {
            Contacts myContacts = contacts.FirstOrDefault(x => x.id == id);
            myContacts.contacts.Add(contact);
        }
        public Chat getChat(string id1, string id2)
        {
            Contacts myContacts = contacts.FirstOrDefault(x => x.id == id1);
            if(myContacts.contacts.Count > 0)
            {
                if (myContacts.contacts.Contains(myContacts.contacts.FirstOrDefault(x=> x.id == id2))) 
                {
                    return chats.FirstOrDefault(x => x.contacts.Contains(id1) && x.contacts.Contains(id2));
                }
                return null;
            }
            return null;

            
        }

       
    }
}