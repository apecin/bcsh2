using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LiteDB;
using sportoviste_sem_bcsh2.Models;
using System.Collections.Generic;

namespace sportoviste_sem_bcsh2.Controllers
{
    public class RezervaceController : Controller
    {
        private readonly string _dbPath = "rezervace.db";

        // GET: RezervaceController
        public IActionResult Index()
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                var rezervaceCollection = db.GetCollection<Rezervace>("rezervace");
                var rezervaceList = rezervaceCollection.FindAll();
                return View(rezervaceList);
            }
        }

        // GET: RezervaceController/Details/5
        public IActionResult Details(int id)
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                var rezervaceCollection = db.GetCollection<Rezervace>("rezervace");
                var rezervace = rezervaceCollection.FindById(id);
                if (rezervace == null) return NotFound();
                return View(rezervace);
            }
        }

        // GET: RezervaceController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RezervaceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Rezervace rezervace)
        {
            try
            {
                using (var db = new LiteDatabase(_dbPath))
                {
                    var rezervaceCollection = db.GetCollection<Rezervace>("rezervace");
                    rezervaceCollection.Insert(rezervace);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RezervaceController/Edit/5
        public IActionResult Edit(int id)
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                var rezervaceCollection = db.GetCollection<Rezervace>("rezervace");
                var rezervace = rezervaceCollection.FindById(id);
                if (rezervace == null) return NotFound();
                return View(rezervace);
            }
        }

        // POST: RezervaceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Rezervace updatedRezervace)
        {
            try
            {
                using (var db = new LiteDatabase(_dbPath))
                {
                    var rezervaceCollection = db.GetCollection<Rezervace>("rezervace");
                    rezervaceCollection.Update(updatedRezervace);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RezervaceController/Delete/5
        public IActionResult Delete(int id)
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                var rezervaceCollection = db.GetCollection<Rezervace>("rezervace");
                var rezervace = rezervaceCollection.FindById(id);
                if (rezervace == null) return NotFound();
                return View(rezervace);
            }
        }

        // POST: RezervaceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                using (var db = new LiteDatabase(_dbPath))
                {
                    var rezervaceCollection = db.GetCollection<Rezervace>("rezervace");
                    rezervaceCollection.Delete(id);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
