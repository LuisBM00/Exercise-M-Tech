using Microsoft.EntityFrameworkCore;

namespace Exercise_M_Tech.Models
{
    public class APPDBContext : DbContext
    {
        public APPDBContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Employee> employees { get; set; }
    }
}
