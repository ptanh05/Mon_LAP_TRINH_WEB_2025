using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab06.Models
{
    // Model đại diện cho học viên
    public class Learner
    {
        [Key]
        public int LearnerID { get; set; } // ID của học viên
        
        [Required]
        [StringLength(50)]
        [Display(Name = "Tên")]
        public string FirstMidName { get; set; } = string.Empty; // Tên
        
        [Required]
        [StringLength(50)]
        [Display(Name = "Họ")]
        public string LastName { get; set; } = string.Empty; // Họ
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày Nhập Học")]
        public DateTime EnrollmentDate { get; set; } // Ngày nhập học
        
        // Khóa ngoại đến Major
        [Display(Name = "Ngành")]
        public int MajorID { get; set; }
        
        // Navigation property đến Major
        [ForeignKey("MajorID")]
        public Major? Major { get; set; }
    }
}

