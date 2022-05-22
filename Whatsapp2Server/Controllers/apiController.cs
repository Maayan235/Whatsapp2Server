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
    [Route("[controller]")]
    public class apiController : Controller
    {
        private readonly UsersApiService _service;
        public IConfiguration _configuration;

        public apiController(IConfiguration configuration) 
        {
            _service = new UsersApiService();
            _configuration = configuration;
        }
        [HttpPut("contacts4/{id}")]
        public IActionResult updateContact([Bind("name, server")]User2 contact, string id)
        {
            contact.id = id;
            _service.editContact(contact);
            return NoContent();
        }


        [HttpGet("messages/{contactId}")]
        public IActionResult getChat(string contactId)
        {
            string username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            Chat chat =_service.getChat(username, contactId);
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
            return BadRequest();
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
        [HttpGet("getUser/{username}")]
        public async Task<IActionResult> getUser(string username)
        {
            User2 user = _service.GetUser(username);
            if(user != null)
            {
                return Json(new User2() { id = user.id, password=user.password});
            }
            User2 defUser  = new User2() { id = "", name = "0", password = "0", profilePicSrc = "0" };
            return Json(defUser);
        }

        [HttpPost]
        // public async Task<IActionResult> Create([Bind("UserName, Password, NickName")] User user)
        public IActionResult Create([Bind("id, password, name")] User2 user)
        {
            if (ModelState.IsValid)
            {
                user.name = "5286";
                _service.Add(user);
                return Created(string.Format("api" + user.id, user.id), user);
            }
            return BadRequest();
        }


       

        [HttpGet("contacts1/{username}")]
        

        public IActionResult sendContacts(string username)
        {

            // check cookies ,, get the username of the connected user... (!)
            string username2 = HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            return Json(_service.Contacts(username2));
            // }

        }

       
        [HttpPost("contacts2/{username}")]
        
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

       /* [HttpDelete("contacts3/{id}")]
        public IActionResult deleteContact(string id)
        {
            //User2 contact = _service.GetUser(id);
            User2 thisUser = _service.GetUser("Yarin");
            _service.deleteContact(thisUser, id);
        }*/


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
       
        
        [HttpDelete("contacts3/{id}")]         //id = username (!)
        /*[Authorize]*/
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
            return Accepted(string.Format("api/contacts3/", id), _service.GetUser(thisUserName));
        }

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
        





        // GET: Users/Details/5
        /*[Authorize]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _service.Get((int)id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create([Bind("Id,UserName,Password,NickName,ProfilePicSrc")] User user)
        {
            if (ModelState.IsValid)
            {
                _service.Create(user.UserName, user.Password, user.NickName, user.ProfilePicSrc);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _service.Get((int)id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(int id, [Bind("Id,UserName,Password,NickName,ProfilePicSrc")] User user)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _service.Edit(id, user.UserName, user.Password, user.NickName, user.ProfilePicSrc);
                } catch (DbUpdateConcurrencyException)
                { 
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _service.Get((int)id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_service.Get((int)id) == null)
            {
                return Problem("Entity set 'Whatsapp2ServerContext.User'  is null.");
            }
            var user = _service.Get((int)id);
            if (user != null)
            {
                _service.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

        *//*        private bool UserExists(int id)
                {
                  return (_service.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
                }*//*

        // GET: Users/Create
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("Id,UserName,Password,NickName,ProfilePicSrc")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = from u in _service.GetAll()
                        where u.UserName == user.UserName
                        select u;

                if (q.Count() > 0)
                {
                    ViewData["Error"] = "Unable to comply; cannot register this user.";
                }
                else
                {
                    Signin(user);
                    //HttpContext.Session.SetString("username", user.UserName);
                    _service.Create(user.UserName, user.Password, user.NickName, user.ProfilePicSrc);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(user);
        }

        // GET: Users/Create
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("UserName,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = _service.GetAll().Where(u => u.UserName == user.UserName && u.Password == user.Password);
                
                if(q.Any())
                {
                    Signin(q.First());
                    //HttpContext.Session.SetString("username", q.First().UserName);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Error"] = "Username and/or password are incorrect.";
                }
            }
            return View(user);
        }

        private async void Signin(User account)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.UserName),
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

        [Authorize]
        public void Logout()
        {
            HttpContext.SignOutAsync();
        }

        // GET: Users/Create
        public IActionResult AccessDinied()
        {
            return View();
        }*/

    }
}
