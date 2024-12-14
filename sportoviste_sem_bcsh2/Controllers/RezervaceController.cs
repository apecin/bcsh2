using Microsoft.AspNetCore.Mvc;
using sportoviste_sem_bcsh2.Models;
using sportoviste_sem_bcsh2.Services;

namespace sportoviste_sem_bcsh2.Controllers
{
    public class RezervaceController : Controller
    {
        private readonly LiteDbService _dbService;

        public RezervaceController(LiteDbService dbService)
        {
            _dbService = dbService;
        }

        public IActionResult Index() => View(_dbService.GetAllRezervace());

        public IActionResult Details(int id)
        {
            var rezervace = _dbService.GetRezervaceById(id);
            if (rezervace == null)
            {
                return NotFound();
            }
            return View(rezervace);
        }


        public IActionResult Create()
        {
            ViewBag.HristeList = _dbService.GetAllHriste()
                .Select(h => new HristeViewModel
                {
                    Id = h.Id,
                    Nazev = h.Nazev,
                    OtevrenoOd = h.OtevrenoOd.ToString(@"hh\:mm"),
                    OtevrenoDo = h.OtevrenoDo.ToString(@"hh\:mm")
                })
                .ToList();

            var userName = HttpContext.Session.GetString("UzivatelJmeno");
            var userSurname = HttpContext.Session.GetString("UzivatelPrijmeni");

            var rezervace = new Rezervace
            {
                Klient = $"{userName} {userSurname}",
                Cas = DateTime.Now.Date // Nastavení na dnešní datum
            };

            return View(rezervace);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Rezervace rezervace, string vybranyCas)
        {
            Console.WriteLine($"HristeId: {rezervace.HristeId}");
            Console.WriteLine($"Vybraný čas: {vybranyCas}");
            Console.WriteLine($"Klient: {rezervace.Klient}");

            // Ověření existence hřiště
            var hriste = _dbService.GetHristeById(rezervace.HristeId);
            if (hriste == null)
            {
                ModelState.AddModelError("", "Vybrané hřiště neexistuje.");
                ViewBag.HristeList = _dbService.GetAllHriste();
                return View(rezervace);
            }

            // Kombinace data a času
            if (TimeSpan.TryParse(vybranyCas, out var parsedTime))
            {
                rezervace.Cas = rezervace.Cas.Date + parsedTime;
            }
            else
            {
                ModelState.AddModelError("", "Čas není ve správném formátu.");
                ViewBag.HristeList = _dbService.GetAllHriste();
                return View(rezervace);
            }

            // Kontrola obsazenosti
            var existingReservations = _dbService.GetAllRezervace()
                .Where(r => r.HristeId == rezervace.HristeId && r.Cas == rezervace.Cas)
                .ToList();

            if (existingReservations.Any())
            {
                ModelState.AddModelError("", "Vybraný čas je již rezervován.");
                ViewBag.HristeList = _dbService.GetAllHriste();
                return View(rezervace);
            }

            // Přiřazení hřiště
            rezervace.Hriste = hriste;

            // Debugging
            Console.WriteLine($"Rezervace: Hřiště = {rezervace.Hriste?.Nazev}, Klient = {rezervace.Klient}, Id = {rezervace.Id}, Cas = {rezervace.Cas}");

            // Uložení rezervace
            _dbService.InsertRezervace(rezervace);

            TempData["SuccessMessage"] = "Rezervace byla úspěšně vytvořena.";
            return RedirectToAction(nameof(Index));
        }








        public IActionResult Edit(int id)
        {
            var rezervace = _dbService.GetRezervaceById(id);
            if (rezervace == null)
            {
                return NotFound();
            }

            var hriste = _dbService.GetHristeById(rezervace.HristeId);
            if (hriste == null)
            {
                ModelState.AddModelError("", "Hřiště spojené s rezervací neexistuje.");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.HristeList = _dbService.GetAllHriste()
                .Select(h => new HristeViewModel
                {
                    Id = h.Id,
                    Nazev = h.Nazev,
                    OtevrenoOd = h.OtevrenoOd.ToString(@"hh\:mm"),
                    OtevrenoDo = h.OtevrenoDo.ToString(@"hh\:mm")
                })
                .ToList();

            ViewBag.AvailableTimes = GetAvailableTimes(hriste);

            return View(rezervace);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Rezervace rezervace, string vybranyCas)
        {
            var hriste = _dbService.GetHristeById(rezervace.HristeId);
            if (hriste == null)
            {
                ModelState.AddModelError("", "Vybrané hřiště neexistuje.");
                ViewBag.HristeList = _dbService.GetAllHriste();
                ViewBag.AvailableTimes = new List<string>();
                return View(rezervace);
            }

            if (TimeSpan.TryParse(vybranyCas, out var parsedTime))
            {
                rezervace.Cas = rezervace.Cas.Date + parsedTime;
            }
            else
            {
                ModelState.AddModelError("", "Čas není ve správném formátu.");
                ViewBag.HristeList = _dbService.GetAllHriste();
                ViewBag.AvailableTimes = GetAvailableTimes(hriste);
                return View(rezervace);
            }

            var existingReservations = _dbService.GetAllRezervace()
                .Where(r => r.HristeId == rezervace.HristeId && r.Cas == rezervace.Cas && r.Id != rezervace.Id)
                .ToList();

            if (existingReservations.Any())
            {
                ModelState.AddModelError("", "Vybraný čas je již rezervován.");
                ViewBag.HristeList = _dbService.GetAllHriste();
                ViewBag.AvailableTimes = GetAvailableTimes(hriste);
                return View(rezervace);
            }

            rezervace.Hriste = hriste;
            _dbService.UpdateRezervace(rezervace);
            TempData["SuccessMessage"] = "Rezervace byla úspěšně upravena.";
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            var rezervace = _dbService.GetRezervaceById(id);
            if (rezervace == null)
            {
                return NotFound();
            }

            // Získání jména přihlášeného uživatele ze session
            var userName = HttpContext.Session.GetString("UzivatelJmeno");
            var userSurname = HttpContext.Session.GetString("UzivatelPrijmeni");
            var fullUserName = $"{userName} {userSurname}";

            // Kontrola, zda rezervace patří aktuálnímu uživateli
            if (rezervace.Klient != fullUserName)
            {
                return Forbid("CookieAuth");
            }

            return View(rezervace);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var rezervace = _dbService.GetRezervaceById(id);
            if (rezervace == null)
            {
                return NotFound();
            }

            // Získání jména přihlášeného uživatele ze session
            var userName = HttpContext.Session.GetString("UzivatelJmeno");
            var userSurname = HttpContext.Session.GetString("UzivatelPrijmeni");
            var fullUserName = $"{userName} {userSurname}";

            // Kontrola, zda rezervace patří aktuálnímu uživateli
            if (rezervace.Klient != fullUserName)
            {
                return Forbid("CookieAuth");
            }

            _dbService.DeleteRezervace(id);
            TempData["SuccessMessage"] = "Rezervace byla úspěšně smazána.";
            return RedirectToAction(nameof(Index));
        }




        private List<string> GetAvailableTimes(Hriste hriste)
        {
            var availableTimes = new List<string>();
            var startTime = hriste.OtevrenoOd.Hours; // Začátek v hodinách
            var endTime = hriste.OtevrenoDo.Hours;   // Konec v hodinách

            for (int i = startTime; i < endTime; i++)
            {
                availableTimes.Add($"{i}:00"); // Přidáme pouze celou hodinu
            }

            return availableTimes;
        }



    }
}
