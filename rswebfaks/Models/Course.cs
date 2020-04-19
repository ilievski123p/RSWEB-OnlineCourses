using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace rswebfaks.Models
{
    public class Course
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        public int Credits { get; set; }
        [Required]
        public int Semester { get; set; }
        [StringLength(100)]
        public string Programme { get; set; }
        [StringLength(25)]
        [Display(Name = "Education Level")]
        public string EducationLevel { get; set; }
        [Display(Name = "First Teacher ID")]
        [ForeignKey("FirstTeacherId")]
        public int? FirstTeacherId { get; set; }
        [Display(Name = "Second Teacher ID")]
        public int? SecondTeacherId { get; set; }

        public Teacher Teacher { get; set; }
       
    }
}
