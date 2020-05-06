using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rswebfaks.Data;
using rswebfaks.Models;
using rswebfaks.ViewModels;

namespace rswebfaks.Controllers
{
    public class TeacherController : Controller
    {
        private readonly OnlineCoursesContext _context;
        private readonly OnlineCoursesContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public TeacherController(OnlineCoursesContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            dbContext = context;
            webHostEnvironment = hostEnvironment;
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

            var teacher = await _context.Teacher.Include(t=>t.Courses1).Include(t=>t.Courses2)
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Create(TeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);
                Teacher teacher = new Teacher
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    AcademicRank=model.AcademicRank,
                    OfficeNumber=model.OfficeNumber,
                    HireDate=model.HireDate,
                    Degree = model.Degree,
                    ProfilePicture = uniqueFileName,
                };
                dbContext.Add(teacher);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public string UploadedFile(TeacherViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfileImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePicture,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (id != teacher.Id)
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
                    if (!TeacherExists(teacher.Id))
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
                .FirstOrDefaultAsync(m => m.Id == id);
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
            return _context.Teacher.Any(e => e.Id == id);
        }

        public async Task<IActionResult> TeacherView()
        {
            var teachers = from m in _context.Teacher
                           select m;
            return View(await teachers.ToListAsync());
        }

       
    }
}
