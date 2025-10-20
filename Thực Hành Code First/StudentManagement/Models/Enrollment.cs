using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    /// <summary>
    /// Model đại diện cho việc đăng ký khóa học của sinh viên
    /// Đây là bảng trung gian (Junction Table) giữa Student và Course
    /// </summary>
    public class Enrollment
    {
        /// <summary>
        /// ID duy nhất của enrollment (Primary Key)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID của sinh viên (Foreign Key)
        /// </summary>
        [Required]
        [Display(Name = "Sinh viên")]
        public int StudentId { get; set; }

        /// <summary>
        /// ID của khóa học (Foreign Key)
        /// </summary>
        [Required]
        [Display(Name = "Khóa học")]
        public int CourseId { get; set; }

        /// <summary>
        /// ID của lớp học phần (Foreign Key)
        /// </summary>
        [Required]
        [Display(Name = "Lớp học phần")]
        public int ClassSectionId { get; set; }

        /// <summary>
        /// Ngày đăng ký khóa học
        /// </summary>
        [Required(ErrorMessage = "Ngày đăng ký không được để trống")]
        [Display(Name = "Ngày đăng ký")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        /// <summary>
        /// Điểm giữa kỳ
        /// </summary>
        [Range(0, 10, ErrorMessage = "Điểm giữa kỳ phải từ 0 đến 10")]
        [Display(Name = "Điểm giữa kỳ")]
        public decimal? MidtermGrade { get; set; }

        /// <summary>
        /// Điểm cuối kỳ
        /// </summary>
        [Range(0, 10, ErrorMessage = "Điểm cuối kỳ phải từ 0 đến 10")]
        [Display(Name = "Điểm cuối kỳ")]
        public decimal? FinalGrade { get; set; }

        /// <summary>
        /// Điểm bài tập
        /// </summary>
        [Range(0, 10, ErrorMessage = "Điểm bài tập phải từ 0 đến 10")]
        [Display(Name = "Điểm bài tập")]
        public decimal? AssignmentGrade { get; set; }

        /// <summary>
        /// Điểm tổng kết (tự động tính hoặc nhập thủ công)
        /// </summary>
        [Range(0, 10, ErrorMessage = "Điểm tổng kết phải từ 0 đến 10")]
        [Display(Name = "Điểm tổng kết")]
        public decimal? Grade { get; set; }

        /// <summary>
        /// Trạng thái đăng ký (Active, Completed, Dropped)
        /// </summary>
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [StringLength(20)]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Active";

        /// <summary>
        /// Navigation property - tham chiếu đến sinh viên
        /// </summary>
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        /// <summary>
        /// Navigation property - tham chiếu đến khóa học
        /// </summary>
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } = null!;

        /// <summary>
        /// Navigation property - tham chiếu đến lớp học phần
        /// </summary>
        [ForeignKey("ClassSectionId")]
        public virtual ClassSection ClassSection { get; set; } = null!;

        /// <summary>
        /// Tính điểm tổng kết dựa trên điểm thành phần
        /// </summary>
        public decimal? CalculateFinalGrade()
        {
            if (MidtermGrade.HasValue && FinalGrade.HasValue && AssignmentGrade.HasValue)
            {
                // Trọng số: Giữa kỳ 30%, Cuối kỳ 50%, Bài tập 20%
                return (MidtermGrade.Value * 0.3m) + (FinalGrade.Value * 0.5m) + (AssignmentGrade.Value * 0.2m);
            }
            return null;
        }

        /// <summary>
        /// Lấy điểm chữ dựa trên điểm số
        /// </summary>
        public string GetGradeLetter()
        {
            var finalGrade = Grade ?? CalculateFinalGrade();
            if (!finalGrade.HasValue) return "N/A";

            return finalGrade.Value switch
            {
                >= 9.0m => "A+",
                >= 8.5m => "A",
                >= 8.0m => "B+",
                >= 7.0m => "B",
                >= 6.5m => "C+",
                >= 5.5m => "C",
                >= 5.0m => "D+",
                >= 4.0m => "D",
                _ => "F"
            };
        }

        /// <summary>
        /// Lấy điểm GPA (4.0 scale)
        /// </summary>
        public decimal GetGpaPoints()
        {
            var finalGrade = Grade ?? CalculateFinalGrade();
            if (!finalGrade.HasValue) return 0;

            return finalGrade.Value switch
            {
                >= 9.0m => 4.0m,
                >= 8.5m => 3.7m,
                >= 8.0m => 3.3m,
                >= 7.0m => 3.0m,
                >= 6.5m => 2.7m,
                >= 5.5m => 2.3m,
                >= 5.0m => 2.0m,
                >= 4.0m => 1.0m,
                _ => 0.0m
            };
        }
    }
}