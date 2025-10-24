using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly SchoolContext _context;

        public InstructorsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Instructors
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
            ViewData["DepartmentSortParm"] = sortOrder == "Department" ? "department_desc" : "Department";
            ViewData["CurrentFilter"] = searchString;

            var instructors = from i in _context.Instructors
                              select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                instructors = instructors.Where(i => i.FirstName.Contains(searchString)
                                       || i.LastName.Contains(searchString)
                                       || i.Email.Contains(searchString)
                                       || i.Department.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    instructors = instructors.OrderByDescending(i => i.LastName);
                    break;
                case "Email":
                    instructors = instructors.OrderBy(i => i.Email);
                    break;
                case "email_desc":
                    instructors = instructors.OrderByDescending(i => i.Email);
                    break;
                case "Department":
                    instructors = instructors.OrderBy(i => i.Department);
                    break;
                case "department_desc":
                    instructors = instructors.OrderByDescending(i => i.Department);
                    break;
                default:
                    instructors = instructors.OrderBy(i => i.LastName);
                    break;
            }

            return View(await instructors.ToListAsync());
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.ClassSections)
                    .ThenInclude(cs => cs.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhoneNumber,Department,Specialization,HireDate,IsActive")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate email
                if (await _context.Instructors.AnyAsync(i => i.Email == instructor.Email))
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại trong hệ thống.");
                    return View(instructor);
                }

                _context.Add(instructor);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm giảng viên thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber,Department,Specialization,HireDate,IsActive")] Instructor instructor)
        {
            if (id != instructor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check for duplicate email (excluding current instructor)
                    if (await _context.Instructors.AnyAsync(i => i.Email == instructor.Email && i.Id != instructor.Id))
                    {
                        ModelState.AddModelError("Email", "Email đã tồn tại trong hệ thống.");
                        return View(instructor);
                    }

                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật giảng viên thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id))
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
            return View(instructor);
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.ClassSections)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.ClassSections)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor != null)
            {
                // Check if instructor has active class sections
                if (instructor.ClassSections.Any(cs => cs.IsActive))
                {
                    TempData["ErrorMessage"] = "Không thể xóa giảng viên đang có lớp học phần hoạt động.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa giảng viên thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.Id == id);
        }
    }
}
