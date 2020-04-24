using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rswebfaks.Data;
using rswebfaks.Models;

namespace rswebfaks.Controllers
{
    public class CourseController : Controller
    {
        private readonly OnlineCoursesContext _context;

        public CourseController(OnlineCoursesContext context)
        {
            _context = context;
        }

        // GET: Course
        public async Task<IActionResult> Index(string teacherString, string searchString, string sortOrder)
        {
            var courses = from c in _context.Course
                          select c;
            courses = courses.Include(c => c.Teacher2).Include(c => c.Teacher1).Include(c=>c.Enrollments).ThenInclude(c=>c.Student);
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["CreditsSortParm"] = sortOrder == "Credits" ? "cred_desc" : "Credits";
            ViewData["SemesterSortParm"] = sortOrder == "Semester" ? "sem_desc" : "Semester";
            ViewData["ProgrammeSortParm"] = sortOrder == "Programme" ? "prom_desc" : "Programme";
            ViewData["LevelSortParm"] = sortOrder == "EducationLevel" ? "lvl_desc" : "EducationLevel";
            ViewData["ftidSortParm"] = sortOrder == "FirstTeacherId" ? "ftid_desc" : "FirstTeacherId";
            ViewData["stidSortParm"] = sortOrder == "SecondTeacherId" ? "stid_desc" : "SecondTeacherId";
            switch (sortOrder)
            {
                case "Credits":
                    courses = courses.OrderBy(s => s.Credits);
                    break;
                case "Semester":
                    courses = courses.OrderBy(s => s.Semester);
                    break;
                case "Programme":
                    courses = courses.OrderBy(s => s.Programme);
                    break;
                case "EducationLevel":
                    courses = courses.OrderBy(s => s.EducationLevel);
                    break;
                case "FirstTeacherId":
                    courses = courses.OrderBy(s => s.FirstTeacherId);
                    break;
                case "SecondTeacherId":
                    courses = courses.OrderBy(s => s.SecondTeacherId);
                    break;
                case "title_desc":
                    courses = courses.OrderByDescending(s => s.Title);
                    break;
                case "cred_decs":
                    courses = courses.OrderByDescending(s => s.Credits);
                    break;
                case "sem_desc":
                    courses = courses.OrderByDescending(s => s.Semester);
                    break;
                case "prom_desc":
                    courses = courses.OrderByDescending(s => s.Programme);
                    break;
                case "lvl_desc":
                    courses = courses.OrderByDescending(s => s.EducationLevel);
                    break;
                case "ftid_desc":
                    courses = courses.OrderByDescending(s => s.FirstTeacherId);
                    break;
                case "stid_desc":
                    courses = courses.OrderByDescending(s => s.SecondTeacherId);
                    break;
                default:
                    courses = courses.OrderBy(c => c.Title);
                    break;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                int x = 0;
                Int32.TryParse(searchString, out x);
                courses = courses.Where(s => s.Title.Contains(searchString) || s.Semester == x || s.Programme.Contains(searchString));
            }
            
            if (!String.IsNullOrEmpty(teacherString))
            {
                // var  teachers = _context.Teacher.Where(m=>m.FirstName.Contains(teacherString));
                
                int y = 0;
               Int32.TryParse(teacherString, out y);
               courses = courses.Where(s => s.FirstTeacherId == y || s.SecondTeacherId == y || s.Teacher1.FirstName.Contains(teacherString) || s.Teacher2.FirstName.Contains(teacherString));
               
            }
            return View(await courses.ToListAsync());
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.Include(t => t.Teacher1).Include(t => t.Teacher2).Include(c =>c.Enrollments).ThenInclude(c=>c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName");
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName");
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
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
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.Include(c=>c.Teacher1).Include(c=>c.Teacher2)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (course == null)
            {
                return NotFound();
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "id", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}
