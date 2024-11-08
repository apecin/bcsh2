using Microsoft.AspNetCore.Mvc;
using sportoviste_sem_bcsh2.Models;
using sportoviste_sem_bcsh2.Services;

namespace sportoviste_sem_bcsh2.Controllers
{
    public class SportovisteController : Controller
    {
        private readonly LiteDbService _dbService;

        public SportovisteController(LiteDbService dbService)
        {
            _dbService = dbService;
        }

        public IActionResult Index() => View(_dbService.GetAllSportoviste());

        public IActionResult Details(int id)
        {
            var sportoviste = _dbService.GetSportovisteById(id);
            return sportoviste == null ? NotFound() : View(sportoviste);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sportoviste sportoviste)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"Přijat model: Název = {sportoviste.Nazev}, Id = {sportoviste.Id}");
                _dbService.InsertSportoviste(sportoviste);
                return RedirectToAction(nameof(Index));
            }

            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Chyba v ModelState pro {state.Key}: {error.ErrorMessage}");
                }
            }

            Console.WriteLine("ModelState není validní.");
            return View(sportoviste);
        }



        public IActionResult Edit(int id)
        {
            var sportoviste = _dbService.GetSportovisteById(id);
            return sportoviste == null ? NotFound() : View(sportoviste);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Sportoviste sportoviste)
        {
            if (ModelState.IsValid)
            {
                _dbService.UpdateSportoviste(sportoviste);
                return RedirectToAction(nameof(Index));
            }
            return View(sportoviste);
        }

        public IActionResult Delete(int id)
        {
            var sportoviste = _dbService.GetSportovisteById(id);
            return sportoviste == null ? NotFound() : View(sportoviste);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _dbService.DeleteSportoviste(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddHriste(int sportovisteId, string nazev, string typ)
        {
            var sportoviste = _dbService.GetSportovisteById(sportovisteId);
            if (sportoviste == null)
            {
                return NotFound();
            }

            var hriste = new Hriste
            {
                Nazev = nazev,
                Typ = typ
            };

            sportoviste.Hriste.Add(hriste);
            _dbService.UpdateSportoviste(sportoviste);

            return RedirectToAction("Details", new { id = sportovisteId });
        }
    }
}
