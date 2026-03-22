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

        public async Task<Employee?> FindByUsernameAndPassword(string username, string password)
        {
            return await this._context
                .Employees
                .Where(w => w.Fullname == username && w.Id.ToString() == password)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }
        
        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public Employee? Get(Guid id)
        {
            return _context.Employees.Find(id);
        }

        public List<Employee> Get(int pageNumber, int pageQuantity)
        {
            return _context.Employees
                .Skip((pageNumber-1) * pageQuantity)
                .Take(pageQuantity)
                .ToList();
        }
    }
}
