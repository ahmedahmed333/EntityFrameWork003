using EntityFrameWork003.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameWork003.Models
{
    internal class Enrollment
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int Progress { get; set; }
        public EnrollmentStatus Status { get; set; }
        public decimal? FinalGrade { get; set; }


        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;

    }
}
