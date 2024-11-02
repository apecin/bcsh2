using Microsoft.AspNetCore.Mvc;
using sportoviste_sem_bcsh2.Models;

namespace sportoviste_sem_bcsh2.Controllers
{
    public class SportovisteController : Controller
    {
        public IActionResult Index()
        {
            var sportovisteList = new List<Sportoviste>
        {
            new Sportoviste { Id = 1, Nazev = "Sportoviště 1" },
            new Sportoviste { Id = 2, Nazev = "Sportoviště 2" },
            new Sportoviste { Id = 3, Nazev = "Sportoviště 3" }
        };

            return View(sportovisteList);
        }
    }

}
