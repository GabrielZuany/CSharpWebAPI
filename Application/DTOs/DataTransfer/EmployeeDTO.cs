using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.DTOs
{
    public class EmployeeDTO
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; }
        public string? Photo { get; set; }
    }
}
