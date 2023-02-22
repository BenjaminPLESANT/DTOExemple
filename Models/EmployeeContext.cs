using Microsoft.EntityFrameworkCore;

namespace DTOExemple.API.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employee { get; set; }
    }
}
