using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LiteDB;


namespace sportoviste_sem_bcsh2.Controllers
{
    public class RezervaceController : Controller
    {
        // GET: RezervaceController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RezervaceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RezervaceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RezervaceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RezervaceController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RezervaceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RezervaceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RezervaceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
