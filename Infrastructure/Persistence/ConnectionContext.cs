using Domain.Employees.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ConnectionContext : DbContext 
    {
        public DbSet<Employee> Employees { get; set; }       
    
        private readonly string _connectionString = "Host=localhost;Username=test;Password=test"; 
        
        public ConnectionContext(DbContextOptions<ConnectionContext> options)
            : base(options) { }
    }
}
