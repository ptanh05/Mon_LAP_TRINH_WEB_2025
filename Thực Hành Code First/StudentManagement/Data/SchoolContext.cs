using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Data
{
    /// <summary>
    /// DbContext chính của ứng dụng quản lý sinh viên
    /// Kế thừa từ DbContext của Entity Framework Core
    /// </summary>
    public class SchoolContext : DbContext
    {
        /// <summary>
        /// Constructor của SchoolContext
        /// </summary>
        /// <param name="options">Các tùy chọn cấu hình cho DbContext</param>
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet cho bảng Students - quản lý thông tin sinh viên
        /// </summary>
        public DbSet<Student> Students { get; set; }

        /// <summary>
        /// DbSet cho bảng Courses - quản lý thông tin khóa học
        /// </summary>
        public DbSet<Course> Courses { get; set; }

        /// <summary>
        /// DbSet cho bảng Enrollments - quản lý việc đăng ký khóa học
        /// </summary>
        public DbSet<Enrollment> Enrollments { get; set; }

        /// <summary>
        /// Cấu hình các mối quan hệ và ràng buộc cho các entity
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder để cấu hình model</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình cho Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                // Thiết lập Primary Key
                entity.HasKey(s => s.Id);

                // Thiết lập các thuộc tính bắt buộc
                entity.Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.PhoneNumber)
                    .HasMaxLength(20);

                entity.Property(s => s.Address)
                    .HasMaxLength(200);

                // Thiết lập index cho Email để tăng hiệu suất tìm kiếm
                entity.HasIndex(s => s.Email)
                    .IsUnique();

                // Cấu hình mối quan hệ 1-nhiều với Enrollment
                entity.HasMany(s => s.Enrollments)
                    .WithOne(e => e.Student)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Cấu hình cho Course entity
            modelBuilder.Entity<Course>(entity =>
            {
                // Thiết lập Primary Key
                entity.HasKey(c => c.Id);

                // Thiết lập các thuộc tính bắt buộc
                entity.Property(c => c.CourseCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(c => c.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Description)
                    .HasMaxLength(500);

                entity.Property(c => c.Credits)
                    .IsRequired();

                // Thiết lập index cho CourseCode để tăng hiệu suất tìm kiếm
                entity.HasIndex(c => c.CourseCode)
                    .IsUnique();

                // Cấu hình mối quan hệ 1-nhiều với Enrollment
                entity.HasMany(c => c.Enrollments)
                    .WithOne(e => e.Course)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Cấu hình cho Enrollment entity
            modelBuilder.Entity<Enrollment>(entity =>
            {
                // Thiết lập Primary Key
                entity.HasKey(e => e.Id);

                // Thiết lập các thuộc tính bắt buộc
                entity.Property(e => e.StudentId)
                    .IsRequired();

                entity.Property(e => e.CourseId)
                    .IsRequired();

                entity.Property(e => e.EnrollmentDate)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                // Thiết lập composite index để đảm bảo một sinh viên không thể đăng ký cùng một khóa học nhiều lần
                entity.HasIndex(e => new { e.StudentId, e.CourseId })
                    .IsUnique();

                // Cấu hình foreign key constraints
                entity.HasOne(e => e.Student)
                    .WithMany(s => s.Enrollments)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Enrollments)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed data - thêm dữ liệu mẫu vào database
            SeedData(modelBuilder);
        }

        /// <summary>
        /// Thêm dữ liệu mẫu vào database khi khởi tạo
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder để thêm seed data</param>
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Courses
            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    CourseCode = "CS101",
                    Title = "Lập trình Cơ bản",
                    Description = "Khóa học lập trình cơ bản với C#",
                    Credits = 3
                },
                new Course
                {
                    Id = 2,
                    CourseCode = "CS201",
                    Title = "Cấu trúc Dữ liệu",
                    Description = "Khóa học về cấu trúc dữ liệu và thuật toán",
                    Credits = 4
                },
                new Course
                {
                    Id = 3,
                    CourseCode = "MATH101",
                    Title = "Toán học Cơ bản",
                    Description = "Khóa học toán học cơ bản",
                    Credits = 3
                }
            );

            // Seed Students
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    Name = "Nguyễn Văn An",
                    BirthDate = new DateTime(2000, 5, 15),
                    Email = "an.nguyen@email.com",
                    PhoneNumber = "0123456789",
                    Address = "Hà Nội"
                },
                new Student
                {
                    Id = 2,
                    Name = "Trần Thị Bình",
                    BirthDate = new DateTime(2001, 8, 20),
                    Email = "binh.tran@email.com",
                    PhoneNumber = "0987654321",
                    Address = "TP. Hồ Chí Minh"
                },
                new Student
                {
                    Id = 3,
                    Name = "Lê Văn Cường",
                    BirthDate = new DateTime(1999, 12, 10),
                    Email = "cuong.le@email.com",
                    PhoneNumber = "0369852147",
                    Address = "Đà Nẵng"
                }
            );

            // Seed Enrollments
            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment
                {
                    Id = 1,
                    StudentId = 1,
                    CourseId = 1,
                    EnrollmentDate = new DateTime(2024, 1, 15),
                    Grade = 8.5m,
                    Status = "Completed"
                },
                new Enrollment
                {
                    Id = 2,
                    StudentId = 1,
                    CourseId = 2,
                    EnrollmentDate = new DateTime(2024, 1, 15),
                    Grade = 9.0m,
                    Status = "Completed"
                },
                new Enrollment
                {
                    Id = 3,
                    StudentId = 2,
                    CourseId = 1,
                    EnrollmentDate = new DateTime(2024, 1, 20),
                    Grade = 7.5m,
                    Status = "Completed"
                },
                new Enrollment
                {
                    Id = 4,
                    StudentId = 3,
                    CourseId = 3,
                    EnrollmentDate = new DateTime(2024, 2, 1),
                    Grade = null,
                    Status = "Active"
                }
            );
        }
    }
}
