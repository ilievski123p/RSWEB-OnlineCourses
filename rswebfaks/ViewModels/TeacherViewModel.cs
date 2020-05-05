using Microsoft.AspNetCore.Http;
using rswebfaks.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace rswebfaks.ViewModels
{
    public class TeacherViewModel
    {
        [StringLength(50)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name ="Degree")]
        public string Degree { get; set; }
        [Display(Name = "Academic Rank")]
        public string AcademicRank { get; set; }
        [Display(Name = "Office Number")]
        public string OfficeNumber { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        [Display(Name ="Profile Picture")]
        public IFormFile ProfileImage { get; set; }

        public string FullName
        {

            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }
        [NotMapped]
        public ICollection<Course> Courses1 { get; set; }
        public ICollection<Course> Courses2 { get; set; }
    }
}
