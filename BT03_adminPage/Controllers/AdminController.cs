using Microsoft.AspNetCore.Mvc;
using BT03_adminPage.Services;

namespace BT03_adminPage.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public AdminController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Dashboard";
            var (users, products, orders) = _dashboardService.GetCounts();
            ViewData["countUsers"] = users;
            ViewData["countProducts"] = products;
            ViewData["countOrders"] = orders;
            ViewData["LoggedInUser"] = "Anh 231220711"; // Tên người dùng đăng nhập
            return View();
        }

        public IActionResult User()
        {
            ViewData["Title"] = "Users";
            return View();
        }

        public IActionResult Product()
        {
            ViewData["Title"] = "Products";
            return View();
        }

        public IActionResult Order()
        {
            ViewData["Title"] = "Orders";
            return View();
        }
    }
}
