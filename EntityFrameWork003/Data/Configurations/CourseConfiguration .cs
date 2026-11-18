using EntityFrameWork003.Models;
using EntityFrameWork003.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EntityFrameWork003.Data.Configurations
{
    internal class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
            builder.Property(c => c.Description).HasMaxLength(1000);
            builder.Property(c => c.Price).HasPrecision(10, 2);
            builder.Property(c => c.Level).HasConversion<string>().HasMaxLength(20);
            builder.Property(c => c.IsPublished).HasDefaultValue(false);


            builder.HasData(
    new Course
    {
        Id = 1,
        Title = "ASP.NET Core Fundamentals",
        Price = 499.99m,
        Level = CourseLevel.Beginner,
        IsPublished = true,
        PublishedDate = new DateTime(2024, 4, 1),
        InstructorId = 1,
        CategoryId = 3
    },
    new Course
    {
        Id = 2,
        Title = "Advanced EF Core",
        Price = 799m,
        Level = CourseLevel.Advanced,
        IsPublished = true,
        PublishedDate = new DateTime(2024, 5, 1),
        InstructorId = 1,
        CategoryId = 2
    },
    new Course
    {
        Id = 3,
        Title = "UI Design Basics",
        Price = 299m,
        Level = CourseLevel.Beginner,
        IsPublished = false,
        InstructorId = 2,
        CategoryId = 4
    });
        }
    }
}
