using Microsoft.EntityFrameworkCore;
using WebAPI.Domain.Model;

namespace WebAPI.Infra
{
    public class ConnectionContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; } // map the Employee class to the employee table        
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Database=webapi;Username=webapi;Password=webapi");
        }


    }
}
