using Microsoft.AspNetCore.Mvc;
using sportoviste_sem_bcsh2.Models;
using sportoviste_sem_bcsh2.Services;

namespace sportoviste_sem_bcsh2.Controllers
{
    public class ObsazenostController : Controller
    {
        private readonly LiteDbService _dbService;

        public ObsazenostController(LiteDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var sportovisteList = _dbService.GetAllSportoviste()
                .Select(s => new
                {
                    Id = s.Id,
                    Nazev = s.Nazev
                })
                .ToList();

            // Debugging výstup
            foreach (var sportoviste in sportovisteList)
            {
                Console.WriteLine($"Sportoviště: Id = {sportoviste.Id}, Název = {sportoviste.Nazev}");
            }

            ViewBag.SportovisteList = sportovisteList;

            return View();
        }



        [HttpPost]
        public IActionResult Obsazenost(int sportovisteId, DateTime? selectedDate)
        {
            var sportoviste = _dbService.GetSportovisteById(sportovisteId);
            if (sportoviste == null)
            {
                TempData["ErrorMessage"] = "Sportoviště neexistuje.";
                return RedirectToAction("Index");
            }

            var date = selectedDate ?? DateTime.Today;

            var obsazenost = sportoviste.Hriste.Select(h => new ObsazenostViewModel
            {
                Hriste = h.Nazev,
                OtevrenoOd = h.OtevrenoOd,
                OtevrenoDo = h.OtevrenoDo,
                Rezervace = _dbService.GetAllRezervace()
                    .Where(r => r.HristeId == h.Id && r.Cas.Date == date.Date)
                    .OrderBy(r => r.Cas)
                    .ToList()
            }).ToList();

            ViewBag.SelectedSportoviste = sportoviste.Nazev;
            ViewBag.SelectedDate = date;

            // Správné předání modelu do view
            return View("Details", obsazenost);
        }


        [HttpGet]
        public IActionResult Details(int sportovisteId, DateTime? date)
        {
            var sportoviste = _dbService.GetSportovisteById(sportovisteId);
            if (sportoviste == null)
            {
                TempData["ErrorMessage"] = "Sportoviště neexistuje.";
                return RedirectToAction("Index");
            }

            var selectedDate = date ?? DateTime.Today;

            var obsazenost = sportoviste.Hriste.Select(h => new ObsazenostViewModel
            {
                Hriste = h.Nazev,
                Rezervace = _dbService.GetAllRezervace()
                    .Where(r => r.HristeId == h.Id && r.Cas.Date == selectedDate.Date)
                    .OrderBy(r => r.Cas)
                    .ToList(),
                OtevrenoOd = h.OtevrenoOd,
                OtevrenoDo = h.OtevrenoDo
            }).ToList();

            ViewBag.SelectedSportoviste = sportoviste.Nazev;
            ViewBag.SelectedDate = selectedDate;

            return View(obsazenost);
        }

    }
}
