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
        /// Ngày đăng ký khóa học
        /// </summary>
        [Required(ErrorMessage = "Ngày đăng ký không được để trống")]
        [Display(Name = "Ngày đăng ký")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        /// <summary>
        /// Điểm số của sinh viên trong khóa học (có thể null nếu chưa có điểm)
        /// </summary>
        [Range(0, 10, ErrorMessage = "Điểm số phải từ 0 đến 10")]
        [Display(Name = "Điểm số")]
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
    }
}