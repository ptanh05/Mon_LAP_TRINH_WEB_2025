using Microsoft.AspNetCore.Mvc;
using DemoLab.Models;

namespace DemoLab.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var sanPhamMoi = new List<Product>
            {
                new Product { Id=1, TenSP="Nồi cơm điện cao tần Nagakawa NAG0102", HinhAnh="/images/nagakawa.jpg" },
                new Product { Id=2, TenSP="Nồi cơm điện cao tần Nagakawa NAG0102", HinhAnh="/images/nagakawa.jpg" }
            };
            return View(sanPhamMoi);
        }
    }
}
