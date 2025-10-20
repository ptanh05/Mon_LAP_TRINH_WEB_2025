using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    /// <summary>
    /// Model đại diện cho giảng viên
    /// </summary>
    public class Instructor
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, StringLength(100)]
        public string LastName { get; set; } = null!;

        [Required, EmailAddress, StringLength(200)]
        public string Email { get; set; } = null!;

        [Phone, StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        public string? Department { get; set; }

        [StringLength(500)]
        public string? Specialization { get; set; }

        public DateTime HireDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<ClassSection> ClassSections { get; set; } = new List<ClassSection>();

        // Computed property
        public string FullName => $"{FirstName} {LastName}";
    }
}
