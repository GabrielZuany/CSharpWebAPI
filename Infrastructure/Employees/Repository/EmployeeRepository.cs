using Domain.Employees.Model;
using Domain.Employees.Repository;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Employees.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly ConnectionContext _context;

        public EmployeeRepository(ConnectionContext context)
        {
            _context = context;
        }
        
        public void Create(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public Employee? Get(Guid id)
        {
            return _context.Employees.Find(id);
        }

        public async Task<List<Employee>> GetAll(int page, int size)
        {
            return await _context
                .Employees
                .Skip(page * size)
                .Take(size)
                .ToListAsync();
        }
    }
}
