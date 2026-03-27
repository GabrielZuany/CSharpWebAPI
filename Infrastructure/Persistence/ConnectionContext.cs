using Domain.Employees.Model;
using Domain.RefreshTokens.Model;
using Domain.Users.Model;
using Infrastructure.Persistence.Configurations.Employees;
using Infrastructure.Persistence.Configurations.RefreshTokens;
using Infrastructure.Persistence.Configurations.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ConnectionContext : DbContext 
    {
        public DbSet<Employee> Employees { get; set; }       
        public DbSet<User> Users { get; set; }       
        public DbSet<RefreshToken> RefreshTokens { get; set; }       
    
        private readonly string _connectionString = "Host=localhost;Username=test;Password=test"; 
        
        public ConnectionContext(DbContextOptions<ConnectionContext> options)
            : base(options) { }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        }
    }
}
