using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DataValidate.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Phải nhập tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phải nhập email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phải nhập mật khẩu")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ 8 đến 100 ký tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phải chọn ngành học")]
        public Branch Branch { get; set; }

        [Required(ErrorMessage = "Phải chọn giới tính")]
        public Gender Gender { get; set; }

        public bool IsRegular { get; set; }

        [Required(ErrorMessage = "Phải nhập địa chỉ")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phải nhập ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}
