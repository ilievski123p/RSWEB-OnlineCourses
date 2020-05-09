using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rswebfaks.Models;

namespace rswebfaks.ViewModels
{
    public class AdminEnrollmentViewModel
    {
        public Enrollment Enrollments { get; set; }
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public IEnumerable<long> SelectedStudents { get; set; }
        public IEnumerable<SelectListItem> StudentsList { get; set; }
    }

}