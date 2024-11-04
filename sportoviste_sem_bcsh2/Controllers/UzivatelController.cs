using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using sportoviste_sem_bcsh2.Models;
using System.Collections.Generic;

namespace sportoviste_sem_bcsh2.Controllers
{
    public class UzivatelController : Controller
    {
        // Mock uživatelé pro testování
        private static List<Uzivatel> mockUzivatele = new List<Uzivatel>
        {
            new Uzivatel { Id = 1, Jmeno = "Jan Novák", Email = "jan@novak.cz", Heslo = "heslo123" },
            new Uzivatel { Id = 2, Jmeno = "Petr Svoboda", Email = "petr@svoboda.cz", Heslo = "tajneheslo" }
        };

        // GET: Uzivatel/Prihlaseni
        public IActionResult Prihlaseni()
        {
            return View();
        }

        // POST: Uzivatel/Prihlaseni
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

                // Přesměrování na hlavní stránku (nebo jinou stránku dle potřeby)
                return RedirectToAction("Index", "Home");
            }

            // Chybová zpráva pro špatné přihlašovací údaje
            ViewBag.Zprava = "Neplatné přihlašovací údaje.";
            return View();
        }

        // GET: Uzivatel/Odhlaseni
        public IActionResult Odhlaseni()
        {
            // Vymazání uživatelských údajů ze session
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
