using Microsoft.AspNetCore.Mvc;
using LiteDB;
using sportoviste_sem_bcsh2.Models;
using System.Collections.Generic;

namespace sportoviste_sem_bcsh2.Controllers
{
    public class RezervaceController : Controller
    {
        private readonly LiteDatabase _db;

        public RezervaceController(LiteDatabase db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var rezervaceCollection = _db.GetCollection<Rezervace>("rezervace");
            var rezervaceList = rezervaceCollection.FindAll();
            return View(rezervaceList);
        }

        public IActionResult Details(int id)
        {
            var rezervaceCollection = _db.GetCollection<Rezervace>("rezervace");
            var rezervace = rezervaceCollection.FindById(id);
            if (rezervace == null) return NotFound();
            return View(rezervace);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Rezervace rezervace)
        {
            try
            {
                var rezervaceCollection = _db.GetCollection<Rezervace>("rezervace");
                rezervaceCollection.Insert(rezervace);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Edit(int id)
        {
            var rezervaceCollection = _db.GetCollection<Rezervace>("rezervace");
            var rezervace = rezervaceCollection.FindById(id);
            if (rezervace == null) return NotFound();
            return View(rezervace);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Rezervace updatedRezervace)
        {
            try
            {
                var rezervaceCollection = _db.GetCollection<Rezervace>("rezervace");
                rezervaceCollection.Update(updatedRezervace);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Delete(int id)
        {
            var rezervaceCollection = _db.GetCollection<Rezervace>("rezervace");
            var rezervace = rezervaceCollection.FindById(id);
            if (rezervace == null) return NotFound();
            return View(rezervace);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var rezervaceCollection = _db.GetCollection<Rezervace>("rezervace");
                rezervaceCollection.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
