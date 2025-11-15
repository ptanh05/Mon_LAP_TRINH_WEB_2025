using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab05.Models
{
    public class Learner
    {
        [Key]
        public int LearnerID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string FirstMidName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }
        
        [ForeignKey("Major")]
        public int MajorID { get; set; }
        
        public Major? Major { get; set; }
    }
}

