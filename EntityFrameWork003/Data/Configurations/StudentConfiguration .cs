using EntityFrameWork003.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameWork003.Data.Configurations
{
    internal class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.FullName).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Email).IsRequired().HasMaxLength(200);
            builder.HasIndex(s => s.Email).IsUnique();
            builder.Property(s => s.DateOfBirth).IsRequired();


            builder.HasData(
    new Student { Id = 1, FullName = "Sara Ali", Email = "sara@mail.com", DateOfBirth = new DateTime(2000, 5, 1) },
    new Student { Id = 2, FullName = "Omar Khaled", Email = "omar@mail.com", DateOfBirth = new DateTime(1999, 8, 15) },
    new Student { Id = 3, FullName = "Nour Tarek", Email = "nour@mail.com", DateOfBirth = new DateTime(2001, 2, 20) });
        }
    }
}
