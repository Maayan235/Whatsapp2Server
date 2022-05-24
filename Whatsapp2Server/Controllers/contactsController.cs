using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Whatsapp2Server.Data;
using Whatsapp2Server.Models;
using Whatsapp2Server.Services;

namespace Whatsapp2Server.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class contatsController : Controller
    {
        private readonly ContactsApiService _service;
        public IConfiguration _configuration;

        public contatsController(IConfiguration configuration)
        {
            _service = new ContactsApiService();
            _configuration = configuration;
        }
       



        // public async Task<IActionResult> Create([Bind("UserName, Password, NickName")] User user)


        [HttpGet]
        public IActionResult sendContacts()
        {

            // check cookies ,, get the username of the connected user... (!)
            string username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;

             ICollection<User2> contacts =  _service.getContacts(username);
            if(contacts == null)
            {
                return null;
            }
            return Json(contacts);
            // }

        }
        [HttpGet("{id}/messages")]
        public IActionResult getMessages(string id)
        {
           
            string username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            Chat chat = _service.getChat(username, id);
            if (chat == null)
            {
                Chat chat2= new Chat();
                chat2.contacts.Append(username);
                chat2.contacts.Append(id);
                return Json(chat2);
            }
                
            return Json(chat);
        }

         
        [HttpPost("{id}/messages")]
        public IActionResult postMessages([Bind("content")] Message message, string id) 
        {
            string username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            User2 myContact = _service.getContacts(username).FirstOrDefault(x => x.id == id);
            
            Chat chat = _service.getChat(username, id);
            message.from = username;
            message.to = id;
            chat.messages.Add(message);
            chat.lastMessage = message;
            myContact.lastMessage = message;
            myContact.last = message.content;
            myContact.lastdate = message.time;
             
            return Created(string.Format("api/contacts/", id + "messages"), id);
        }


        [HttpGet("{id}/lastMessage")]
        public IActionResult lastMessage(string id)
        {
            string username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            User2 myContact = _service.getContacts(username).FirstOrDefault(x => x.id == id);
            return Json(myContact.lastMessage);
        }
        [HttpPost]

        public IActionResult AddContact([Bind("id,server,name")] User2 contact)
        {
            contact.profilePicSrc = "https://www.history.ox.ac.uk/sites/default/files/history/images/person/unknown_9.gif";
            string id = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            _service.addContact(id, contact);
            return Created(string.Format("api/contacts/", contact.id), contact);
        }

        /* [HttpDelete("contacts3/{id}")]
         public IActionResult deleteContact(string id)
         {
             //User2 contact = _service.GetUser(id);
             User2 thisUser = _service.GetUser("Yarin");
             _service.deleteContact(thisUser, id);
         }*/


        [HttpGet("{id}")]         //id = username (!)
        [Authorize]
        public IActionResult getSpecificContact(string id)
        {
            var thisUserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            ICollection<User2> myContacts = _service.getContacts(thisUserName);
            if (myContacts.Count > 0)
            {
                User2 contact = myContacts.FirstOrDefault(x => x.id == id);
                if ( contact != null && contact.id == id)
                {
                    return Json(contact);
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        // todo: change
        // Put contact id
        [HttpPut("{id}")]         //id = username (!)
        [Authorize]
        public IActionResult EditSpecificContact([Bind("ServerName,NickName")] User2 newContact, string id)
        {

            var thisUserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            ICollection<User2> myContacts = _service.getContacts(thisUserName);
            if (myContacts.Count > 0)
            {
                User2 contact = myContacts.FirstOrDefault(x => x.id == id);
                if (contact.id == id)
                {
                    contact.server = newContact.server;
                    contact.name = newContact.name;
                    return Accepted(string.Format("api/contacts/", thisUserName), id);

                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }



        // todo: check
        // Delete contact id


        [HttpDelete("{id}")]         //id = username (!)
        /*[Authorize]*/
        public IActionResult DeleteSpecificContact(string id)
        {

            var thisUserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            ICollection<User2> myContacts = _service.getContacts(thisUserName);
            //var thisUserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            myContacts.Remove(myContacts.FirstOrDefault(x => x.id == id));
            return Accepted(string.Format("api/contacts/", id), id);


        }
    }
}