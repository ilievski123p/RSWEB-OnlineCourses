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
    
    public class EnrollmentController : Controller
    {
        private readonly OnlineCoursesContext _context;
        private readonly OnlineCoursesContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EnrollmentController(OnlineCoursesContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            dbContext = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Enrollment
        public async Task<IActionResult> Index()
        {
            var onlineCoursesContext = _context.Enrollment.Include(e => e.Course).Include(e => e.Student);
            return View(await onlineCoursesContext.ToListAsync());
        }

        // GET: Enrollment/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollment/Create
        public IActionResult Create()
        {
            ViewData["Courseid"] = new SelectList(_context.Course, "Id", "Title");
            ViewData["Studentid"] = new SelectList(_context.Student, "Id", "FullName");
            return View();
        }

        // POST: Enrollment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnrollmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);
                Enrollment enrol = new Enrollment
                {
                    CourseId = model.CourseId,
                    StudentId = model.StudentId,
                    Semester = model.Semester,
                    Year = model.Year,
                    Grade = model.Grade,
                    ProjectUrl = model.ProjectUrl,
                    ExamPoints = model.ExamPoints,
                    SeminalPoints=model.SeminalPoints,
                    ProjectPoints=model.ProjectPoints,
                    AdditionalPoints=model.AdditionalPoints,
                    FinishDate=model.FinishDate,
                    SeminalUrl = uniqueFileName,
                };
                dbContext.Add(enrol);
                await dbContext.SaveChangesAsync();
                ViewData["Courseid"] = new SelectList(_context.Course, "Id", "Title");
                ViewData["Studentid"] = new SelectList(_context.Student, "Id", "FullName");
                return RedirectToAction(nameof(Index));

            }
            return View();
        }
        public string UploadedFile(EnrollmentViewModel model)
        {
            string uniqueFileName = null;

            if (model.SeminalUrl != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "files");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.SeminalUrl.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.SeminalUrl.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Enrollment/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["Courseid"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["Studentid"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CourseId,StudentId,Semester,Year,Grade,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollment/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);
            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(long id)
        {
            return _context.Enrollment.Any(e => e.Id == id);
        }


        //----------------------------------------------------------------------------------------------------------
        public async Task<IActionResult> StudentEnrollment(string searchString)
        {
            var enrols = _context.Enrollment.Include(e => e.Course).Include(e => e.Student).AsQueryable();
            enrols = enrols.Where(m => m.Student.FirstName.Contains(searchString));
            enrols = enrols.OrderByDescending(m => m.Semester);
            return View(await enrols.ToListAsync());
        }


        // GET: Enrollment/Edit/5
        public async Task<IActionResult> StudentEnrollmentEdit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["Courseid"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["Studentid"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentEnrollmentEdit(long id,  EnrollmentViewModel entry)
        {
            Enrollment enrol;
             enrol = await _context.Enrollment.Include(e => e.Course).Include(e => e.Student).FirstOrDefaultAsync(m => m.Id == id);
           
            if (ModelState.IsValid)
            {
                try
                {
                    enrol.ProjectUrl = entry.ProjectUrl;
                    enrol.SeminalUrl = UploadedFile(entry);
                    _context.Update(enrol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrol.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToPage("");
            }
        //    ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrol.CourseId);
          //  ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View();
        }



        //---------------------------------------------------------------------------------------------------
        [HttpGet]

        public async Task<IActionResult> TeacherEnrollmentView([FromQuery] string courseYear,[FromQuery] string courseString)
        {
            ViewData["CY"] = courseYear;
            var enrols = _context.Enrollment.Include(e => e.Course)
                .Include(e => e.Student).AsQueryable();
            var godina = DateTime.Now.Year;
            
            if (!String.IsNullOrEmpty(courseYear))
            {
                enrols = enrols.Where(e => e.Year.ToString().Contains(courseYear));
            }
            else
            {
                enrols = enrols.Where(e => e.Course.Title.Contains(courseString) /*&& e.Year == godina */);
                enrols = enrols.Where(e => e.Year.Equals(godina));
                enrols = enrols.OrderByDescending(e => e.Year);
            }
            
            return View(await enrols.ToListAsync());
        }



        // GET: Enrollment/Edit/5
        public async Task<IActionResult> TeacherEnrollmentEdit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["Courseid"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["Studentid"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherEnrollmentEdit(long id, [Bind("Id,CourseId,StudentId,Semester,Year,Grade,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToPage("");
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }



        //---------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminEnrollmentEdit(int id, [Bind("Id,CourseId,StudentId,Semester,Year,Grade,SeminalUrl,ProjectUrl,ExamPoints,SeminalPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(StudentEnrollment));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }




        //------------------------------------------------------------------------------------------------
        public async Task<IActionResult> StudentEnrollmentEdits(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            EnrollmentViewModel vm = new EnrollmentViewModel
            {
                Id = enrollment.Id,
                Semester = enrollment.Semester,
                Year = enrollment.Year,
                Grade = enrollment.Grade,
                ProjectUrl = enrollment.ProjectUrl,
                SeminalPoints = enrollment.SeminalPoints,
                ProjectPoints = enrollment.ProjectPoints,
                AdditionalPoints = enrollment.AdditionalPoints,
                ExamPoints = enrollment.ExamPoints,
                FinishDate = enrollment.FinishDate,
                CourseId = enrollment.CourseId,
                StudentId = enrollment.StudentId
            };
            ViewData["StudentName"] = _context.Student.Where(s => s.Id == enrollment.StudentId).Select(s => s.FullName).FirstOrDefault();
            ViewData["CourseName"] = _context.Course.Where(s => s.Id == enrollment.CourseId).Select(s => s.Title).FirstOrDefault();
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentEnrollmentEdits(long id, EnrollmentViewModel enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = UploadedFile(enrollment);

                    Enrollment enrollmentvm = new Enrollment
                    {
                        Id = enrollment.Id,
                        Semester = enrollment.Semester,
                        Year = enrollment.Year,
                        Grade = enrollment.Grade,
                        ProjectUrl = enrollment.ProjectUrl,
                        SeminalPoints = enrollment.SeminalPoints,
                        ProjectPoints = enrollment.ProjectPoints,
                        AdditionalPoints = enrollment.AdditionalPoints,
                        ExamPoints = enrollment.ExamPoints,
                        FinishDate = enrollment.FinishDate,
                        CourseId = enrollment.CourseId,
                        StudentId = enrollment.StudentId,
                        SeminalUrl = uniqueFileName
                    };
                    _context.Update(enrollmentvm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToPage("");
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FirstName", enrollment.StudentId);
            return View(enrollment);
        }

        //-------------------------------------------------------------------------------------------------------

        /*  public IActionResult AdminStudentEnrollment(long? id)
          {
              var courses = _context.Enrollment.Include(e => e.Student).Where(e => e.CourseId == id).FirstOrDefault();
              AdminEnrollmentViewModel coursestudent = new AdminEnrollmentViewModel
              {

                  Enrollments = courses,
                  StudentsList = new MultiSelectList(_context.Student.OrderBy(s => s.FirstName), "Id", "FullName"),
                  SelectedStudents = _context.Enrollment
                                      .Where(s => s.CourseId == id)
                                      .Include(m => m.Student).Select(sa => sa.StudentId)
              };
              return View(coursestudent);
          }
          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> AdminStudentEnrollment(int id, AdminEnrollmentViewModel NewEnroll)
          {

              IEnumerable<long> listStudents;
              listStudents = NewEnroll.SelectedStudents;
              IEnumerable<long> existStudents = _context.Enrollment.Where(s => listStudents.Contains(s.StudentId) && s.CourseId == id).Select(s => s.StudentId);
              IEnumerable<long> newStudents = listStudents.Where(s => !existStudents.Contains(s));
              foreach (int studentId in newStudents)
                  _context.Enrollment.Add(new Enrollment { StudentId = studentId, CourseId = id, Year = NewEnroll.Enrollments.Year, Semester = NewEnroll.Enrollments.Semester });
              await _context.SaveChangesAsync();
              return RedirectToAction(nameof(Index));
          }

      

        [HttpGet]
        public async Task<IActionResult> UnEnrollStudents(long? id)
        {

            var course = _context.Course.Where(s => s.Id == id).Include(s => s.Enrollments).First();

            if (course == null)
            {
                return NotFound();
            }

            var unEnrollStudentsVM = new UnEnrollStudents
            {
                Course = course,
                StudentList = new MultiSelectList(_context.Student.OrderBy(s => s.Id), "Id", "FullName"),
                SelectedStudents = course.Enrollments.Select(sa => sa.StudentId),
            };

            return View(unEnrollStudentsVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnEnrollStudents(long id)
        {

            var enrollments = await _context.Enrollment.FirstOrDefaultAsync(s => s.Id == id);
            await TryUpdateModelAsync<Enrollment>(
               enrollments,
               "",
               s => s.FinishDate);

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(enrollments);
        }
        */

    }
}