using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Employees.Model
{
    public class Employee
    {
        public Guid Id { get; private set; }
        public string Fullname { get; private set; }
        public int Age { get; private set; }
        public string Postalcode { get; private set; }
        public string? Photo { get; private set; }
        public LevelAccessEnum LevelAccess { get; private set; }

        public Employee(string fullname, int age, string postalcode, string photo)
        {
            Id = Guid.NewGuid();
            Fullname = fullname ?? throw new ArgumentNullException(nameof(fullname));
            Age = age;
            Postalcode = postalcode;
            Photo = photo;
        }

        protected Employee() { }
    }
}
