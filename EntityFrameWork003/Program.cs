using EntityFrameWork003.Data;
using EntityFrameWork003.Models;
using EntityFrameWork003.Models.Enums;
using Microsoft.EntityFrameworkCore;
namespace EntityFrameWork003
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // create 

            static int AddInstructor(string name, string email, string bio = null)
            {
                using var db = new LearningContext();
                var instructor = new Instructor
                {
                    FullName = name,
                    Email = email,
                    JoinedAt = DateTime.Now
                };

                db.Instructors.Add(instructor);

                db.SaveChanges();
                Console.WriteLine($"Instructor added with Id = {instructor.Id}");
                return instructor.Id;

            }


            static int AddCourse(int instructorId, int categoryId, string title, decimal price, CourseLevel level)
            {
                using var db = new LearningContext();

                if (!db.Instructors.Any(i => i.Id == instructorId))
                { Console.WriteLine($"Instructor {instructorId} not found."); return 0; }

                if (!db.Categories.Any(c => c.Id == categoryId))
                { Console.WriteLine($"Category {categoryId} not found."); return 0; }


                var course = new Course
                {
                    Title = title,
                    Price = price,
                    Level = level,
                    InstructorId = instructorId,
                    CategoryId = categoryId
                };

                db.Courses.Add(course);

                db.SaveChanges();
                Console.WriteLine($"Course added with Id = {course.Id}");
                return course.Id;

            }

            static List<int> AddStudents()
            {
                using var db = new LearningContext();

                var students = new List<Student>
    {
        new() { FullName = "Hana Adel",   Email = "hana@mail.com",   DateOfBirth = new DateTime(2002, 1, 1) },
        new() { FullName = "Youssef Gaber", Email = "youssef@mail.com", DateOfBirth = new DateTime(2000, 6, 6) },
        new() { FullName = "Laila Fathy", Email = "laila@mail.com",  DateOfBirth = new DateTime(2001, 9, 9) }
    };
                db.Students.AddRange(students);

                db.SaveChanges();
                Console.WriteLine($"Students added with Ids = {string.Join(", ", students.Select(s => s.Id))}");
                return students.Select(s => s.Id).ToList();

            }

            static void EnrollStudent(int studentId, int courseId)
            {
                using var db = new LearningContext();

                if (!db.Students.Any(s => s.Id == studentId))
                { Console.WriteLine($"Refused: student {studentId} does not exist."); return; }

                var course = db.Courses.Find(courseId);
                if (course is null)
                { Console.WriteLine($"Refused: course {courseId} does not exist."); return; }

                if (!course.IsPublished)
                { Console.WriteLine($"Refused: course '{course.Title}' is not published."); return; }

                if (db.Enrollments.Any(e => e.StudentId == studentId && e.CourseId == courseId))
                { Console.WriteLine($"Refused: student {studentId} is already enrolled in course {courseId}."); return; }

                db.Enrollments.Add(new Enrollment
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    EnrollmentDate = DateTime.Now,
                    Progress = 0,
                    Status = EnrollmentStatus.Active
                });
                db.SaveChanges();
                Console.WriteLine($"Student {studentId} enrolled in course {courseId}.");
            }
            // read 
            static void FindCourse(int id, string title)
            {
                using var db = new LearningContext();

                var byId = db.Courses.Find(id);

                Console.WriteLine(byId is null ? $"Course Id {id}: not found" : $"By Id: {byId.Title}");

                var byTitle = db.Courses.FirstOrDefault(c => c.Title == title);
                Console.WriteLine(byTitle is null ? $"Course '{title}': not found" : $"By title: {byTitle.Id} - {byTitle.Title}");


            }
            static void FilterCourses(decimal min, decimal max)
            {
                using var db = new LearningContext();

                var courses = db.Courses
                    .Where(c => c.Price >= min && c.Price <= max)
                    .OrderByDescending(c => c.Price)
                    .Select(c => new { c.Title, c.Level, c.Price })
                    .ToList(); ;

                Console.WriteLine($"Courses between {min} and {max}:");
                courses.ForEach(c => Console.WriteLine($"  {c.Title} | {c.Level} | {c.Price}"));

            }

            static void PrintSubCategories(int parentId)
            {
                using var db = new LearningContext();
                var subs = db.Categories.Where(c => c.ParentCategoryId == parentId).Select(c => c.Name).ToList();
                Console.WriteLine($"Sub-categories of {parentId}: {(subs.Count == 0 ? "none" : string.Join(", ", subs))}");
            }

            static void PrintTopLevelCategories()
            {
                using var db = new LearningContext();
                var tops = db.Categories.Where(c => c.ParentCategoryId == null).Select(c => c.Name).ToList();
                Console.WriteLine($"Top-level categories: {string.Join(", ", tops)}");
            }


            static void PrintCourseRoster(int courseId)
            {
                using var db = new LearningContext();

                var studentIds = db.Enrollments
                    .Where(e => e.CourseId == courseId)
                    .Select(e => e.StudentId)
                    .ToList();

                var names = db.Students
                    .Where(s => studentIds.Contains(s.Id))
                    .Select(s => s.FullName)
                    .ToList();

                Console.WriteLine($"Course {courseId} has {names.Count} student(s): {string.Join(", ", names)}");
            }

            // update
            static void ChangePrice(int courseId, decimal newPrice)
            {
                using var db = new LearningContext();
                var course = db.Courses.Find(courseId);
                if (course is null) { Console.WriteLine($"Course {courseId} not found."); return; }
                if (newPrice < 0) { Console.WriteLine("Price cannot be negative."); return; }

                course.Price = newPrice;
                db.SaveChanges();
                Console.WriteLine($"Price of '{course.Title}' is now {newPrice}.");
            }
            static void UpdateProgress(int studentId, int courseId, int progress)
            {
                using var db = new LearningContext();

                if (progress < 0 || progress > 100)
                { Console.WriteLine($"Rejected: progress {progress} must be between 0 and 100."); return; }

                var enrollment = db.Enrollments.Find(studentId, courseId);   // نفس ترتيب HasKey
                if (enrollment is null)
                { Console.WriteLine("Enrollment not found."); return; }

                enrollment.Progress = progress;
                if (progress == 100) enrollment.Status = EnrollmentStatus.Completed;

                db.SaveChanges();
                Console.WriteLine($"Progress = {progress}, status = {enrollment.Status}.");
            }

            static void PublishCourse(int courseId)
            {
                using var db = new LearningContext();
                var course = db.Courses.Find(courseId);
                if (course is null) { Console.WriteLine($"Course {courseId} not found."); return; }

                course.IsPublished = true;
                course.PublishedDate = DateTime.Now;
                db.SaveChanges();
                Console.WriteLine($"Course '{course.Title}' published.");
            }



            // delete 

            static void Unenroll(int studentId, int courseId)
            {
                using var db = new LearningContext();
                var enrollment = db.Enrollments.Find(studentId, courseId);
                if (enrollment is null) { Console.WriteLine("Enrollment not found."); return; }

                db.Enrollments.Remove(enrollment);
                db.SaveChanges();
                Console.WriteLine($"Student {studentId} unenrolled from course {courseId}.");
            }

            static void DeleteCategoryUnsafe(int categoryId)
            {
                using var db = new LearningContext();
                var category = db.Categories.Find(categoryId);
                if (category is null) { Console.WriteLine("Category not found."); return; }

                try
                {
                    db.Categories.Remove(category);
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Delete failed: {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            static void DeleteCategorySafe(int categoryId)
            {
                using var db = new LearningContext();
                var category = db.Categories.Find(categoryId);
                if (category is null) { Console.WriteLine("Category not found."); return; }

                if (db.Categories.Any(c => c.ParentCategoryId == categoryId))
                { Console.WriteLine($"Refused: '{category.Name}' still has sub-categories."); return; }

                if (db.Courses.Any(c => c.CategoryId == categoryId))
                { Console.WriteLine($"Refused: '{category.Name}' still has courses."); return; }

                db.Categories.Remove(category);
                db.SaveChanges();
                Console.WriteLine($"Category '{category.Name}' deleted.");
            }

            static void DeleteCourseWithEnrollments(int courseId)
            {
                using var db = new LearningContext();
                var course = db.Courses.Find(courseId);
                if (course is null) { Console.WriteLine("Course not found."); return; }

                Console.WriteLine($"Enrollments before: {db.Enrollments.Count(e => e.CourseId == courseId)}");

                db.Courses.Remove(course);
                db.SaveChanges();
                Console.WriteLine($"Enrollments after: {db.Enrollments.Count(e => e.CourseId == courseId)}");
            }

        }
    }
}
