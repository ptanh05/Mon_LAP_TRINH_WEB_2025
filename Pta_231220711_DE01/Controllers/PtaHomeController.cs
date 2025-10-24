using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pta_231220711_DE01.Models;

namespace Pta_231220711_DE01.Controllers
{
    public class PtaHomeController : Controller
    {
        private readonly ILogger<PtaHomeController> _logger;

        public PtaHomeController(ILogger<PtaHomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult PtaIndex()
        {
            return View();
        }

        public IActionResult PtaContact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
