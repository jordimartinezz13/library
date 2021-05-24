using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pt1_mvc.Models;
using Microsoft.AspNetCore.Http;

namespace pt1_mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly bankContext dataContext;

        public UserController(bankContext context)
        {
            dataContext = context;
        }

        // GET: User/Login
        public IActionResult Login()
        {
            return View();
        }


        // POST: Client/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("UserName, Password")] User user)
        {
            var user0 = dataContext.Users.FirstOrDefault(u => (
                (u.UserName == user.UserName) &&
                (u.Password == user.Password)
            ));

            if (user0 != null) {
                HttpContext.Session.SetString("userName", user0.UserName);
                return RedirectToAction("Index", "Home");
            } else {
                ViewBag.missatge = "LOGIN INCORRECTE. Torni a introduir les credencials";
                return View();
            }
        }

        // GET: User/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
