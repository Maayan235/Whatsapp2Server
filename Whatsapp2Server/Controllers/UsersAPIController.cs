using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Whatsapp2Server.Data;
using Whatsapp2Server.Models;
using Whatsapp2Server.Services;

namespace Whatsapp2Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersAPIController : Controller
    {
        private readonly UsersApiService _service;


        public UsersAPIController()//Whatsapp2ServerContext context)
        {
            _service = new UsersApiService();
        }


        // GET: Users
        [HttpGet("{username}")]
        public async Task<IActionResult> getUser(string username)
        {
            User user = _service.GetUser(username);
            if(user != null)
            {
                return Json(user);
            }
            User defUser  = new User() { Id = 1000, UserName = "0", Password = "0", NickName = "0", ProfilePicSrc = "0" };
            return Json(defUser);
        }

        [HttpPost]
        // public async Task<IActionResult> Create([Bind("UserName, Password, NickName")] User user)
        public IActionResult Create([Bind("UserName, Password, NickName")] User user)
        {
            if (ModelState.IsValid)
            {
                _service.Add(user);
                return Created(string.Format("api/[Controller]/" + user.Id, user.Id), user);
            }
            return BadRequest();
        }

        [HttpPost("logIn")]
        // public async Task<IActionResult> Create([Bind("UserName, Password, NickName")] User user)
        public IActionResult login([Bind("Chats, ProfilePicSrc, ServerName,Id, UserName, Password, NickName, Contacts")] User user)
        {
            if (ModelState.IsValid)
            {
                Signin(user);
                //_service.Update(user);
                return Created(string.Format("api/contacts/logIn", user.UserName), user);
            }
            return BadRequest();
        }

        [HttpGet("contacts")]
        [Authorize]
        public IActionResult sendContacts()
        {
            
            // check cookies ,, get the username of the connected user... (!)
            var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value; 

            return Json(_service.Contacts(username));
            // }

        }

        [HttpGet("contacts/{id}")]         //id = username (!)
        [Authorize]
        public IActionResult getSpecificContact(string id)
        {
            

            // check cookies ,, get the username of the connected user... (!)
            var thisUserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;  
            //var thisUserName = ... 
            User user2 = _service.Contacts(thisUserName).FirstOrDefault(x => x.UserName == id);
            if (user2.UserName == id)
            //if(user!=null)
            {
                return Json(user2);
            }
            return BadRequest();

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
