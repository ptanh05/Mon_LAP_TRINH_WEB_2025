using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.Data;

namespace StudentManagement.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SchoolContext _context;

    public HomeController(ILogger<HomeController> logger, SchoolContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _context.Students.CountAsync();
        var courses = await _context.Courses.CountAsync();
        var enrollments = await _context.Enrollments.CountAsync();

        ViewBag.StudentsCount = students;
        ViewBag.CoursesCount = courses;
        ViewBag.EnrollmentsCount = enrollments;

        return View();
    }

    [HttpGet("/stats/overview")]
    public async Task<IActionResult> StatsOverview()
    {
        var students = await _context.Students.CountAsync();
        var courses = await _context.Courses.CountAsync();
        var enrollments = await _context.Enrollments.CountAsync();
        var avgGpa = await _context.Enrollments
            .Where(e => (e.Grade ?? e.MidtermGrade ?? e.FinalGrade ?? e.AssignmentGrade) != null)
            .Select(e => e.GetGpaPoints())
            .DefaultIfEmpty(0)
            .AverageAsync();
        return Ok(new { students, courses, enrollments, avgGpa = Math.Round((double)avgGpa, 2) });
    }

    [HttpGet("/stats/top-courses")]
    public async Task<IActionResult> StatsTopCourses()
    {
        var data = await _context.Courses
            .Select(c => new { c.Title, Count = c.Enrollments.Count })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToListAsync();
        return Ok(data);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
