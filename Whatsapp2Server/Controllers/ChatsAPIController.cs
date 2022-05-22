/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Whatsapp2Server.Data;
using Whatsapp2Server.Models;
using Whatsapp2Server.services;

namespace Whatsapp2Server.Controllers
{
    public class ChatsAPIController : Controller
    {
        private readonly IChat _service;

        public ChatsAPIController(Whatsapp2ServerContext context)
        {
            _service = new ChatService();
        }

        // GET: Chats
        public async Task<IActionResult> Index()
        {
              return View( _service.GetAll());
        }

        // GET: Chats/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _service.GetAll() == null)
            {
                return NotFound();
            }

            var chat = _service.GetAll().FirstOrDefault(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // GET: Chats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                _service.Add(chat);
                return RedirectToAction(nameof(Index));
            }
            return View(chat);
        }

        // POST: Chats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Search(string query)
        {

            return View(_service.GetAll());
*//*            var q = 

            return View(_service.GetAll().ToList());*//*
        }

        // GET: Chats/Edit/5
        *//*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _service.getAll() == null)
            {
                return NotFound();
            }

            var chat = _service.getAll().FirstOrDefault(c=>c.Id == id);
            if (chat == null)
            {
                return NotFound();
            }
            return View(chat);
        }
        
        // POST: Chats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Chat chat)
        {
            if (id != chat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatExists(chat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chat);
        }*//*

        // GET: Chats/Delete/5
        *//* public async Task<IActionResult> Delete(int? id)
         {
             if (id == null || _context.Chat == null)
             {
                 return NotFound();
             }

             var chat = await _context.Chat
                 .FirstOrDefaultAsync(m => m.Id == id);
             if (chat == null)
             {
                 return NotFound();
             }

             return View(chat);
         }

         // POST: Chats/Delete/5
         [HttpPost, ActionName("Delete")]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> DeleteConfirmed(int id)
         {
             if (_context.Chat == null)
             {
                 return Problem("Entity set 'Whatsapp2ServerContext.Chat'  is null.");
             }
             var chat = await _context.Chat.FindAsync(id);
             if (chat != null)
             {
                 _context.Chat.Remove(chat);
             }

             await _context.SaveChangesAsync();
             return RedirectToAction(nameof(Index));
         }

         private bool ChatExists(int id)
         {
           return _context.Chat.Any(e => e.Id == id);
         }*//*
    }
}
*/