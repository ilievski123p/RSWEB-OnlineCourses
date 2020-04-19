using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace rswebfaks.Models
{
    public class Enrollment
    {
        [Key]
        [Required]
        public long Id { get; set; }

        [Required]
        [ForeignKey("Courseid")]
        public int? Courseid { get; set; }

        [Required]
        [ForeignKey("Studentid")]
        public long? Studentid { get; set; }

        [StringLength(10)]
        public string Semester { get; set; }

        public int Year { get; set; }

        public int Grade { get; set; }

        [StringLength(255)]
        [Display(Name = "Seminal URL")]
        public string SeminalUrl { get; set; }

        [StringLength(255)]
        [Display(Name = "Project URL")]
        public string ProjectUrl { get; set; }

        [Display(Name = "Exam Points")]
        public int ExamPoints { get; set; }

        [Display(Name = "Seminal Points")]
        public int SeminalPoints { get; set; }

        [Display(Name = "Additional Points")]
        public int AdditionalPoints { get; set; }

        [Display(Name = "Finish Date")]
        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
