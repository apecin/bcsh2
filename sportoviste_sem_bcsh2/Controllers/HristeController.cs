using Microsoft.AspNetCore.Mvc;
using sportoviste_sem_bcsh2.Models;
using LiteDB;
using System.Linq; // Pro použití metody Any()

namespace sportoviste_sem_bcsh2.Controllers
{
    public class HristeController : Controller
    {
        private readonly LiteDatabase _db;

        public HristeController()
        {
            // Připojení k databázi LiteDB
            _db = new LiteDatabase(@"Filename=Hriste.db; Connection=shared");

            // Získání kolekce `hriste`
            var hristeCollection = _db.GetCollection<Hriste>("hriste");

            // Kontrola, zda kolekce obsahuje data
            if (!hristeCollection.FindAll().Any())
            {
                // Pokud je kolekce prázdná, vložíme výchozí data
                hristeCollection.Insert(new Hriste { Id = 1, Nazev = "Fotbalové Hřiště" });
                hristeCollection.Insert(new Hriste { Id = 2, Nazev = "Tenisový Kurt" });
                hristeCollection.Insert(new Hriste { Id = 3, Nazev = "Basketbalová Hala" });
            }
        }

        public IActionResult Index()
        {
            var hriste = _db.GetCollection<Hriste>("hriste").FindAll().ToList();
            Console.WriteLine($"Počet záznamů v kolekci hriste: {hriste.Count}");

            if (hriste == null)
            {
                hriste = new List<Hriste>(); // Vytvoříme prázdný seznam, pokud je null
            }
            return View(hriste);
        }


        public IActionResult Details(int id)
        {
            var hriste = _db.GetCollection<Hriste>("hriste").FindById(id);
            return hriste == null ? NotFound() : View(hriste);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hriste hriste)
        {
            if (ModelState.IsValid)
            {
                _db.GetCollection<Hriste>("hriste").Insert(hriste);
                return RedirectToAction(nameof(Index));
            }
            return View(hriste);
        }

        public IActionResult Edit(int id)
        {
            var hriste = _db.GetCollection<Hriste>("hriste").FindById(id);
            return hriste == null ? NotFound() : View(hriste);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Hriste hriste)
        {
            if (ModelState.IsValid)
            {
                _db.GetCollection<Hriste>("hriste").Update(hriste);
                return RedirectToAction(nameof(Index));
            }
            return View(hriste);
        }

        public IActionResult Delete(int id)
        {
            var hriste = _db.GetCollection<Hriste>("hriste").FindById(id);
            return hriste == null ? NotFound() : View(hriste);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _db.GetCollection<Hriste>("hriste").Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
