namespace HRMS.Dtos.Employees
{
    public class SaveEmployeeDto
    {
        public long? Id { get; set; }//nullable
        public string FirstName { get; set; }
        public string lastName { get; set; }

        public string? Email { get; set; }

        public string Position { get; set; }
        public DateTime BirthDate { get; set; }

        public string phoneNumber { get; set; }
        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public decimal? Salary { get; set; }

    }

}
