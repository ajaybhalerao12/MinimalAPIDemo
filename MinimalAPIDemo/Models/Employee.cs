using System.ComponentModel.DataAnnotations;

namespace MinimalAPIDemo.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Name is required")]
        [StringLength(100,ErrorMessage ="Name can't be greater than 100")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Position is required!")]
        [StringLength(50,ErrorMessage ="Name can't be greater than 50")]
        public string Position { get; set; }

        [Range(30000, 200000, ErrorMessage ="Salary must be between 30000 and 200000")]
        public double Salary { get; set; }
    }
}
