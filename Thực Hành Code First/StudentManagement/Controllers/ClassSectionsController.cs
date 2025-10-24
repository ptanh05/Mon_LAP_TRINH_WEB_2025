using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class ClassSectionsController : Controller
    {
        private readonly SchoolContext _context;

        public ClassSectionsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: ClassSections
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["SectionCodeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "sectioncode_desc" : "";
            ViewData["CourseSortParm"] = sortOrder == "Course" ? "course_desc" : "Course";
            ViewData["InstructorSortParm"] = sortOrder == "Instructor" ? "instructor_desc" : "Instructor";
            ViewData["CapacitySortParm"] = sortOrder == "Capacity" ? "capacity_desc" : "Capacity";
            ViewData["CurrentFilter"] = searchString;

            var classSections = from cs in _context.ClassSections
                               .Include(cs => cs.Course)
                               .Include(cs => cs.Instructor)
                               select cs;

            if (!String.IsNullOrEmpty(searchString))
            {
                classSections = classSections.Where(cs => cs.SectionCode.Contains(searchString)
                                       || cs.Course.Title.Contains(searchString)
                                       || cs.Instructor.FullName.Contains(searchString)
                                       || cs.Room.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "sectioncode_desc":
                    classSections = classSections.OrderByDescending(cs => cs.SectionCode);
                    break;
                case "Course":
                    classSections = classSections.OrderBy(cs => cs.Course.Title);
                    break;
                case "course_desc":
                    classSections = classSections.OrderByDescending(cs => cs.Course.Title);
                    break;
                case "Instructor":
                    classSections = classSections.OrderBy(cs => cs.Instructor.LastName);
                    break;
                case "instructor_desc":
                    classSections = classSections.OrderByDescending(cs => cs.Instructor.LastName);
                    break;
                case "Capacity":
                    classSections = classSections.OrderBy(cs => cs.MaxCapacity);
                    break;
                case "capacity_desc":
                    classSections = classSections.OrderByDescending(cs => cs.MaxCapacity);
                    break;
                default:
                    classSections = classSections.OrderBy(cs => cs.SectionCode);
                    break;
            }

            return View(await classSections.ToListAsync());
        }

        // GET: ClassSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classSection = await _context.ClassSections
                .Include(cs => cs.Course)
                .Include(cs => cs.Instructor)
                .Include(cs => cs.Enrollments)
                    .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (classSection == null)
            {
                return NotFound();
            }

            return View(classSection);
        }

        // GET: ClassSections/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Courses, "Id", "Title");
            ViewData["InstructorId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Instructors, "Id", "FullName");
            return View();
        }

        // POST: ClassSections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SectionCode,CourseId,InstructorId,MaxCapacity,StartDate,EndDate,Room,Schedule,IsActive")] ClassSection classSection)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate section code within the same course
                if (await _context.ClassSections.AnyAsync(cs => cs.CourseId == classSection.CourseId && cs.SectionCode == classSection.SectionCode))
                {
                    ModelState.AddModelError("SectionCode", "Mã lớp học phần đã tồn tại trong khóa học này.");
                    ViewData["CourseId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Courses, "Id", "Title", classSection.CourseId);
                    ViewData["InstructorId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Instructors, "Id", "FullName", classSection.InstructorId);
                    return View(classSection);
                }

                _context.Add(classSection);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm lớp học phần thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Courses, "Id", "Title", classSection.CourseId);
            ViewData["InstructorId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Instructors, "Id", "FullName", classSection.InstructorId);
            return View(classSection);
        }

        // GET: ClassSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classSection = await _context.ClassSections.FindAsync(id);
            if (classSection == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Courses, "Id", "Title", classSection.CourseId);
            ViewData["InstructorId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Instructors, "Id", "FullName", classSection.InstructorId);
            return View(classSection);
        }

        // POST: ClassSections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SectionCode,CourseId,InstructorId,MaxCapacity,StartDate,EndDate,Room,Schedule,IsActive")] ClassSection classSection)
        {
            if (id != classSection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check for duplicate section code within the same course (excluding current section)
                    if (await _context.ClassSections.AnyAsync(cs => cs.CourseId == classSection.CourseId && cs.SectionCode == classSection.SectionCode && cs.Id != classSection.Id))
                    {
                        ModelState.AddModelError("SectionCode", "Mã lớp học phần đã tồn tại trong khóa học này.");
                        ViewData["CourseId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Courses, "Id", "Title", classSection.CourseId);
                        ViewData["InstructorId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Instructors, "Id", "FullName", classSection.InstructorId);
                        return View(classSection);
                    }

                    _context.Update(classSection);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật lớp học phần thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassSectionExists(classSection.Id))
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
            ViewData["CourseId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Courses, "Id", "Title", classSection.CourseId);
            ViewData["InstructorId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Instructors, "Id", "FullName", classSection.InstructorId);
            return View(classSection);
        }

        // GET: ClassSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classSection = await _context.ClassSections
                .Include(cs => cs.Course)
                .Include(cs => cs.Instructor)
                .Include(cs => cs.Enrollments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (classSection == null)
            {
                return NotFound();
            }

            return View(classSection);
        }

        // POST: ClassSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classSection = await _context.ClassSections
                .Include(cs => cs.Enrollments)
                .FirstOrDefaultAsync(cs => cs.Id == id);

            if (classSection != null)
            {
                // Check if class section has active enrollments
                if (classSection.Enrollments.Any(e => e.Status == "Active"))
                {
                    TempData["ErrorMessage"] = "Không thể xóa lớp học phần đang có sinh viên đăng ký.";
                    return RedirectToAction(nameof(Index));
                }

                _context.ClassSections.Remove(classSection);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa lớp học phần thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClassSectionExists(int id)
        {
            return _context.ClassSections.Any(e => e.Id == id);
        }
    }
}
