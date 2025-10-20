using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    /// <summary>
    /// Controller quản lý các thao tác CRUD cho Enrollment
    /// </summary>
    public class EnrollmentsController : Controller
    {
        private readonly SchoolContext _context;

        /// <summary>
        /// Constructor của EnrollmentsController
        /// </summary>
        /// <param name="context">DbContext instance được inject</param>
        public EnrollmentsController(SchoolContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Enrollments - Hiển thị danh sách tất cả đăng ký khóa học
        /// </summary>
        /// <returns>View chứa danh sách đăng ký</returns>
        public async Task<IActionResult> Index()
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();

            return View(enrollments);
        }

        /// <summary>
        /// GET: Enrollments/Details/5 - Hiển thị chi tiết thông tin đăng ký
        /// </summary>
        /// <param name="id">ID của đăng ký</param>
        /// <returns>View chi tiết đăng ký hoặc NotFound</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        /// <summary>
        /// GET: Enrollments/Create - Hiển thị form tạo đăng ký mới
        /// </summary>
        /// <returns>View form tạo đăng ký</returns>
        public async Task<IActionResult> Create()
        {
            ViewData["StudentId"] = new SelectList(await _context.Students.ToListAsync(), "Id", "Name");
            ViewData["CourseId"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Title");
            return View();
        }

        /// <summary>
        /// POST: Enrollments/Create - Xử lý việc tạo đăng ký mới
        /// </summary>
        /// <param name="enrollment">Dữ liệu đăng ký từ form</param>
        /// <returns>Redirect về Index hoặc hiển thị lại form nếu có lỗi</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,CourseId,EnrollmentDate,Grade,Status")] Enrollment enrollment)
        {
            // Kiểm tra đăng ký trùng (sinh viên đã đăng ký khóa học này)
            bool duplicateExists = await _context.Enrollments
                .AnyAsync(e => e.StudentId == enrollment.StudentId && e.CourseId == enrollment.CourseId);
            
            if (duplicateExists)
            {
                ModelState.AddModelError("", "Sinh viên đã đăng ký khóa học này rồi.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = $"Đăng ký khóa học đã được tạo thành công!";
                
                return RedirectToAction(nameof(Index));
            }

            ViewData["StudentId"] = new SelectList(await _context.Students.ToListAsync(), "Id", "Name", enrollment.StudentId);
            ViewData["CourseId"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Title", enrollment.CourseId);
            return View(enrollment);
        }

        /// <summary>
        /// GET: Enrollments/Edit/5 - Hiển thị form chỉnh sửa đăng ký
        /// </summary>
        /// <param name="id">ID của đăng ký cần chỉnh sửa</param>
        /// <returns>View form chỉnh sửa hoặc NotFound</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            ViewData["StudentId"] = new SelectList(await _context.Students.ToListAsync(), "Id", "Name", enrollment.StudentId);
            ViewData["CourseId"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Title", enrollment.CourseId);
            return View(enrollment);
        }

        /// <summary>
        /// POST: Enrollments/Edit/5 - Xử lý việc cập nhật thông tin đăng ký
        /// </summary>
        /// <param name="id">ID của đăng ký</param>
        /// <param name="enrollment">Dữ liệu đăng ký đã chỉnh sửa</param>
        /// <returns>Redirect về Index hoặc hiển thị lại form nếu có lỗi</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,CourseId,EnrollmentDate,Grade,Status")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            // Kiểm tra đăng ký trùng với đăng ký khác
            bool duplicateExists = await _context.Enrollments
                .AnyAsync(e => e.StudentId == enrollment.StudentId && e.CourseId == enrollment.CourseId && e.Id != enrollment.Id);
            
            if (duplicateExists)
            {
                ModelState.AddModelError("", "Sinh viên đã đăng ký khóa học này rồi.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = $"Thông tin đăng ký đã được cập nhật thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
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

            ViewData["StudentId"] = new SelectList(await _context.Students.ToListAsync(), "Id", "Name", enrollment.StudentId);
            ViewData["CourseId"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Title", enrollment.CourseId);
            return View(enrollment);
        }

        /// <summary>
        /// GET: Enrollments/Delete/5 - Hiển thị form xác nhận xóa đăng ký
        /// </summary>
        /// <param name="id">ID của đăng ký cần xóa</param>
        /// <returns>View xác nhận xóa hoặc NotFound</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        /// <summary>
        /// POST: Enrollments/Delete/5 - Xử lý việc xóa đăng ký
        /// </summary>
        /// <param name="id">ID của đăng ký cần xóa</param>
        /// <returns>Redirect về Index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = $"Đăng ký đã được xóa thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Kiểm tra xem đăng ký có tồn tại trong database không
        /// </summary>
        /// <param name="id">ID của đăng ký</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.Id == id);
        }
    }
}
