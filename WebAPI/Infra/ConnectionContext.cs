using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using WebAPI.Domain.Model;

namespace WebAPI.Infra
{
    public class ConnectionContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; } // map the Employee class to the employee table        
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder("Host=localhost;Username=test;Password=test");
            using var dataSource = dataSourceBuilder.Build();
        }


    }
}
