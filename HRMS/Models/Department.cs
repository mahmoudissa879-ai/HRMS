using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class Department
    {
        public long Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
       
        public string Description { get; set; }

        public string ? floorNumber { get; set; }

        // Navigation property for related employees
        // list of employees becouse one department can have many employees
        // Icollection 
       // public ICollection<Employee> Employees { get; set; }
    }
}
