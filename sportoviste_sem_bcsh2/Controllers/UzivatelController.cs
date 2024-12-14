using Microsoft.AspNetCore.Mvc;
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

        // GET: Zobrazení registračního formuláře
        public IActionResult Registrace()
        {
            return View();
        }

        // POST: Zpracování registrace uživatele
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registrace(Uzivatel uzivatel, string heslo)
        {
            if (ModelState.IsValid)
            {
                // Nastavení výchozí role, pokud není zadaná
                if (string.IsNullOrEmpty(uzivatel.Role))
                {
                    uzivatel.Role = "User"; // Nastavení role "User" jako výchozí
                }

                // Kontrola, zda email už existuje
                if (_dbService.GetAllUzivatele().Any(u => u.Email.Trim().ToLower() == uzivatel.Email.Trim().ToLower()))
                {
                    ModelState.AddModelError("", "Tento email je již registrován.");
                    return View(uzivatel);
                }

                // Hashování hesla
                uzivatel.Heslo = HashPassword(heslo);

                // Zavolání metody pro vložení uživatele
                _dbService.InsertUzivatel(uzivatel);

                // Ověření úspěšnosti registrace
                if (_dbService.GetAllUzivatele().Any(u => u.Email == uzivatel.Email))
                {
                    TempData["SuccessMessage"] = "Registrace proběhla úspěšně! Můžete se nyní přihlásit.";
                    return RedirectToAction("Prihlaseni");
                }
                else
                {
                    ModelState.AddModelError("", "Došlo k chybě při ukládání uživatele. Zkuste to prosím znovu.");
                }
            }

            return View(uzivatel);
        }



        // GET: Zobrazení přihlašovacího formuláře
        public IActionResult Prihlaseni()
        {
            // Debug výpis pro kontrolu, zda je session prázdná nebo uživatel přihlášen
            Console.WriteLine("Zkontrolujme session: " + HttpContext.Session.GetString("UzivatelJmeno"));

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UzivatelJmeno")))
            {
                // Pokud je uživatel přihlášen, přesměrujeme ho
                Console.WriteLine("Uživatel je přihlášen, přesměrovávám...");
                return RedirectToAction("Index", "Home");
            }

            // Pokud není přihlášen, ukážeme přihlašovací stránku
            Console.WriteLine("Uživatel není přihlášen, zobrazíme přihlašovací formulář.");
            return View();
        }


        // POST: Zpracování přihlášení uživatele
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Prihlaseni(string email, string heslo)
        {
            // Debug výpis všech uživatelů pro ověření, zda jsou správně uloženi v databázi
            Console.WriteLine("Seznam všech uživatelů:");
            foreach (var user in _dbService.GetAllUzivatele())
            {
                Console.WriteLine($"Email: {user.Email}, Jméno: {user.Jmeno}");
            }

            // Debug výpis pro kontrolu přijatých údajů
            Console.WriteLine("Přijatý email: " + email);
            Console.WriteLine("Přijaté heslo: " + heslo);

            // Získání uživatele z databáze podle emailu Case-insensitive
            var uzivatel = _dbService.GetAllUzivatele().FirstOrDefault(u => u.Email.Trim().ToLower() == email.Trim().ToLower());

            // Debug výpis pro kontrolu, zda uživatel existuje
            if (uzivatel == null)
            {
                Console.WriteLine("Uživatel s tímto emailem neexistuje.");
            }

            // Ověření hesla
            if (uzivatel != null && VerifyPassword(heslo, uzivatel.Heslo))
            {
                Console.WriteLine("Heslo správné, přihlašujeme uživatele.");

                // Nastavení session s informacemi o uživateli
                HttpContext.Session.SetString("UzivatelJmeno", uzivatel.Jmeno);
                HttpContext.Session.SetInt32("UzivatelId", uzivatel.Id);

                // Přesměrování na hlavní stránku po úspěšném přihlášení
                return RedirectToAction("Index", "Home");
            }

            // Pokud přihlášení selže, přidání chybové zprávy a zobrazení přihlašovacího formuláře znovu
            Console.WriteLine("Přihlášení selhalo. Neplatné přihlašovací údaje.");
            ModelState.AddModelError("", "Neplatné přihlašovací údaje.");
            return View();  // Zůstane na přihlašovací stránce
        }


        // GET: Odhlášení uživatele
        public IActionResult Odhlaseni()
        {
            // Vymazání informací o uživateli ze session
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // Pomocná metoda pro hashování hesla
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Pomocná metoda pro ověření hesla
        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }

        public IActionResult Profil()
        {
            // Získání ID aktuálně přihlášeného uživatele
            var userId = HttpContext.Session.GetInt32("UzivatelId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Nejste přihlášený. Přihlaste se, prosím.";
                return RedirectToAction("Prihlaseni");
            }

            // Načtení uživatele z databáze podle ID
            var uzivatel = _dbService.GetUzivatelById(userId.Value);
            if (uzivatel == null)
            {
                TempData["ErrorMessage"] = "Nepodařilo se načíst profil uživatele.";
                return RedirectToAction("Prihlaseni");
            }

            return View(uzivatel);
        }
        public IActionResult AccessDenied(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}
