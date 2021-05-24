using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pt1_mvc.Models;
using Microsoft.AspNetCore.Http;

namespace pt1_mvc.Controllers
{
    public class ClientController : Controller
    {
        private readonly bankContext dataContext;

        public ClientController(bankContext context)
        {
            dataContext = context;
        }

        private void VeureUser()
        {
            if (!(string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))) {
                var userName = HttpContext.Session.GetString("userName");
                ViewBag.userName = userName;
            }
        }

        // GET: Client
        public IActionResult Index()
        {
            VeureUser();

            var clients = dataContext.Clients;
            return View(clients.ToList());
        }

        // GET: MajorsEdat
        public IActionResult MajorsEdat()
        {
            VeureUser();
            
            // newDate és la data de naixement per tenir 18 anys exactament
            var myDate = DateTime.Now;
            var newDate = myDate.AddYears(-18);

            var clients = dataContext.Clients.Where(client => client.DataN <= newDate)
                        .OrderByDescending(client => client.DataN);
            
            // per debugar, podem mostrar newDate en la vista
            /*DateTime dt = (DateTime) newDate;
            string nd = dt.ToString("dd-MM-yyyy");
            ViewBag.newDate = nd;*/

            return View(clients.ToList());
        }

        // GET: Client/Create
        public IActionResult Create()
        {
            VeureUser();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        // POST: Client/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Dni,Nom,Cognoms,DataN")] Client client)
        {
            VeureUser();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid) {
                dataContext.Add(client);
                dataContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            } else {
                ViewBag.missatge = client.validarClient().missatge;
                return View();
            }
        }

        // GET: Client/Edit/5
        public IActionResult Edit(int? id)
        {
            VeureUser();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var client = dataContext.Clients
                .FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Client/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("Id,Dni,Nom,Cognoms,DataN")] Client client)
        {
            VeureUser();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) {
                return RedirectToAction("Index", "Home");
            }

            //cerquem el client a partir de la id que ens arriba del formulari, client.Id
            var client0 = dataContext.Clients
                .FirstOrDefault(c => c.Id == client.Id);

            if (client0 == null)
            {
                return NotFound();
            }

            try
                {
            // actualitzem les dades del client i persistim en la BDD
                    if(ModelState.IsValid) {
                        client0.Dni = client.Dni;
                        client0.Nom = client.Nom;
                        client0.Cognoms = client.Cognoms;
                        client0.DataN = client.DataN;
                        dataContext.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    } else {
                        ViewBag.missatge = client.validarClient().missatge;
                    }
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }

            return View(client0);
        }

        // GET: Client/Delete/5
        public IActionResult Delete(int? id)
        {
            VeureUser();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var client = dataContext.Clients
                .FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) {
                return RedirectToAction("Index", "Home");
            }
                        
            var client = dataContext.Clients.Find(id);
            dataContext.Clients.Remove(client);
            dataContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult ValidarDataN(DateTime DataN)
        {
            // retorna true només si la data és anterior a la d'avui
            if(DataN <= DateTime.Now)
            {
                return Json(true);
            }
                return Json(false);    
            }
        }
}
