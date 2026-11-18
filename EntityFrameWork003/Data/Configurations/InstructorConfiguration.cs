using EntityFrameWork003.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EntityFrameWork003.Data.Configurations
{
    internal class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.FullName).IsRequired().HasMaxLength(100);
            builder.Property(i => i.Email).IsRequired().HasMaxLength(200);
            builder.HasIndex(i => i.Email).IsUnique();
            builder.Property(i => i.Bio).HasMaxLength(1000);
            builder.Property(i => i.JoinedAt).IsRequired();

            builder.HasMany(i => i.Courses)
                   .WithOne(c => c.Instructor)
                   .HasForeignKey(c => c.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
    new Instructor { Id = 1, FullName = "Ahmed Hassan", Email = "ahmed@learn.com", Bio = "Backend trainer", JoinedAt = new DateTime(2024, 1, 10) },
    new Instructor { Id = 2, FullName = "Mona Samir", Email = "mona@learn.com", JoinedAt = new DateTime(2024, 3, 5) });

        }

    }
}
