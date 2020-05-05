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
    public class StudentController : Controller
    {
        private readonly OnlineCoursesContext _context;
        private readonly OnlineCoursesContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public StudentController(OnlineCoursesContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            dbContext = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Student
        public async Task<IActionResult> Index(string courseString, string searchString, string sortOrder)
        {
            var students = from m in _context.Student
                           select m;
            students = students.Include(s=>s.Enrollments).ThenInclude(s=>s.Course);
            ViewData["FNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "fname_desc" : "";
            ViewData["CreditsSortParm"] = sortOrder == "Credits" ? "cred_desc" : "Credits";
            ViewData["LNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "lname_desc" : "";
            ViewData["StudentIdSortParm"] = sortOrder=="StudentId" ? "stid_desc" : "StudentId";
            ViewData["DateSortParm"] = sortOrder=="Date" ? "date_desc" : "Date";
            ViewData["SemSortParm"] = sortOrder == "Semester" ? "sem_desc" : "Semester";
            ViewData["LevelSortParm"] = sortOrder == "EducationLevel" ? "lvl_desc" : "EducationLevel";
            switch (sortOrder)
            {
                case "fname_desc":
                    students = students.OrderByDescending(s => s.FirstName);
                    break;
                case "Credits":
                    students = students.OrderBy(s => s.AcquiredCredits);
                    break;
                case "cred_desc":
                    students = students.OrderByDescending(s => s.AcquiredCredits);
                    break;
                case "StudentId":
                    students = students.OrderBy(s => s.StudentId);
                    break;
                case "stid_desc":
                    students = students.OrderByDescending(s => s.StudentId);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                case "Semester":
                    students = students.OrderBy(s => s.CurrentSemestar);
                    break;
                case "sem_desc":
                    students = students.OrderByDescending(s => s.CurrentSemestar);
                    break;
                case "lname_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "EducationLevel":
                    students = students.OrderBy(s => s.EducationLevel);
                    break;
                case "lvl_desc":
                    students = students.OrderByDescending(s => s.EducationLevel);
                    break;
                default:
                    students = students.OrderBy(s => s.FirstName);
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.StudentId.Contains(searchString) || s.FirstName.Contains(searchString) || s.LastName.Contains(searchString));
               
            }
            if (!String.IsNullOrEmpty(courseString))
            {
                var enrollments = _context.Enrollment.AsQueryable();
                enrollments = enrollments.Where(m => m.Course.Title.Contains(courseString));
                students = students.Where(s => s.Enrollments.Any(e => e.Course.Title == courseString));
               
            }
           return View(await students.ToListAsync());
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            var enrols = _context.Enrollment;
            var student = await _context.Student.Include(s=>s.Enrollments).ThenInclude(s=>s.Course)
               // .Include(s => s.Enrollments.Where(e=>e.Studentid == id))
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            
            return View( student);
        }

  
        // GET: Student/Create
        public IActionResult Create()
        {
           
            return View();
        }
       
        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);
                Student student = new Student
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    StudentId = model.StudentId,
                    EducationLevel = model.EducationLevel,
                    AcquiredCredits = model.AcquiredCredits,
                    CurrentSemestar = model.CurrentSemestar,
                    EnrollmentDate = model.EnrollmentDate,
                    ProfilePicture = uniqueFileName,
                };
                dbContext.Add(student);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View();
        }
        public string  UploadedFile(StudentViewModel model)
        {
            string uniqueFileName=null;

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
        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,StudentId,ProfilePicture,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemestar,EducationLevel")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
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

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(long id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
