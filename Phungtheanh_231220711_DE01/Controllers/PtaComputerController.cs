using Microsoft.AspNetCore.Mvc;
using Pta_231220711_DE01.Models;

namespace Pta_231220711_DE01.Controllers
{
    public class PtaComputerController : Controller
    {
        private static List<PtaComputer> _computers = new List<PtaComputer>
        {
            new PtaComputer { PtaComId = 1, PtaComName = "Laptop Dell XPS 13", PtaComPrice = 25000000, PtaComImage = "dell-xps13.jpg", PtaStatus = true },
            new PtaComputer { PtaComId = 2, PtaComName = "MacBook Pro M2", PtaComPrice = 35000000, PtaComImage = "macbook-pro.jpg", PtaStatus = true },
            new PtaComputer { PtaComId = 3, PtaComName = "PC Gaming ASUS ROG", PtaComPrice = 15000000, PtaComImage = "asus-rog.jpg", PtaStatus = false }
        };

        public IActionResult PtaIndex()
        {
            return View(_computers);
        }

        public IActionResult PtaDetails(int id)
        {
            var computer = _computers.FirstOrDefault(c => c.PtaComId == id);
            if (computer == null)
            {
                return NotFound();
            }
            return View(computer);
        }

        public IActionResult PtaCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PtaCreate(PtaComputer computer)
        {
            if (ModelState.IsValid)
            {
                computer.PtaComId = _computers.Max(c => c.PtaComId) + 1;
                _computers.Add(computer);
                return RedirectToAction(nameof(PtaIndex));
            }
            return View(computer);
        }

        public IActionResult PtaEdit(int id)
        {
            var computer = _computers.FirstOrDefault(c => c.PtaComId == id);
            if (computer == null)
            {
                return NotFound();
            }
            return View(computer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PtaEdit(int id, PtaComputer computer)
        {
            if (id != computer.PtaComId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingComputer = _computers.FirstOrDefault(c => c.PtaComId == id);
                if (existingComputer != null)
                {
                    existingComputer.PtaComName = computer.PtaComName;
                    existingComputer.PtaComPrice = computer.PtaComPrice;
                    existingComputer.PtaComImage = computer.PtaComImage;
                    existingComputer.PtaStatus = computer.PtaStatus;
                }
                return RedirectToAction(nameof(PtaIndex));
            }
            return View(computer);
        }

        public IActionResult PtaDelete(int id)
        {
            var computer = _computers.FirstOrDefault(c => c.PtaComId == id);
            if (computer == null)
            {
                return NotFound();
            }
            return View(computer);
        }

        [HttpPost, ActionName("PtaDelete")]
        [ValidateAntiForgeryToken]
        public IActionResult PtaDeleteConfirmed(int id)
        {
            var computer = _computers.FirstOrDefault(c => c.PtaComId == id);
            if (computer != null)
            {
                _computers.Remove(computer);
            }
            return RedirectToAction(nameof(PtaIndex));
        }
    }
}
