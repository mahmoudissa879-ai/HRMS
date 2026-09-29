namespace HRMS.Dtos.Employees
{
    //  DTO : Data transfer Object
    public class EmployeeDto
    {

        public long Id { get; set; }
        public string Name { get; set; }

        public string Position { get; set; }

        public DateTime BirthDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Email { get; set; }
        public string phoneNumber { get; set; }
        public bool IsActive { get; set; }
        public decimal? Salary { get; set; }
        public long? DepartmentId { get; set; }// foreign key
        public long? ManagerId { get; set; }// foreign key
        public string? DepartmentName { get; set; }
        public string? ManagerName { get; set; }

    }
}

