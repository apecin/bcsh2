using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using sportoviste_sem_bcsh2.Models;
using sportoviste_sem_bcsh2.Services;
using System.Security.Cryptography;
using System.Text;

namespace sportoviste_sem_bcsh2.Controllers
{
    public class UzivatelController : Controller
    {
        private readonly LiteDbService _dbService;

        public UzivatelController(LiteDbService dbService)
        {
            _dbService = dbService;
        }

        // GET: Uzivatel/Registrace
        public IActionResult Registrace() => View();

        // POST: Uzivatel/Registrace
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registrace(Uzivatel uzivatel)
        {
            if (ModelState.IsValid)
            {
                // Kontrola, jestli už email není v databázi
                if (_dbService.Uzivatele.FindOne(u => u.Email == uzivatel.Email) != null)
                {
                    ModelState.AddModelError("Email", "Tento email je již zaregistrovaný.");
                    return View(uzivatel);
                }

                // Hashování hesla
                uzivatel.Heslo = HashPassword(uzivatel.Heslo);
                uzivatel.Role = "User"; // Přiřazení role novým uživatelům

                _dbService.InsertUzivatel(uzivatel);
                return RedirectToAction("Prihlaseni");
            }
            return View(uzivatel);
        }

        // GET: Uzivatel/Prihlaseni
        public IActionResult Prihlaseni() => View();

        // POST: Uzivatel/Prihlaseni
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Prihlaseni(string email, string heslo)
        {
            var uzivatel = _dbService.Uzivatele.FindOne(u => u.Email == email);

            // Kontrola uživatele a hesla
            if (uzivatel == null || uzivatel.Heslo != HashPassword(heslo))
            {
                ViewBag.Zprava = "Neplatné přihlašovací údaje.";
                return View();
            }

            // Uložení informací o uživateli do session
            HttpContext.Session.SetString("UzivatelJmeno", uzivatel.Jmeno);
            HttpContext.Session.SetInt32("UzivatelId", uzivatel.Id);
            HttpContext.Session.SetString("UzivatelRole", uzivatel.Role);

            return RedirectToAction("Index", "Home");
        }

        // GET: Uzivatel/Odhlaseni
        public IActionResult Odhlaseni()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // Pomocná metoda pro hashování hesla
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
