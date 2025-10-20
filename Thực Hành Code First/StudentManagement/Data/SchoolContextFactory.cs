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
            
            // Sử dụng cùng instance SQL Server với runtime (SQLEXPRESS)
            optionsBuilder.UseSqlServer("Server=PHUNGTHEANH\\SQLEXPRESS;Database=StudentManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
            
            return new SchoolContext(optionsBuilder.Options);
        }
    }
}
