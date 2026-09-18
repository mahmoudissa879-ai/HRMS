namespace HRMS.Models
{
    public class Employee// Model
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string lastName { get; set; }

        public string? Email { get; set; }// (?)=> (opteinal) / => nullable 

        public string Position { get; set; }
        public DateTime BirthDate { get; set; }

        public string phoneNumber { get; set; }// why string ?   07 ===> there is no number start 0 or +962
        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }// required

        public DateTime? EndDate { get; set; }//nullable
        public decimal? Salary { get; set; }



    }
}
