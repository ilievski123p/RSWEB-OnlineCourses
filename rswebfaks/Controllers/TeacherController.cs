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
    public class TeacherController : Controller
    {
        private readonly OnlineCoursesContext _context;

        public TeacherController(OnlineCoursesContext context)
        {
            _context = context;
        }

        // GET: Teacher
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
           var teachers = from m in _context.Teacher
                          select m;
            teachers = teachers.Include(t => t.Courses1).Include(t => t.Courses2);
                     
            // Sortiranje
            ViewData["FnameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "fname_desc" : "";
            ViewData["LNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "lname_desc" : "";
            ViewData["DegreeSortParm"] = sortOrder=="Degree" ?  "deg_desc" : "Degree";
            ViewData["RankSortParm"] = sortOrder == "AcademicRank" ?  "rank_desc" : "AcademicRank";
            ViewData["ofnumSortParm"] = sortOrder == "OfficeNumber" ?  "ofn_desc" : "OfficeNumber";
            ViewData["DateSortParm"] = sortOrder == "HireDate" ?  "date_desc" : "HireDate";
            switch (sortOrder)
            {
                case "Degree":
                    teachers = teachers.OrderBy(t => t.Degree);
                    break;
                case "AcademicRank":
                    teachers = teachers.OrderBy(t => t.AcademicRank);
                    break;
                case "OfficeNumber":
                    teachers = teachers.OrderBy(t => t.OfficeNumber);
                    break;
                case "HireDate":
                    teachers = teachers.OrderBy(t => t.HireDate);
                    break;
                case "fname_desc":
                    teachers = teachers.OrderByDescending(t => t.FirstName);
                    break;
                case "lname_desc":
                    teachers = teachers.OrderByDescending(t => t.LastName);
                    break;
                case "deg_desc":
                    teachers = teachers.OrderByDescending(t => t.Degree);
                    break;
                case "rank_desc":
                    teachers = teachers.OrderByDescending(t => t.AcademicRank);
                    break;
                case "ofn_desc":
                    teachers = teachers.OrderByDescending(t => t.OfficeNumber);
                    break;
                case "date_desc":
                    teachers = teachers.OrderByDescending(t => t.HireDate);
                    break;
                default:
                    teachers = teachers.OrderBy(t => t.FirstName);
                    break;
            }

            //Search
            if (!String.IsNullOrEmpty(searchString))
            {
                teachers = teachers.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString) || s.AcademicRank.Contains(searchString) || s.Degree.Contains(searchString));
            }
          
            return View(await teachers.ToListAsync());
        }

        // GET: Teacher/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teacher/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teacher/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teacher/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teacher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (id != teacher.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.id))
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
            return View(teacher);
        }

        // GET: Teacher/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.id == id);
        }
    }
}
