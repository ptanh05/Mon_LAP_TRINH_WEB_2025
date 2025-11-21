using Microsoft.EntityFrameworkCore;

namespace lab06.Models
{
    // Class để khởi tạo dữ liệu mẫu cho database
    public static class DbInitializer
    {
        // Phương thức để seed dữ liệu vào database
        public static void Initialize(SchoolContext context)
        {
            // Đảm bảo database đã được tạo
            context.Database.EnsureCreated();

            // Kiểm tra xem đã có dữ liệu chưa
            if (context.Majors.Any())
            {
                // Nếu đã có dữ liệu thì không seed nữa
                return;
            }

            // Tạo dữ liệu mẫu cho các chuyên ngành
            var majors = new Major[]
            {
                new Major { MajorID = 1, MajorName = "IT" },
                new Major { MajorID = 2, MajorName = "Economics" },
                new Major { MajorID = 3, MajorName = "Mathematics" }
            };

            // Thêm các chuyên ngành vào database
            foreach (var major in majors)
            {
                context.Majors.Add(major);
            }
            context.SaveChanges();

            // Tạo dữ liệu mẫu cho các học viên
            var learners = new Learner[]
            {
                new Learner { LearnerID = 1, FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2005-09-01"), MajorID = 1 },
                new Learner { LearnerID = 2, FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2002-09-01"), MajorID = 2 },
                new Learner { LearnerID = 3, FirstMidName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2003-09-01"), MajorID = 1 },
                new Learner { LearnerID = 4, FirstMidName = "Gytis", LastName = "Barzdukas", EnrollmentDate = DateTime.Parse("2002-09-01"), MajorID = 3 },
                new Learner { LearnerID = 5, FirstMidName = "Nam", LastName = "Lê", EnrollmentDate = DateTime.Parse("2023-10-04"), MajorID = 3 },
                new Learner { LearnerID = 6, FirstMidName = "Peggy", LastName = "Justice", EnrollmentDate = DateTime.Parse("2001-09-01"), MajorID = 1 },
                new Learner { LearnerID = 7, FirstMidName = "Minh", LastName = "Trần", EnrollmentDate = DateTime.Parse("2023-10-04"), MajorID = 2 },
                new Learner { LearnerID = 8, FirstMidName = "Laura", LastName = "Norman", EnrollmentDate = DateTime.Parse("2003-09-01"), MajorID = 1 },
                new Learner { LearnerID = 9, FirstMidName = "Nino", LastName = "Olivetto", EnrollmentDate = DateTime.Parse("2005-09-01"), MajorID = 3 },
                new Learner { LearnerID = 10, FirstMidName = "An", LastName = "Nguyễn", EnrollmentDate = DateTime.Parse("2023-10-05"), MajorID = 1 },
                new Learner { LearnerID = 11, FirstMidName = "Bình", LastName = "Phạm", EnrollmentDate = DateTime.Parse("2023-10-06"), MajorID = 2 },
                new Learner { LearnerID = 12, FirstMidName = "Chi", LastName = "Hoàng", EnrollmentDate = DateTime.Parse("2023-10-07"), MajorID = 3 },
                new Learner { LearnerID = 13, FirstMidName = "Bích", LastName = "vũ", EnrollmentDate = DateTime.Parse("2023-10-23"), MajorID = 2 },
                new Learner { LearnerID = 14, FirstMidName = "Dũng", LastName = "Võ", EnrollmentDate = DateTime.Parse("2023-10-08"), MajorID = 1 },
                new Learner { LearnerID = 15, FirstMidName = "Hương", LastName = "Đỗ", EnrollmentDate = DateTime.Parse("2023-10-09"), MajorID = 2 }
            };

            // Thêm các học viên vào database
            foreach (var learner in learners)
            {
                context.Learners.Add(learner);
            }
            context.SaveChanges();
        }
    }
}

