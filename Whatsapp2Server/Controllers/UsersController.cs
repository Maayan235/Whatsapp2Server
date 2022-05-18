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
    public class UsersController : Controller
    {
        private readonly IUserService _service;


        public UsersController(Whatsapp2ServerContext context)
        {
            _service = new UserService();
        }

        // GET: Users
        [Authorize]
        public IActionResult Index()
        {
              return View(_service.GetAll());
        }

        // GET: Users/Details/5
        [Authorize]
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

        /*        private bool UserExists(int id)
                {
                  return (_service.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
                }*/

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
        }

    }
}
