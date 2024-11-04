using Microsoft.AspNetCore.Mvc;
using sportoviste_sem_bcsh2.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace sportoviste_sem_bcsh2.Controllers
{
    public class AutentizaceController : Controller
    {
        //zkoušecí data uživatelů
        private static List<Uzivatel> mockUzivatele = new List<Uzivatel>
        {
            new Uzivatel { Id = 1, Jmeno = "Jan Novák", Email = "jan@novak.cz", Heslo = "heslo" },
            new Uzivatel { Id = 2, Jmeno = "Petr Svoboda", Email = "petr@svoboda.cz", Heslo = "heslo" }
        };

        // GET: Autentizace/Prihlaseni
        public IActionResult Prihlaseni()
        {
            return View();
        }

        // POST: Autentizace/Prihlaseni
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Prihlaseni(string email, string heslo)
        {
            var uzivatel = mockUzivatele.Find(u => u.Email == email && u.Heslo == heslo);
            if (uzivatel != null)
            {
                // Uložení informací o uživateli do session
                HttpContext.Session.SetString("UzivatelJmeno", uzivatel.Jmeno);
                HttpContext.Session.SetInt32("UzivatelId", uzivatel.Id);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Zprava = "Neplatné přihlašovací údaje.";
            return View();
        }

        // GET: Autentizace/Odhlaseni
        public IActionResult Odhlaseni()
        {
            // Vymazání uživatelských údajů ze session
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
