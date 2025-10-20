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
        public async Task<IActionResult> Index(string? search, string? sortBy, string? sortDir, int page = 1, int pageSize = 10)
        {
            var query = _context.Students.AsQueryable();

            // Search by name or email
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(s => s.Name.Contains(term) || s.Email.Contains(term));
            }

            // Sorting
            bool desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
            query = sortBy?.ToLower() switch
            {
                "name" => desc ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
                "email" => desc ? query.OrderByDescending(s => s.Email) : query.OrderBy(s => s.Email),
                "birthdate" => desc ? query.OrderByDescending(s => s.BirthDate) : query.OrderBy(s => s.BirthDate),
                _ => desc ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id)
            };

            // Pagination
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;
            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var vm = new StudentManagement.Models.StudentListViewModel
            {
                Students = items,
                Search = search,
                SortBy = sortBy,
                SortDir = sortDir,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<FileResult> ExportCsv(string? search)
        {
            var query = _context.Students.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(s => s.Name.Contains(term) || s.Email.Contains(term));
            }
            var list = await query.OrderBy(s => s.Id).ToListAsync();

            System.Text.StringBuilder sb = new();
            sb.AppendLine("Id,Name,BirthDate,Email,PhoneNumber,Address");
            foreach (var s in list)
            {
                string line = string.Join(",",
                    s.Id,
                    EscapeCsv(s.Name),
                    s.BirthDate.ToString("yyyy-MM-dd"),
                    EscapeCsv(s.Email),
                    EscapeCsv(s.PhoneNumber ?? string.Empty),
                    EscapeCsv(s.Address ?? string.Empty));
                sb.AppendLine(line);
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", "students.csv");
        }

        private static string EscapeCsv(string value)
        {
            if (value.Contains('"') || value.Contains(',') || value.Contains('\n') || value.Contains('\r'))
            {
                value = '"' + value.Replace("\"", "\"\"") + '"';
            }
            return value;
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
            // Kiểm tra email trùng trước khi lưu
            if (!string.IsNullOrWhiteSpace(student.Email))
            {
                var normalizedEmail = student.Email.Trim();
                bool emailExists = await _context.Students
                    .AnyAsync(s => s.Email == normalizedEmail);
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại. Vui lòng dùng email khác.");
                }
            }

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

            // Kiểm tra email trùng với người khác
            if (!string.IsNullOrWhiteSpace(student.Email))
            {
                var normalizedEmail = student.Email.Trim();
                bool emailExists = await _context.Students
                    .AnyAsync(s => s.Email == normalizedEmail && s.Id != student.Id);
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại. Vui lòng dùng email khác.");
                }
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
