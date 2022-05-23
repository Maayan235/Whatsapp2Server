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

            return Json(_service.getContacts(username));
            // }

        }
        [HttpGet("{id}/messages")]
        public IActionResult getMessages(string id)
        {
            string username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            Chat chat = _service.getChat(username, id);
            if (chat == null)
                return BadRequest();
            return Json(chat.messages);
        }


        /*[HttpPost("{id}/messages")]
        public IActionResult postMessages([Bind("content")] Message message, string id) 
        {
            string username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            Chat chat = _service.getChat(username, id);
        }*/

        [HttpPost]

        public IActionResult AddContact([Bind("id,server,name")] User2 contact)
        {
            User2 copyContact = new User2() { id = contact.id, name = contact.name, server = contact.server };
            string id = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            _service.addContact(id, copyContact);
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
                if (contact.id == id)
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