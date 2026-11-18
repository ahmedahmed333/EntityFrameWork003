using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameWork003.Models
{
    internal class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public int? ParentCategoryId { get; set; }          // null = top-level
        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
