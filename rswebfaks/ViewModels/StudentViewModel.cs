using Microsoft.AspNetCore.Http;
using rswebfaks.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rswebfaks.ViewModels
{
    public class StudentViewModel
    {
        [Required]
        [Display(Name = "StudentID")]
        [StringLength(20)]
        public string StudentId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Acquired Credits")]
        public int AcquiredCredits { get; set; }

        [Display(Name = "Current Semester")]
        public int CurrentSemestar { get; set; }

        [StringLength(25)]
        [Display(Name = "Education Level")]
        public string EducationLevel { get; set; }

        [Display(Name ="Profile Picture")]
        public IFormFile ProfileImage { get; set; }

        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
