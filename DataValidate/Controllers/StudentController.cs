using Microsoft.AspNetCore.Mvc;
using DataValidate.Models;
namespace DataValidate.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                // ✅ Nếu dữ liệu hợp lệ thì xử lý (ví dụ: lưu DB)
                // Tạm thời demo: chuyển sang trang Success
                return RedirectToAction("Success");
            }

            // ❌ Nếu có lỗi validation thì quay lại view Create
            return View(student);
        }

        // Trang hiển thị khi tạo thành công
        public IActionResult Success()
        {
            return View();
        }
    }
}
