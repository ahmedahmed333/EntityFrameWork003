using EntityFrameWork003.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EntityFrameWork003.Data.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Description).HasMaxLength(500);

            builder.HasOne(c => c.ParentCategory)
                   .WithMany(p => p.SubCategories)
                   .HasForeignKey(c => c.ParentCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Courses)
                    .WithOne(c => c.Category)
                   .HasForeignKey(c => c.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasData(
    new Category { Id = 1, Name = "Programming" },
    new Category { Id = 2, Name = "Backend", ParentCategoryId = 1 },
    new Category { Id = 3, Name = "ASP.NET", ParentCategoryId = 2 },
    new Category { Id = 4, Name = "Design" },
    new Category { Id = 5, Name = "Mobile", ParentCategoryId = 1 });
        }
    }
}
