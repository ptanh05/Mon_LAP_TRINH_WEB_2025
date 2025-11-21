using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab06.Models;

namespace lab06.ViewComponents
{
    // ViewComponent để hiển thị danh sách các chuyên ngành để lọc
    public class MajorViewComponent : ViewComponent
    {
        private readonly SchoolContext db;

        // Constructor để inject SchoolContext
        public MajorViewComponent(SchoolContext context)
        {
            db = context;
        }

        // Invoke method - được gọi khi render ViewComponent
        public IViewComponentResult Invoke()
        {
            // Lấy danh sách tất cả các chuyên ngành từ database
            var majors = db.Majors.ToList();
            
            // Trả về view với danh sách majors
            return View(majors);
        }
    }
}

