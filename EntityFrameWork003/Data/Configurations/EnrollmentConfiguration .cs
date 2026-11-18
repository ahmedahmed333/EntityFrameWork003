using EntityFrameWork003.Models;
using EntityFrameWork003.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameWork003.Data.Configurations
{
    internal class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {

        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => new { e.StudentId, e.CourseId });

            builder.Property(e => e.EnrollmentDate).IsRequired();
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.FinalGrade).HasPrecision(5, 2);

            builder.ToTable(t => t.HasCheckConstraint(
    "CK_Enrollments_Progress", "[Progress] BETWEEN 0 AND 100"));


            builder.HasOne(e => e.Student)
             .WithMany(s => s.Enrollments)
             .HasForeignKey(e => e.StudentId)
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Course)
                    .WithMany(c => c.Enrollments)
                        .HasForeignKey(e => e.CourseId)
                        .OnDelete(DeleteBehavior.Cascade);


    //        builder.HasData(
    //new Enrollment { StudentId = 1, CourseId = 1, EnrollmentDate = new DateTime(2024, 6, 1), Progress = 40, Status = EnrollmentStatus.Active },
    //new Enrollment { StudentId = 2, CourseId = 1, EnrollmentDate = new DateTime(2024, 6, 2), Progress = 100, Status = EnrollmentStatus.Completed, FinalGrade = 92.5m },
    //new Enrollment { StudentId = 1, CourseId = 2, EnrollmentDate = new DateTime(2024, 6, 3), Progress = 10, Status = EnrollmentStatus.Active });
        }
    }
}
