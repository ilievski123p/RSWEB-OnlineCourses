using Microsoft.EntityFrameworkCore;
using rswebfaks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rswebfaks.Data
{
    public class OnlineCoursesContext : DbContext
    {
        public OnlineCoursesContext(DbContextOptions<OnlineCoursesContext> options) : base(options)
        {
        }
        public DbSet<Student> Student { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<rswebfaks.Models.Teacher> Teacher { get; set; }
        public DbSet<rswebfaks.Models.TeacherCourse> TeacherCourse { get; set; }
       
    }
}
