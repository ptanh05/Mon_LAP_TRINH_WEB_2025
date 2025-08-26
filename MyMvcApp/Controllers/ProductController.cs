using Microsoft.AspNetCore.Mvc;

namespace MyMvcApp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
