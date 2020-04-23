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
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<rswebfaks.Models.TeacherCourse> TeacherCourse { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>()
                .HasOne<Student>(p => p.Student)
                .WithMany(p => p.Enrollments)
                .HasForeignKey(p => p.StudentId);
            modelBuilder.Entity<Enrollment>()
                .HasOne<Course>(d => d.Course)
                .WithMany(d => d.Enrollments)
                .HasForeignKey(d => d.CourseId);
            modelBuilder.Entity<Course>()
                .HasOne<Teacher>(p => p.Teacher1)
                .WithMany(p => p.Courses1)
                .HasForeignKey(p => p.FirstTeacherId);
            modelBuilder.Entity<Course>()
                .HasOne<Teacher>(p => p.Teacher2)
                .WithMany(p => p.Courses2)
                .HasForeignKey(p => p.SecondTeacherId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
