using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    /// <summary>
    /// Controller quản lý các thao tác CRUD cho Course
    /// </summary>
    public class CoursesController : Controller
    {
        private readonly SchoolContext _context;

        /// <summary>
        /// Constructor của CoursesController
        /// </summary>
        /// <param name="context">DbContext instance được inject</param>
        public CoursesController(SchoolContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Courses - Hiển thị danh sách tất cả khóa học
        /// </summary>
        /// <returns>View chứa danh sách khóa học</returns>
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .ToListAsync();

            return View(courses);
        }

        /// <summary>
        /// GET: Courses/Details/5 - Hiển thị chi tiết thông tin khóa học
        /// </summary>
        /// <param name="id">ID của khóa học</param>
        /// <returns>View chi tiết khóa học hoặc NotFound</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        /// <summary>
        /// GET: Courses/Create - Hiển thị form tạo khóa học mới
        /// </summary>
        /// <returns>View form tạo khóa học</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: Courses/Create - Xử lý việc tạo khóa học mới
        /// </summary>
        /// <param name="course">Dữ liệu khóa học từ form</param>
        /// <returns>Redirect về Index hoặc hiển thị lại form nếu có lỗi</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseCode,Title,Description,Credits")] Course course)
        {
            // Kiểm tra CourseCode trùng trước khi lưu
            if (!string.IsNullOrWhiteSpace(course.CourseCode))
            {
                var normalizedCode = course.CourseCode.Trim().ToUpper();
                bool codeExists = await _context.Courses
                    .AnyAsync(c => c.CourseCode == normalizedCode);
                if (codeExists)
                {
                    ModelState.AddModelError("CourseCode", "Mã khóa học đã tồn tại. Vui lòng dùng mã khác.");
                }
                else
                {
                    course.CourseCode = normalizedCode;
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = $"Khóa học {course.Title} đã được tạo thành công!";
                
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        /// <summary>
        /// GET: Courses/Edit/5 - Hiển thị form chỉnh sửa khóa học
        /// </summary>
        /// <param name="id">ID của khóa học cần chỉnh sửa</param>
        /// <returns>View form chỉnh sửa hoặc NotFound</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        /// <summary>
        /// POST: Courses/Edit/5 - Xử lý việc cập nhật thông tin khóa học
        /// </summary>
        /// <param name="id">ID của khóa học</param>
        /// <param name="course">Dữ liệu khóa học đã chỉnh sửa</param>
        /// <returns>Redirect về Index hoặc hiển thị lại form nếu có lỗi</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseCode,Title,Description,Credits")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            // Kiểm tra CourseCode trùng với khóa học khác
            if (!string.IsNullOrWhiteSpace(course.CourseCode))
            {
                var normalizedCode = course.CourseCode.Trim().ToUpper();
                bool codeExists = await _context.Courses
                    .AnyAsync(c => c.CourseCode == normalizedCode && c.Id != course.Id);
                if (codeExists)
                {
                    ModelState.AddModelError("CourseCode", "Mã khóa học đã tồn tại. Vui lòng dùng mã khác.");
                }
                else
                {
                    course.CourseCode = normalizedCode;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = $"Thông tin khóa học {course.Title} đã được cập nhật thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            return View(course);
        }

        /// <summary>
        /// GET: Courses/Delete/5 - Hiển thị form xác nhận xóa khóa học
        /// </summary>
        /// <param name="id">ID của khóa học cần xóa</param>
        /// <returns>View xác nhận xóa hoặc NotFound</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        /// <summary>
        /// POST: Courses/Delete/5 - Xử lý việc xóa khóa học
        /// </summary>
        /// <param name="id">ID của khóa học cần xóa</param>
        /// <returns>Redirect về Index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = $"Khóa học {course.Title} đã được xóa thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Kiểm tra xem khóa học có tồn tại trong database không
        /// </summary>
        /// <param name="id">ID của khóa học</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
