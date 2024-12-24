using Npgsql.PostgresTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Domain.Model
{
    [Table("employee")]
    public class Employee
    {
        [Key]
        [Column("id")]
        public Guid Id { get; private set; }
        [Column("fullname")]
        public string Fullname { get; private set; }
        [Column("age")]
        public int Age { get; private set; }
        [Column("postalcode")]
        public string Postalcode { get; private set; }
        [Column("photo")]
        public string? Photo { get; private set; }

        public Employee(string fullname, int age, string postalcode, string photo)
        {
            Id = Guid.NewGuid();
            Fullname = fullname ?? throw new ArgumentNullException(nameof(fullname));
            Age = age;
            Postalcode = postalcode;
            Photo = photo;
        }

        public Employee()
        {
        }
    }
}
