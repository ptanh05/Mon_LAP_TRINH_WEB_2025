using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StudentManagement.Data
{
    /// <summary>
    /// Design-time factory cho SchoolContext
    /// Được sử dụng bởi Entity Framework tools để tạo DbContext instance tại design time
    /// </summary>
    public class SchoolContextFactory : IDesignTimeDbContextFactory<SchoolContext>
    {
        /// <summary>
        /// Tạo instance của SchoolContext tại design time
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Instance của SchoolContext</returns>
        public SchoolContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchoolContext>();
            
            // Sử dụng LocalDB connection string cho design time
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SchoolDB;Trusted_Connection=true;MultipleActiveResultSets=true");
            
            return new SchoolContext(optionsBuilder.Options);
        }
    }
}
