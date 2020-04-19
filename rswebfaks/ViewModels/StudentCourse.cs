using Microsoft.AspNetCore.Mvc.Rendering;
using rswebfaks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rswebfaks.ViewModels
{
    public class StudentCourse
    {
        public IList<Student> Students { get; set; }
        public SelectList Courses { get; set; }
        public string CourseStudent { get; set; }
        public string SearchString { get; set; }
        
    }
}
