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
                User2 user = new User2() { id = "Yarin", server = "localhost:5286", name = "Yerin", password = "123456", profilePicSrc = "" };

                User2 defUser2 = new User2() { id = "Maayan", server = "localhost:5286", name = "satla", password = "123456", profilePicSrc = "", lastMessage = m1 };

                User2 defUser3 = new User2() { id = "Avital", server = "localhost:5286", name = "vita", password = "123456", profilePicSrc = "" };

                Contacts contact = new Contacts() { id = "Yarin", contacts = new Collection<User2>() { (defUser2),(defUser3) } };

                Contacts contact2 = new Contacts() { id = "Maayan", contacts = new Collection<User2>() { (user) } };
                Contacts contact3 = new Contacts() { id = "Avital", contacts = new Collection<User2>()  };

                contactsList.Add(contact);
                contactsList.Add(contact2);
                contactsList.Add(contact3);
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
            ICollection <User2> id2Contacts = contactsList.FirstOrDefault(x => x.id == id2).contacts;
            if(id2Contacts.FirstOrDefault(x=>x.id == user.id) == null)
            {
                id2Contacts.Add(user);
            }
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
            myContact.lastdate = message.time.ToString();
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
                myContact.lastdate = message.time.ToString();
                return 0;
            }


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
        public Contact fromUserToContact(User2 user)
        {
            Contact contact = new Contact();
            contact.id = user.id;
            contact.name = user.name;
            contact.server = user.server;
            contact.last = user.last;
            contact.lastdate = user.lastdate;
            return contact;
        }
        public ICollection<Contact> fromUsersToContacts(ICollection<User2> contactsList)
        {
            ICollection<Contact> contacts = new Collection<Contact>();
            foreach (User2 contact in contactsList)
            {
                contacts.Add(fromUserToContact(contact));
            }
            return contacts;
        }

        public MessageRet convertMessage(Message oldMessage, string id)
        {
            MessageRet message = new MessageRet();
            message.id = oldMessage.id;
            message.created = oldMessage.time;
            message.content = oldMessage.content;
            if(oldMessage.from == id)
            {
                message.sent = true;
            }
            return message;

        }
        public ICollection<MessageRet> convertMessages(ICollection<Message> oldMessages, string id)
        {
            ICollection < MessageRet > messages = new Collection<MessageRet>();
            foreach (Message oldMessage in oldMessages)
            {
                messages.Add(convertMessage(oldMessage, id));
            }
            return messages;
        }



    }
}