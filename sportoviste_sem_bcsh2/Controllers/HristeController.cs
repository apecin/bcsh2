using Microsoft.AspNetCore.Mvc;
using sportoviste_sem_bcsh2.Models;
using sportoviste_sem_bcsh2.Services;

namespace sportoviste_sem_bcsh2.Controllers
{
    // controler zodpovědný pro CRUD operace pro hřiště
    public class HristeController : Controller
    {
        // Odkaz na databázovou službu pro práci s LiteDB
        private readonly LiteDbService _dbService;

        // Konstruktor přijímá LiteDbService prostřednictvím Dependency Injection
        public HristeController(LiteDbService dbService)
        {
            _dbService = dbService;
        }

        // Zobrazení všech hřišť
        public IActionResult Index() => View(_dbService.GetAllHriste());

        // Zobrazení detailů konkrétního hřiště
        public IActionResult Details(int id)
        {
            var hriste = _dbService.GetHristeById(id);
            return hriste == null ? NotFound() : View(hriste);
        }

        // GET: Metoda zobrazující formulář pro vytvoření nového hřiště
        public IActionResult Create()
        {
            // Získání seznamu sportovišť pro ComboBox ve formuláři
            ViewBag.SportovisteList = _dbService.GetAllSportoviste();
            return View();
        }

        // POST: Zpracování formuláře pro vytvoření nového hřiště
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hriste hriste)
        {
            if (ModelState.IsValid) // Kontrola platnosti modelu
            {
                _dbService.InsertHriste(hriste); // Vložení nového hřiště do databáze
                return RedirectToAction(nameof(Index)); // Přesměrování na seznam hřišť
            }

            // Pokud je ModelState neplatný, znovu načte seznam sportovišť
            ViewBag.SportovisteList = _dbService.GetAllSportoviste();
            return View(hriste); // Zobrazení formuláře s chybami
        }

        // GET: Zobrazení formuláře pro úpravu existujícího hřiště podle ID
        public IActionResult Edit(int id)
        {
            var hriste = _dbService.GetHristeById(id); // Načtení hřiště podle ID
            if (hriste == null) return NotFound(); // Pokud neexistuje, vrací 404

            ViewBag.SportovisteList = _dbService.GetAllSportoviste(); // Načte seznam sportovišť
            return View(hriste); // Zobrazení formuláře s daty hřiště
        }

        // POST: Zpracování formuláře pro úpravu hřiště
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Hriste hriste)
        {
            if (ModelState.IsValid) // Kontrola platnosti modelu
            {
                _dbService.UpdateHriste(hriste); // Aktualizace hřiště v databázi
                return RedirectToAction(nameof(Index)); // Přesměrování na seznam hřišť
            }

            // Pokud je ModelState neplatný, znovu načte seznam sportovišť
            ViewBag.SportovisteList = _dbService.GetAllSportoviste();
            return View(hriste); // Zobrazení formuláře s chybami
        }

        // GET: Zobrazení formuláře pro potvrzení smazání hřiště
        public IActionResult Delete(int id)
        {
            var hriste = _dbService.GetHristeById(id); // Načtení hřiště podle ID
            return hriste == null ? NotFound() : View(hriste); // Pokud neexistuje, vrací 404
        }

        // POST: Smazání hřiště po potvrzení formuláře
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _dbService.DeleteHriste(id); // Smazání hřiště podle ID
            return RedirectToAction(nameof(Index)); // Přesměrování na seznam hřišť
        }
    }
}
