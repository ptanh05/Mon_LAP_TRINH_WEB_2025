using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    /// <summary>
    /// Model đại diện cho lớp học phần
    /// </summary>
    public class ClassSection
    {
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string SectionCode { get; set; } = null!;

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        [Required]
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; } = null!;

        [Required]
        public int MaxCapacity { get; set; } = 30;

        [Required]
        public int CurrentEnrollment { get; set; } = 0;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [StringLength(50)]
        public string? Room { get; set; }

        [StringLength(100)]
        public string? Schedule { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        // Computed properties
        public bool IsFull => CurrentEnrollment >= MaxCapacity;
        public int AvailableSlots => MaxCapacity - CurrentEnrollment;
        public string DisplayName => $"{Course.CourseCode} - {SectionCode}";
    }
}
