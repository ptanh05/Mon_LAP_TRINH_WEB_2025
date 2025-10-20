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
        /// DbSet cho bảng Instructors - quản lý thông tin giảng viên
        /// </summary>
        public DbSet<Instructor> Instructors { get; set; }

        /// <summary>
        /// DbSet cho bảng ClassSections - quản lý lớp học phần
        /// </summary>
        public DbSet<ClassSection> ClassSections { get; set; }

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

            // Cấu hình cho Instructor entity
            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(i => i.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(i => i.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(i => i.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(i => i.PhoneNumber)
                    .HasMaxLength(20);

                entity.Property(i => i.Department)
                    .HasMaxLength(200);

                entity.Property(i => i.Specialization)
                    .HasMaxLength(500);

                entity.HasIndex(i => i.Email)
                    .IsUnique();
            });

            // Cấu hình cho ClassSection entity
            modelBuilder.Entity<ClassSection>(entity =>
            {
                entity.HasKey(cs => cs.Id);

                entity.Property(cs => cs.SectionCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(cs => cs.Room)
                    .HasMaxLength(50);

                entity.Property(cs => cs.Schedule)
                    .HasMaxLength(100);

                // Foreign key relationships
                entity.HasOne(cs => cs.Course)
                    .WithMany()
                    .HasForeignKey(cs => cs.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cs => cs.Instructor)
                    .WithMany(i => i.ClassSections)
                    .HasForeignKey(cs => cs.InstructorId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Unique constraint for section code per course
                entity.HasIndex(cs => new { cs.CourseId, cs.SectionCode })
                    .IsUnique();
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

                entity.Property(e => e.ClassSectionId)
                    .IsRequired();

                entity.Property(e => e.EnrollmentDate)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                // Thiết lập composite index để đảm bảo một sinh viên không thể đăng ký cùng một lớp học phần nhiều lần
                entity.HasIndex(e => new { e.StudentId, e.ClassSectionId })
                    .IsUnique();

                // Cấu hình foreign key constraints
                entity.HasOne(e => e.Student)
                    .WithMany(s => s.Enrollments)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Course)
                    .WithMany()
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ClassSection)
                    .WithMany(cs => cs.Enrollments)
                    .HasForeignKey(e => e.ClassSectionId)
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

            // Seed Instructors
            modelBuilder.Entity<Instructor>().HasData(
                new Instructor
                {
                    Id = 1,
                    FirstName = "Nguyễn",
                    LastName = "Văn Giảng",
                    Email = "giang.nguyen@university.edu",
                    PhoneNumber = "0123456789",
                    Department = "Khoa Công nghệ Thông tin",
                    Specialization = "Lập trình Web, Database",
                    HireDate = new DateTime(2020, 9, 1),
                    IsActive = true
                },
                new Instructor
                {
                    Id = 2,
                    FirstName = "Trần",
                    LastName = "Thị Minh",
                    Email = "minh.tran@university.edu",
                    PhoneNumber = "0987654321",
                    Department = "Khoa Công nghệ Thông tin",
                    Specialization = "Cấu trúc Dữ liệu, Thuật toán",
                    HireDate = new DateTime(2019, 3, 15),
                    IsActive = true
                },
                new Instructor
                {
                    Id = 3,
                    FirstName = "Lê",
                    LastName = "Văn Toán",
                    Email = "toan.le@university.edu",
                    PhoneNumber = "0369852147",
                    Department = "Khoa Toán",
                    Specialization = "Toán học Cơ bản, Thống kê",
                    HireDate = new DateTime(2021, 1, 10),
                    IsActive = true
                }
            );

            // Seed ClassSections
            modelBuilder.Entity<ClassSection>().HasData(
                new ClassSection
                {
                    Id = 1,
                    SectionCode = "CS101-01",
                    CourseId = 1,
                    InstructorId = 1,
                    MaxCapacity = 30,
                    CurrentEnrollment = 0,
                    StartDate = new DateTime(2024, 9, 1),
                    EndDate = new DateTime(2024, 12, 15),
                    Room = "A101",
                    Schedule = "T2, T4, T6 - 8:00-10:00",
                    IsActive = true
                },
                new ClassSection
                {
                    Id = 2,
                    SectionCode = "CS201-01",
                    CourseId = 2,
                    InstructorId = 2,
                    MaxCapacity = 25,
                    CurrentEnrollment = 0,
                    StartDate = new DateTime(2024, 9, 1),
                    EndDate = new DateTime(2024, 12, 15),
                    Room = "A102",
                    Schedule = "T3, T5 - 10:00-12:00",
                    IsActive = true
                },
                new ClassSection
                {
                    Id = 3,
                    SectionCode = "MATH101-01",
                    CourseId = 3,
                    InstructorId = 3,
                    MaxCapacity = 40,
                    CurrentEnrollment = 0,
                    StartDate = new DateTime(2024, 9, 1),
                    EndDate = new DateTime(2024, 12, 15),
                    Room = "B201",
                    Schedule = "T2, T4 - 14:00-16:00",
                    IsActive = true
                }
            );

            // Seed Enrollments
            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment
                {
                    Id = 1,
                    StudentId = 1,
                    CourseId = 1,
                    ClassSectionId = 1,
                    EnrollmentDate = new DateTime(2024, 1, 15),
                    MidtermGrade = 8.5m,
                    FinalGrade = 9.0m,
                    AssignmentGrade = 8.0m,
                    Grade = 8.7m,
                    Status = "Completed"
                },
                new Enrollment
                {
                    Id = 2,
                    StudentId = 1,
                    CourseId = 2,
                    ClassSectionId = 2,
                    EnrollmentDate = new DateTime(2024, 1, 15),
                    MidtermGrade = 9.0m,
                    FinalGrade = 9.5m,
                    AssignmentGrade = 8.5m,
                    Grade = 9.2m,
                    Status = "Completed"
                },
                new Enrollment
                {
                    Id = 3,
                    StudentId = 2,
                    CourseId = 1,
                    ClassSectionId = 1,
                    EnrollmentDate = new DateTime(2024, 1, 20),
                    MidtermGrade = 7.0m,
                    FinalGrade = 8.0m,
                    AssignmentGrade = 7.5m,
                    Grade = 7.6m,
                    Status = "Completed"
                },
                new Enrollment
                {
                    Id = 4,
                    StudentId = 3,
                    CourseId = 3,
                    ClassSectionId = 3,
                    EnrollmentDate = new DateTime(2024, 2, 1),
                    Grade = null,
                    Status = "Active"
                }
            );
        }
    }
}
