using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    /// <summary>
    /// Controller quản lý các thao tác CRUD cho Student
    /// </summary>
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;

        /// <summary>
        /// Constructor của StudentsController
        /// </summary>
        /// <param name="context">DbContext instance được inject</param>
        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Students - Hiển thị danh sách tất cả sinh viên
        /// </summary>
        /// <returns>View chứa danh sách sinh viên</returns>
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách sinh viên từ database
            var students = await _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .ToListAsync();

            return View(students);
        }

        /// <summary>
        /// GET: Students/Details/5 - Hiển thị chi tiết thông tin sinh viên
        /// </summary>
        /// <param name="id">ID của sinh viên</param>
        /// <returns>View chi tiết sinh viên hoặc NotFound</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Tìm sinh viên theo ID và include các enrollment
            var student = await _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        /// <summary>
        /// GET: Students/Create - Hiển thị form tạo sinh viên mới
        /// </summary>
        /// <returns>View form tạo sinh viên</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: Students/Create - Xử lý việc tạo sinh viên mới
        /// </summary>
        /// <param name="student">Dữ liệu sinh viên từ form</param>
        /// <returns>Redirect về Index hoặc hiển thị lại form nếu có lỗi</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BirthDate,Email,PhoneNumber,Address")] Student student)
        {
            if (ModelState.IsValid)
            {
                // Thêm sinh viên mới vào database
                _context.Add(student);
                await _context.SaveChangesAsync();
                
                // Hiển thị thông báo thành công
                TempData["SuccessMessage"] = $"Sinh viên {student.Name} đã được tạo thành công!";
                
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        /// <summary>
        /// GET: Students/Edit/5 - Hiển thị form chỉnh sửa sinh viên
        /// </summary>
        /// <param name="id">ID của sinh viên cần chỉnh sửa</param>
        /// <returns>View form chỉnh sửa hoặc NotFound</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Tìm sinh viên theo ID
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        /// <summary>
        /// POST: Students/Edit/5 - Xử lý việc cập nhật thông tin sinh viên
        /// </summary>
        /// <param name="id">ID của sinh viên</param>
        /// <param name="student">Dữ liệu sinh viên đã chỉnh sửa</param>
        /// <returns>Redirect về Index hoặc hiển thị lại form nếu có lỗi</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BirthDate,Email,PhoneNumber,Address")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin sinh viên
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    
                    // Hiển thị thông báo thành công
                    TempData["SuccessMessage"] = $"Thông tin sinh viên {student.Name} đã được cập nhật thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        /// <summary>
        /// GET: Students/Delete/5 - Hiển thị form xác nhận xóa sinh viên
        /// </summary>
        /// <param name="id">ID của sinh viên cần xóa</param>
        /// <returns>View xác nhận xóa hoặc NotFound</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Tìm sinh viên theo ID và include các enrollment
            var student = await _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        /// <summary>
        /// POST: Students/Delete/5 - Xử lý việc xóa sinh viên
        /// </summary>
        /// <param name="id">ID của sinh viên cần xóa</param>
        /// <returns>Redirect về Index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Tìm sinh viên theo ID
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                // Xóa sinh viên (cascade delete sẽ xóa các enrollment liên quan)
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                
                // Hiển thị thông báo thành công
                TempData["SuccessMessage"] = $"Sinh viên {student.Name} đã được xóa thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Kiểm tra xem sinh viên có tồn tại trong database không
        /// </summary>
        /// <param name="id">ID của sinh viên</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
