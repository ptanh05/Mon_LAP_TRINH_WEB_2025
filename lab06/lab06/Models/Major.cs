using System.ComponentModel.DataAnnotations;

namespace lab06.Models
{
    // Model đại diện cho chuyên ngành
    public class Major
    {
        [Key]
        public int MajorID { get; set; } // ID của chuyên ngành
        
        [Required]
        [StringLength(50)]
        public string MajorName { get; set; } = string.Empty; // Tên chuyên ngành
        
        // Danh sách học viên thuộc chuyên ngành này
        public ICollection<Learner> Learners { get; set; } = new List<Learner>();
    }
}

