/*using Microsoft.AspNetCore.Mvc;
using Whatsapp2Server.Services;
using Whatsapp2Server.Models;
using Whatsapp2Server.services;

*//*namespace Whatsapp2Server.Controllers
{


    [ApiController]
    [Route("api/")]
    public class LoggedInUserAPIController : Controller
    {
        private readonly LoggedInApiService _service;
        //[HttpGet]
        public LoggedInUserAPIController()//Whatsapp2ServerContext context)
        {
            _service = new LoggedInApiService();
        }

        [HttpPost]
        public IActionResult update(string username)
        {
            _service.Update(username);
            //return BadRequest();
        }

    }
}*//*


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
    [Route("api/contacts")]
    public class LoggedInUserApiController : Controller
    {
        private readonly LoggedInApiService _service;


        [HttpPost("logIn")]
        // public async Task<IActionResult> Create([Bind("UserName, Password, NickName")] User user)
        public IActionResult Create([Bind("Chats, ProfilePicSrc, ServerName,Id, UserName, Password, NickName, Contacts")] User user)
        {
            if (ModelState.IsValid)
            {
                Signin(user);
                _service.Update(user);
                return Created(string.Format("api/contacts/logIn", user.UserName), user);
            }
            return BadRequest();
        }

        public LoggedInUserApiController()//Whatsapp2ServerContext context)
        {
            _service = new LoggedInApiService();
        }
        [HttpGet]
        public IActionResult sendContacts()
        {
            // if (_service.Get().UserName != "")
            // {
            return Json(_service.Contacts());
            // }

        }


        [HttpGet("{id}")]         //id = username (!)
        public IActionResult getSpecificContact(string id)
        {
            User user = _service.Contacts().FirstOrDefault(x => x.UserName == id);
            if(user.UserName == id)
            //if(user!=null)
            {
                return Json(user);
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

    }
}
        // GET: Users
        */