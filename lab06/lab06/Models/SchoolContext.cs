using Microsoft.EntityFrameworkCore;

namespace lab06.Models
{
    // DbContext để quản lý kết nối và truy vấn database
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        // DbSet cho bảng Learners
        public DbSet<Learner> Learners { get; set; }
        
        // DbSet cho bảng Majors
        public DbSet<Major> Majors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Cấu hình quan hệ giữa Learner và Major
            modelBuilder.Entity<Learner>()
                .HasOne(l => l.Major)
                .WithMany(m => m.Learners)
                .HasForeignKey(l => l.MajorID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

