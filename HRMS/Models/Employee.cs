using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class Employee// Model
    {
 
        // [Key] annotation is used to specify that the Id property is the primary key but asp.net core will automatically consider the property named Id as the primary key 
        public long Id { get; set; }
        [MaxLength(50)]// to limit the length of the string to 50 characters
        public string FirstName { get;  set;}
        [MaxLength(50)]


        public string lastName { get; set; }
        [MaxLength(50)]

        public string? Email { get; set; }// (?)=> (opteinal) / => nullable 
     
        public DateTime BirthDate { get; set;}
        [MaxLength(50)]
        public string phoneNumber { get; set; }// why string ?   07 ===> there is no number start 0 or +962
        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }// required

        public DateTime? EndDate { get; set; }//nullable
        public decimal? Salary { get; set; }
        [ForeignKey("Department")]
        public long? DepartmentId { get; set; }// foreign key
       
        public Department? Department { get; set; }// navigation property for the relationship with the Department entity Entity Framework Core will to know that the DepartmentId property is a foreign key that references the Id property of the Department entity. The Department property is a navigation property that allows you to access the related Department entity from an Employee entity.
        [ForeignKey("Manager")]
        public long? ManagerId { get; set; }// foreign key
        public Employee? Manager { get; set; }// navigation property for the relationship with the Manager entity


        [ForeignKey("Lookup")]
        public long? PositionId { get; set; }// foreign key
        public lookup? Lookup { get; set; }// navigation property for the relationship with the Position entity
    }
}
