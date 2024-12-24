using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Application.ViewModel
{
    public class EmployeeViewModel
    {
        //[Display(Name = "id")]
        //public Guid? Id { get; set; }

        [Display(Name = "fullname")]
        public string Fullname { get; set; }
        [Display(Name = "age")]
        public int Age { get; set; }
        [Display(Name = "postalcode")]
        public string Postalcode { get; set; }
        [Display(Name = "photo")]
        public IFormFile Photo { get; set; }

        // public EmployeeViewModel(string fullname, int age, string postalcode)
        // {
        //     Id = Guid.NewGuid();
        //     Fullname = fullname;
        //     Age = age;
        //     Postalcode = postalcode;
        // }

    }
}
