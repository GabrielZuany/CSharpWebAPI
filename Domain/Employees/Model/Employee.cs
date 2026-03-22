using Domain.BaseDomain;

namespace Domain.Employees.Model
{
    public class Employee : Base
    {
        public string Fullname { get; private set; }
        public int Age { get; private set; }
        public string Postalcode { get; private set; }
        public string? Photo { get; private set; }

        public Employee(string fullname, int age, string postalcode, string photo)
        {
            Fullname = fullname;
            Age = age;
            Postalcode = postalcode;
            Photo = photo;
        }

        protected Employee() { }
    }
}
