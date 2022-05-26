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
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/contacts")]
    public class contatsController : Controller
    {
        private readonly ContactsApiService _service;
        private readonly UsersApiService _usersService;
        public IConfiguration _configuration;
        public string loggedId;

        public contatsController(IConfiguration configuration)
        {
            _service = new ContactsApiService();
            _usersService = new UsersApiService();
            _configuration = configuration;
            //GetUserId();
        }

        private async void GetUserId()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            loggedId = jwt.Claims.First(x => x.Type == "UserId").Value;

        }
       
        // public async Task<IActionResult> Create([Bind("UserName, Password, NickName")] User user)


        [HttpGet]
        public IActionResult sendContacts()
        {
            //string username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            string username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;


            ICollection<User2> myContacts =  _service.getContacts(username);
            if(myContacts == null)
            {
                return null;
            }
            ICollection<Contact> contacts = _service.fromUsersToContacts(myContacts);
            return Json(contacts);
            // }

        }
        [HttpGet("{id}/messages")]
        public IActionResult getmessages(string id)
        {
           
            string username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            Chat chat = _service.getChat(username, id);
            if (chat == null)
            {
                Chat chat2= new Chat();
                chat2.contacts.Add(username);
                chat2.contacts.Add(id);
                _service.addChat(chat2);
                return Json(_service.convertMessages( chat2.messages, username));
            }
                
            return Json(_service.convertMessages(chat.messages, username));
        }
        [HttpGet("chat/{id}")]
        public IActionResult getchat(string id)
        {

            string username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            Chat chat = _service.getChat(username, id);
            if (chat == null)
            {
                Chat chat2 = new Chat();
                chat2.contacts.Add(username);
                chat2.contacts.Add(id);
                _service.addChat(chat2);
                return Json(chat2);
            }

            return Json(chat);
        }
        [HttpPost("{id}/messages")]
        public IActionResult postMessages([Bind("content")] Message message, string id) 
        {
            string username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            if(_service.addMessage(message.content, username, id) == 0)
            {
                return Created(string.Format("api/contacts/", id + "messages"), id);

            }
            _service.addMessageInOther(message.content, username, id);

            return NotFound();
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
            string id = HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            if (contact.server != "localhost:5286")
            {
                User2 user2 = new User2();
                user2.server = contact.server;
                user2.id = contact.id;
                user2.name = contact.name;
                _service.addContact(id, contact);
                return Created(string.Format("api/contacts/", contact.id), contact);

            }
            else
            {
                if (_usersService.GetUser(contact.id) == null)
                {
                    return NotFound();
                }
                User2 user = new User2();
                user.name = contact.name;
                user.server = contact.server;
                User2 thisUser = _usersService.GetUser(id);
                _service.addContact(id, contact);
                _service.addContactInOther(thisUser, contact.id);
                return Created(string.Format("api/contacts/", contact.id), contact);
            }
        }



        /* [HttpDelete("contacts3/{id}")]
         public IActionResult deleteContact(string id)
         {
             //User2 contact = _service.GetUser(id);
             User2 thisUser = _service.GetUser("Yarin");
             _service.deleteContact(thisUser, id);
         }*/


        [HttpGet("{id}")]         //id = username (!)
        
        public IActionResult getSpecificContact(string id)
        {
            var thisUserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            ICollection<User2> myContacts = _service.getContacts(thisUserName);
            if (myContacts.Count > 0)
            {
                User2 contact = myContacts.FirstOrDefault(x => x.id == id);
                if ( contact != null && contact.id == id)
                {
                    return Json(_service.fromUserToContact( contact));
                }
                else
                {
                    return NotFound();
                }
            }
            return NotFound();
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
                    return NotFound();
                }
            }
            return NotFound();
        }

        [HttpGet("{id}/messages/{id2}")]
        public IActionResult getSpecificMessage(string id, int id2)
        {
            var thisUserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            Message message = _service.getSpecificMessage(thisUserName, id, id2);
            if (message == null)
                return NotFound();
            return Json(_service.convertMessage( message, thisUserName));
        }
        [HttpPut("{id}/messages/{id2}")]
        public IActionResult EditSpecificMessage([Bind("content")] Message message, string id, int id2)
        {
            var thisUserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            Message thisMessage = _service.getSpecificMessage(thisUserName, id, id2);
            if (thisMessage == null)
            {
                return NotFound();
            }
            thisMessage.content = message.content;
            return Ok();

            // todo: check
            // Delete contact id
        }
        [HttpDelete("{id}/messages/{id2}")]
        public IActionResult DeleteSpecificMessage(string id, int id2)
        {
            var thisUserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            Chat chat = _service.getChat(thisUserName, id);
            Message thisMessage = _service.getSpecificMessage(thisUserName, id, id2);
            if (thisMessage == null)
            {
                return NotFound();
            }
            chat.messages.Remove(thisMessage);
            return Ok();

            // todo: check
            // Delete contact id
        }


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