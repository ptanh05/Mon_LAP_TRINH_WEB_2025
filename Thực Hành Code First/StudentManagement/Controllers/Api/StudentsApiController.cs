using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Controllers.Api
{
    /// <summary>
    /// REST API Controller cho Students
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsApiController : ControllerBase
    {
        private readonly SchoolContext _context;

        public StudentsApiController(SchoolContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/students - Lấy danh sách sinh viên với phân trang
        /// </summary>
        /// <param name="page">Số trang (mặc định 1)</param>
        /// <param name="pageSize">Kích thước trang (mặc định 10)</param>
        /// <param name="search">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách sinh viên với thông tin phân trang</returns>
        [HttpGet]
        public async Task<IActionResult> GetStudents(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? search = null)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0 || pageSize > 100) pageSize = 10;

            var query = _context.Students.AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(s => s.Name.Contains(term) || s.Email.Contains(term));
            }

            var totalCount = await query.CountAsync();
            var students = await query
                .OrderBy(s => s.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Email,
                    s.BirthDate,
                    s.PhoneNumber,
                    s.Address,
                    EnrollmentCount = s.Enrollments.Count
                })
                .ToListAsync();

            return Ok(new
            {
                data = students,
                pagination = new
                {
                    page,
                    pageSize,
                    totalCount,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                }
            });
        }

        /// <summary>
        /// GET: api/students/5 - Lấy thông tin chi tiết sinh viên
        /// </summary>
        /// <param name="id">ID của sinh viên</param>
        /// <returns>Thông tin chi tiết sinh viên</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Email,
                    s.BirthDate,
                    s.PhoneNumber,
                    s.Address,
                    Enrollments = s.Enrollments.Select(e => new
                    {
                        e.Id,
                        e.EnrollmentDate,
                        e.Grade,
                        e.Status,
                        Course = new
                        {
                            e.Course.Id,
                            e.Course.CourseCode,
                            e.Course.Title,
                            e.Course.Credits
                        }
                    })
                })
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound(new { message = "Không tìm thấy sinh viên" });
            }

            return Ok(student);
        }

        /// <summary>
        /// POST: api/students - Tạo sinh viên mới
        /// </summary>
        /// <param name="student">Thông tin sinh viên</param>
        /// <returns>Thông tin sinh viên đã tạo</returns>
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check duplicate email
            if (await _context.Students.AnyAsync(s => s.Email == student.Email))
            {
                return BadRequest(new { message = "Email đã tồn tại" });
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        /// <summary>
        /// PUT: api/students/5 - Cập nhật thông tin sinh viên
        /// </summary>
        /// <param name="id">ID của sinh viên</param>
        /// <param name="student">Thông tin sinh viên cập nhật</param>
        /// <returns>Kết quả cập nhật</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            if (id != student.Id)
            {
                return BadRequest(new { message = "ID không khớp" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check duplicate email with other students
            if (await _context.Students.AnyAsync(s => s.Email == student.Email && s.Id != id))
            {
                return BadRequest(new { message = "Email đã tồn tại" });
            }

            try
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Students.AnyAsync(e => e.Id == id))
                {
                    return NotFound(new { message = "Không tìm thấy sinh viên" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Cập nhật thành công" });
        }

        /// <summary>
        /// DELETE: api/students/5 - Xóa sinh viên
        /// </summary>
        /// <param name="id">ID của sinh viên</param>
        /// <returns>Kết quả xóa</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Không tìm thấy sinh viên" });
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        /// <summary>
        /// GET: api/students/5/enrollments - Lấy danh sách đăng ký của sinh viên
        /// </summary>
        /// <param name="id">ID của sinh viên</param>
        /// <returns>Danh sách đăng ký</returns>
        [HttpGet("{id}/enrollments")]
        public async Task<IActionResult> GetStudentEnrollments(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Không tìm thấy sinh viên" });
            }

            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == id)
                .Select(e => new
                {
                    e.Id,
                    e.EnrollmentDate,
                    e.Grade,
                    e.Status,
                    Course = new
                    {
                        e.Course.Id,
                        e.Course.CourseCode,
                        e.Course.Title,
                        e.Course.Credits
                    }
                })
                .ToListAsync();

            return Ok(enrollments);
        }
    }
}
