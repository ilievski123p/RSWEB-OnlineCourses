using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using rswebfaks.Data;
using rswebfaks.Models;

namespace rswebfaks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentApiController : ControllerBase
    {
        private readonly OnlineCoursesContext _context;

        public EnrollmentApiController(OnlineCoursesContext context)
        {
            _context = context;
        }

        // GET: api/EnrollmentApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollment()
        {
            return await _context.Enrollment.ToListAsync();
        }

        // GET: api/EnrollmentApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enrollment>> GetEnrollment(long id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return enrollment;
        }

        // PUT: api/EnrollmentApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnrollment(long id, Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return BadRequest();
            }

            _context.Entry(enrollment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EnrollmentApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Enrollment>> PostEnrollment(Enrollment enrollment)
        {
            _context.Enrollment.Add(enrollment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnrollment", new { id = enrollment.Id }, enrollment);
        }

        // DELETE: api/EnrollmentApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Enrollment>> DeleteEnrollment(long id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();

            return enrollment;
        }

        private bool EnrollmentExists(long id)
        {
            return _context.Enrollment.Any(e => e.Id == id);
        }

        [Route("api/Enrollment/TeacherEnrollmentView/{courseString}/{courseYear?}")]
        public string Get(string courseString, string courseYear)
        {
            var enrols = _context.Enrollment.Include(e => e.Course)
                .Include(e => e.Student).AsQueryable();
            return "[Route]";
        }

    }
}
