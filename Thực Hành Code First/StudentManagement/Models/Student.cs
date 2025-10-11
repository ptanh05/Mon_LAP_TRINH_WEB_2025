using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    /// <summary>
    /// Model đại diện cho sinh viên trong hệ thống
    /// </summary>
    public class Student
    {
        /// <summary>
        /// ID duy nhất của sinh viên (Primary Key)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Họ và tên sinh viên
        /// </summary>
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        [Display(Name = "Họ và tên")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Ngày sinh của sinh viên
        /// </summary>
        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Email của sinh viên
        /// </summary>
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Số điện thoại của sinh viên
        /// </summary>
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Địa chỉ của sinh viên
        /// </summary>
        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        /// <summary>
        /// Danh sách các khóa học mà sinh viên đã đăng ký
        /// Navigation property - mối quan hệ 1-nhiều với Enrollment
        /// </summary>
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}