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
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Whatsapp2Server.Data;
using Whatsapp2Server.Models;
using Whatsapp2Server.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace Whatsapp2Server.Controllers
{

    //[EnableCors("AnotherPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class apiController : Controller
    {
        private readonly ContactsApiService _contactservice;
        private static readonly HttpClient client = new HttpClient();
        private readonly UsersApiService _service;
        public IConfiguration _configuration;

        public apiController(IConfiguration configuration)
        {
            _service = new UsersApiService();
            _contactservice = new ContactsApiService();
            
            _configuration = configuration;
        }
        /* [HttpPut("contacts/{id}")]
         public IActionResult updateContact([Bind("name, server")]User2 contact, string id)
         {
             contact.id = id;
             _service.editContact(contact);
             return NoContent();
         }*/

        [HttpPost("transfer")]
        public IActionResult transfer([Bind("content", "from", "to")] Message message)
        {
            if(_contactservice.addMessage(message.content, message.to, message.from) == 0)
            {
                return Created(string.Format("api/transfer"), message.content);
            }
            return NotFound();    

        }
        [HttpPost("invitations")]
        public IActionResult invitations([Bind( "from", "to","sever")] Invitation invitation)
        {
            if(_service.GetUser(invitation.to) == null)
            {
                return NotFound();
            }
            string username = invitation.to;
            User2 contactToAdd = new User2();
            contactToAdd.id = invitation.from;
            contactToAdd.server = invitation.server;
            contactToAdd.name = invitation.from;
            _contactservice.addContact(username, contactToAdd);
            return Created(string.Format("api/transfer"), invitation.to);
        }



        [HttpGet("messages/{contactId}")]
        public IActionResult getChat(string contactId)
        {
            string username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            Chat chat = _service.getChat(username, contactId);
            if (chat != null)
            {
                return Json(chat.messages);
            }
            return null;
        }

        // public async Task<IActionResult> Create([Bind("UserName, Password, NickName")] User user)
        [HttpPost("logIn")]
        public IActionResult login([Bind("id")] User2 user)
        {
            if (ModelState.IsValid)
            {
                User2 loggedIn = _service.GetUser(user.id);
                //     Post(user.UserName);
                Signin(loggedIn);
                //_service.Update(user);

                return Created(string.Format("api/logIn", loggedIn.id), loggedIn.id);
            }
            return NotFound();
        }

        [HttpPost("signIn/{username}")]
        public IActionResult Post(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["JWTParams:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, DateTime.UtcNow.ToString()),
                new Claim("UserID", username)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTParams:SecretKey"]));
            var mac = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JWTParams:Issuer"],
                _configuration["JWTParams:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: mac);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        // GET: Users
        //[EnableCors("AnotherPolicy")]
        [HttpGet("getUser/{username}")]
        public async Task<IActionResult> getUser(string username)
        {
            User2 user = _service.GetUser(username);
            if (user != null)
            {
                return Json(new User2() { id = user.id, password = user.password });
            }
            User2 defUser = new User2() { id = "", name = "0", password = "0", profilePicSrc = "0" };
            return Json(defUser);
        }

        [HttpPost]
        // public async Task<IActionResult> Create([Bind("UserName, Password, NickName")] User user)
        public IActionResult Create([Bind("id, password, name")] User2 user)
        {
            if (ModelState.IsValid)
            {
                
                User2 newUser = _service.Create(user);
                if(newUser == null)
                {
                    return NotFound();
                }
                _contactservice.createContacts(user.id);
                _service.Add(newUser);
                return Created(string.Format("api" + user.id, user.id), user);
            }
            return NotFound();
        }



        /*
                [HttpGet("contacts1")]


                public IActionResult sendContacts(string username)
                {

                    // check cookies ,, get the username of the connected user... (!)
                    string username2 = HttpContext.User.FindFirst(ClaimTypes.Name).Value;

                    return Json(_service.Contacts(username2));
                    // }

                }


                [HttpPost("contacts")]

                public IActionResult AddContact(string username, [Bind("id,server,name")] User2 contact)
                {
                    User2 newContact = null, copyContact = null;
                    bool otherServerContact = false;
                    if (contact.server == "5286")
                    {
                        newContact = _service.GetUser(contact.id);
                        if (newContact != null)
                        {
                            copyContact = new User2() { id = contact.id, name = contact.name, server = contact.server, profilePicSrc = newContact.profilePicSrc };
                        }
                    }
                    else
                    {
                        copyContact = contact;
                        otherServerContact = true;
                    }
                    if (newContact != null || otherServerContact)
                    {
                        string id = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                        User2 thisUser = _service.GetUser(id);
                        User2 bdika = thisUser.contacts.FirstOrDefault(x => x.id == contact.id);
                        if (thisUser.contacts.FirstOrDefault(x => x.id == contact.id) == null)
                        {
                            // User copyContact = new User() { Id = contact.Id, NickName = contact.NickName, ServerName = contact.ServerName, ProfilePicSrc = newContact.ProfilePicSrc };
                            _service.AddToContacts(id, copyContact);
                            return Created(string.Format("api/contacts/", contact.id), contact);
                        }
                        else
                        {
                            // already have this contact
                            return BadRequest();
                        }
                    }
                    //User doesn't exist
                    return NotFound();
                }

               *//* [HttpDelete("contacts3/{id}")]
                public IActionResult deleteContact(string id)
                {
                    //User2 contact = _service.GetUser(id);
                    User2 thisUser = _service.GetUser("Yarin");
                    _service.deleteContact(thisUser, id);
                }*//*


                [HttpGet("contacts/{id}")]         //id = username (!)
                [Authorize]
                public IActionResult getSpecificContact(string id)
                {


                    // check cookies ,, get the username of the connected user... (!)
                    var thisUserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;  
                    //var thisUserName = ... 
                    User2 user = _service.Contacts(thisUserName).FirstOrDefault(x => x.id == id);
                    if (user.id == id)
                    //if(user!=null)
                    {
                        return Json(user);
                    }
                    return BadRequest();
                }

                // todo: change
                // Put contact id
                [HttpPut("contacts/{id}")]         //id = username (!)
                [Authorize]
                public IActionResult EditSpecificContact([Bind("ServerName,NickName")] User2 contact, string id)
                {

                    var thisUserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                    // todo: to change to id 
                    User2 editedContact = _service.Contacts(thisUserName).FirstOrDefault(x => x.id == id);
                    if (editedContact == null)
                    {
                        return BadRequest();
                    }
                    editedContact.server = contact.server;
                    editedContact.name = contact.name;
                    return Accepted(string.Format("api/contacts/", thisUserName), _service.GetUser(thisUserName));
                }

                // todo: check
                // Delete contact id


                [HttpDelete("contacts/{id}")]         //id = username (!)
                *//*[Authorize]*//*
                public IActionResult DeleteSpecificContact(string id)
                {

                    //var thisUserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                    var thisUserName = "Yarin";

                    // todo: to change to id 
                    User2 contact = _service.Contacts(thisUserName).FirstOrDefault(x => x.id == id);
                    if (contact == null)
                    {
                        return BadRequest();
                    }
                    _service.Contacts(thisUserName).Remove(contact);
                    return Accepted(string.Format("api/contacts/", id), _service.GetUser(thisUserName));
                }
        */
        private async void Signin(User2 account)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.id),
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //ExpireUtc = DataTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

        }

    }
}



