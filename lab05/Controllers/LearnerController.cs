using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab05.Data;
using lab05.Models;

namespace lab05.Controllers
{
    public class LearnerController : Controller
    {
        private readonly SchoolContext db;

        public LearnerController(SchoolContext context)
        {
            db = context;
        }

        // GET: Learner
        public IActionResult Index(int? mid)
        {
            if (mid == null)
            {
                var learners = db.Learners.Include(m => m.Major).ToList();
                return View(learners);
            }
            else
            {
                var learners = db.Learners
                    .Where(l => l.MajorID == mid)
                    .Include(m => m.Major)
                    .ToList();
                return View(learners);
            }
        }

        // GET: Learner/LearnerByMajorID
        public IActionResult LearnerByMajorID(int mid)
        {
            var learners = db.Learners
                .Where(l => l.MajorID == mid)
                .Include(m => m.Major)
                .ToList();
            return PartialView("LearnerTable", learners);
        }

        // GET: Learner/Create
        public IActionResult Create()
        {
            ViewBag.MajorID = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(db.Majors, "MajorID", "MajorName");
            return View();
        }

        // POST: Learner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Learners.Add(learner);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorID = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        // GET: Learner/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = db.Learners.Find(id);
            if (learner == null)
            {
                return NotFound();
            }
            ViewBag.MajorID = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        // POST: Learner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Learner learner)
        {
            if (id != learner.LearnerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(learner);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerID))
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
            ViewBag.MajorID = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        // GET: Learner/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = db.Learners
                .Include(m => m.Major)
                .FirstOrDefault(m => m.LearnerID == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // POST: Learner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var learner = db.Learners.Find(id);
            if (learner != null)
            {
                db.Learners.Remove(learner);
                db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LearnerExists(int id)
        {
            return db.Learners.Any(e => e.LearnerID == id);
        }
    }
}

