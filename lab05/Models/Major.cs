using System.ComponentModel.DataAnnotations;

namespace lab05.Models
{
    public class Major
    {
        [Key]
        public int MajorID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string MajorName { get; set; } = string.Empty;
        
        public ICollection<Learner> Learners { get; set; } = new List<Learner>();
    }
}

