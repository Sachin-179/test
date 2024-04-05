using Microsoft.EntityFrameworkCore;

namespace CsvToSqlConverter.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> employee { get; set; }
    }
}
