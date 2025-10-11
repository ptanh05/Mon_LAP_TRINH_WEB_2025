using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    /// <summary>
    /// Model đại diện cho khóa học trong hệ thống
    /// </summary>
    public class Course
    {
        /// <summary>
        /// ID duy nhất của khóa học (Primary Key)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Mã khóa học (ví dụ: CS101, MATH201)
        /// </summary>
        [Required(ErrorMessage = "Mã khóa học không được để trống")]
        [StringLength(10, ErrorMessage = "Mã khóa học không được vượt quá 10 ký tự")]
        [Display(Name = "Mã khóa học")]
        public string CourseCode { get; set; } = string.Empty;

        /// <summary>
        /// Tên khóa học
        /// </summary>
        [Required(ErrorMessage = "Tên khóa học không được để trống")]
        [StringLength(100, ErrorMessage = "Tên khóa học không được vượt quá 100 ký tự")]
        [Display(Name = "Tên khóa học")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả về khóa học
        /// </summary>
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        /// <summary>
        /// Số tín chỉ của khóa học
        /// </summary>
        [Required(ErrorMessage = "Số tín chỉ không được để trống")]
        [Range(1, 6, ErrorMessage = "Số tín chỉ phải từ 1 đến 6")]
        [Display(Name = "Số tín chỉ")]
        public int Credits { get; set; }

        /// <summary>
        /// Danh sách các sinh viên đã đăng ký khóa học này
        /// Navigation property - mối quan hệ 1-nhiều với Enrollment
        /// </summary>
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}