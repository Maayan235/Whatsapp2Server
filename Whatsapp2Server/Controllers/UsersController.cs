using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult Index()
        {
              return View(_service.GetAll());
        }

        // GET: Users/Details/5
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}
