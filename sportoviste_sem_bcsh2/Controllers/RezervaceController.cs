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
            return rezervace == null ? NotFound() : View(rezervace);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Rezervace rezervace)
        {
            if (ModelState.IsValid)
            {
                _dbService.InsertRezervace(rezervace);
                return RedirectToAction(nameof(Index));
            }
            return View(rezervace);
        }

        public IActionResult Edit(int id)
        {
            var rezervace = _dbService.GetRezervaceById(id);
            return rezervace == null ? NotFound() : View(rezervace);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Rezervace rezervace)
        {
            if (ModelState.IsValid)
            {
                _dbService.UpdateRezervace(rezervace);
                return RedirectToAction(nameof(Index));
            }
            return View(rezervace);
        }

        public IActionResult Delete(int id)
        {
            var rezervace = _dbService.GetRezervaceById(id);
            return rezervace == null ? NotFound() : View(rezervace);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _dbService.DeleteRezervace(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
