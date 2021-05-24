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
    public class CompteController : Controller
    {
        private readonly bankContext dataContext;

        public CompteController(bankContext context)
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

        // GET: Compte
        public IActionResult Index()
        {
            VeureUser();

            var comptes = dataContext.Comptes.Include(compte => compte.Client);
            return View(comptes.ToList());
        }

        // GET: Compte/Create
        public IActionResult Create()
        {
            VeureUser();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) {
                return RedirectToAction("Index", "Home");
            }

            PopulateClientsDropDownList();
            return View();
        }

        // POST: Compte/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Codi, Saldo, ClientId")] Compte compte)
        {
            VeureUser();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid) {
                dataContext.Add(compte);
                dataContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            } else {
                ViewBag.missatge = compte.validarCompte().missatge;
                PopulateClientsDropDownList();
                return View();
            }
        }

        // GET: Compte/Edit/5
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

            var compte = dataContext.Comptes
                .FirstOrDefault(c => c.Id == id);
            if (compte == null)
            {
                return NotFound();
            }
            PopulateClientsDropDownList(compte.ClientId);
            return View(compte);
        }

        // POST: Compte/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("Id, Codi, Saldo, ClientId")] Compte compte)
        {

            VeureUser();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) {
                return RedirectToAction("Index", "Home");
            }

            //cerquem el compte a partir de la id que ens arriba del formulari, compte.Id
            var compte0 = dataContext.Comptes
                .FirstOrDefault(c => c.Id == compte.Id);

            if (compte0 == null)
            {
                return NotFound();
            }

            try
                {
            // actualitzem les dades del compte i persistim en la BDD
                    if(ModelState.IsValid) {
                        compte0.Codi = compte.Codi;
                        compte0.Saldo = compte.Saldo;
                        compte0.ClientId = compte.ClientId;
                        dataContext.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    } else {
                        ViewBag.missatge = compte.validarCompte().missatge;
                    }
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }

            PopulateClientsDropDownList(compte0.ClientId);
            return View(compte0);
        }

        private void PopulateClientsDropDownList(object selectedClient = null)
        {
            var clientsQuery = from c in dataContext.Clients
                                   orderby c.Id
                                   select c;
            ViewBag.ClientId = new SelectList(clientsQuery.AsNoTracking(), "Id", "FullName", selectedClient);
        }

        // GET: Compte/Delete/5
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

            var compte = dataContext.Comptes.Include(compte => compte.Client)
                .FirstOrDefault(c => c.Id == id);
            if (compte == null)
            {
                return NotFound();
            }

            return View(compte);
        }

        // POST: Compte/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) {
                return RedirectToAction("Index", "Home");
            }

            var compte = dataContext.Comptes.Find(id);
            dataContext.Comptes.Remove(compte);
            dataContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
