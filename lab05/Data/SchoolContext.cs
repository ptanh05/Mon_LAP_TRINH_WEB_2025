using Microsoft.EntityFrameworkCore;
using lab05.Models;

namespace lab05.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Learner> Learners { get; set; }
        public DbSet<Major> Majors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Majors
            modelBuilder.Entity<Major>().HasData(
                new Major { MajorID = 1, MajorName = "IT" },
                new Major { MajorID = 2, MajorName = "Economics" },
                new Major { MajorID = 3, MajorName = "Mathematics" }
            );

            // Seed data for Learners
            modelBuilder.Entity<Learner>().HasData(
                new Learner 
                { 
                    LearnerID = 1, 
                    FirstMidName = "Carson", 
                    LastName = "Alexander", 
                    MajorID = 1, 
                    EnrollmentDate = new DateTime(2005, 9, 1) 
                },
                new Learner 
                { 
                    LearnerID = 2, 
                    FirstMidName = "Tuấn Nam", 
                    LastName = "Trần", 
                    MajorID = 1, 
                    EnrollmentDate = new DateTime(2023, 10, 5, 14, 46, 0) 
                },
                new Learner 
                { 
                    LearnerID = 3, 
                    FirstMidName = "Meredith", 
                    LastName = "Alonso", 
                    MajorID = 2, 
                    EnrollmentDate = new DateTime(2002, 9, 1) 
                },
                new Learner 
                { 
                    LearnerID = 4, 
                    FirstMidName = "Arturo", 
                    LastName = "Anand", 
                    MajorID = 2, 
                    EnrollmentDate = new DateTime(2003, 9, 1) 
                },
                new Learner 
                { 
                    LearnerID = 5, 
                    FirstMidName = "Gytis", 
                    LastName = "Barzdukas", 
                    MajorID = 3, 
                    EnrollmentDate = new DateTime(2002, 9, 1) 
                },
                new Learner 
                { 
                    LearnerID = 6, 
                    FirstMidName = "Yan", 
                    LastName = "Li", 
                    MajorID = 3, 
                    EnrollmentDate = new DateTime(2002, 9, 1) 
                }
            );
        }
    }
}

