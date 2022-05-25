using System.Collections.ObjectModel;
using Whatsapp2Server.Models;

namespace Whatsapp2Server.Services
{
    public class ContactsApiService
    {
        private static ICollection<Contacts> contactsList = new List<Contacts>();
        private static ICollection<Chat> chats = new List<Chat>();


        public ContactsApiService()
        {
            if (contactsList.Count == 0)
            {
                Message m1 = new Message() { id = -1, fromMe = true, content = "hiiii", from = "Maayan", to = "Yarin", time = DateTime.Now };
                User2 user = new User2() { id = "Yarin", server = "5286", name = "Yerin", password = "123456", profilePicSrc = "" };

                User2 defUser2 = new User2() { id = "Maayan", server = "5286", name = "satla", password = "123456", profilePicSrc = "", lastMessage = m1 };

                User2 defUser3 = new User2() { id = "Avital", server = "5286", name = "vita", password = "123456", profilePicSrc = "" };

                Contacts contact = new Contacts() { id = "Yarin", contacts = new Collection<User2>() { (defUser2) } };

                Contacts contact2 = new Contacts() { id = "Maayan", contacts = new Collection<User2>() { (user) } };

                contactsList.Add(contact);
                contactsList.Add(contact2);
                Chat chat = new Chat();
                chat.lastMessage = m1;
                chat.contacts.Add("Yarin");
                chat.contacts.Add("Maayan");
                chat.messages.Add(m1);
                chats.Add(chat);

            }
        }

        public Chat getChat(string id1, string id2)
        {

            Chat chat = chats.FirstOrDefault(x => x.contacts.Contains(id1) && x.contacts.Contains(id2));
            if (chat != null && chat.contacts.Contains(id1) && chat.contacts.Contains(id2))
            {
                return chat;
            }
            return null;


        }

        public void createContacts(string id)
        {
            Contacts contacts = new Contacts();
            contacts.id = id;
            contactsList.Add(contacts);
        }
        public void addContactInOther(User2 user , string id2)
        {
            User2 contact = new User2();
            contact.id = user.id;
            contact.name = user.name;
            contact.server = user.server;
            contactsList.FirstOrDefault(x => x.id == id2).contacts.Add(contact);
        }
        public ICollection<User2> getContacts(string id)
        {
            Contacts myContacts = contactsList.FirstOrDefault(x => x.id == id);
            if (myContacts == null)
            {
                return null;
            }
            return myContacts.contacts;
        }

        public Message getSpecificMessage(string thisUserId,string contactId,int messageId)
        {
            
                Chat chat = getChat(thisUserId, contactId);
                if (chat == null)
                {
                    return null;
                }
                Message messsage = chat.messages.FirstOrDefault(x => x.id == messageId);
                if (messsage == null)
                {
                    return null;
                }
            return messsage;
            
        }

        public int addMessage(string content, string id1, string id2)
        {
            User2 myContact = getContacts(id1).FirstOrDefault(x => x.id == id2);
            Chat chat = getChat(id1, id2);
            if (chat == null)
            {
                return -1;
            }
            Message message = new Message();
            message.from = id1;
            message.to = id2;
            message.content = content;
            chat.messages.Add(message);
            chat.lastMessage = message;
            myContact.lastMessage = message;
            myContact.last = message.content;
            myContact.lastdate = message.time;
            return 0;
        }

        public int addMessageInOther(string content, string id1, string id2)
        {
                User2 myContact = getContacts(id2).FirstOrDefault(x => x.id == id1);
                Chat chat = getChat(id1, id2);
                if (chat == null)
                {
                    return -1;
                }
                Message message = new Message();
                message.from = id1;
                message.to = id2;
                message.content = content;
                myContact.lastMessage = message;
                myContact.last = message.content;
                myContact.lastdate = message.time;
                return 0;
            }
                
                /*

            Chat chat = _service.getChat(username, id);
           
            chat.messages.Add(message);
            chat.lastMessage = message;
            myContact.lastMessage = message;
            myContact.last = message.content;
            myContact.lastdate = message.time;*/

        
        public void addContact(string id, User2 contact)
        {
            Contacts myContacts = contactsList.FirstOrDefault(x => x.id == id);
            if (myContacts == null)
            {
                Contacts newContacts = new Contacts() { id = id, contacts = new Collection<User2>() { contact } };
                contactsList.Add(newContacts);
            }
            else
            {
                myContacts.contacts.Add(contact);
            }
        }
        public void addChat(Chat chat)
        {
            chats.Add(chat);
        }
        
    }
}