using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Models;
using HRMS.Dtos.Employees;
namespace HRMS.Controllers
{

    //Data Annotations (Extra informations )
    [Route("api/[controller]")]// Route which Controller should requistAPI go /////[controller] => when run the app will replace it with class name( Employees ) 
    [ApiController]// Asp understand that this class is the Controller
    public class EmployeesController : ControllerBase  // there are something done i cane inherit it from ControllerBase
    {
        public List<Employee> employee = new List<Employee>()
        {

            new Employee { Id = 1,FirstName="ahmad",lastName="Emad",Email="ahmad123@gmail.com",Position="developer",BirthDate=new DateTime(2001,2,28),phoneNumber="0792432565" ,IsActive=true,StartDate=new DateTime(),Salary=1000},
                   new Employee { Id = 2,FirstName="sami",lastName="tareq",Email="sami123@gmail.com",Position="HR",BirthDate=new DateTime(2001,2,28),phoneNumber="07725458895" ,IsActive=true,StartDate=new DateTime(),Salary=1000},

                               new Employee { Id = 3,FirstName="mahmoud",lastName="hamed",Email="mahmoud23@gmail.com",Position="manager",BirthDate=new DateTime(2001,2,28),phoneNumber="0795645665" ,IsActive=true,StartDate=new DateTime(),Salary=1000},

                                           new Employee { Id = 4,FirstName="saleh",lastName="omar",Email="saleh123@gmail.com",Position="developer",BirthDate=new DateTime(2001,2,28),phoneNumber="0789089055" ,IsActive=true,StartDate=new DateTime(),Salary=1000}


 };


        [HttpGet("GetByCraiteria")]//for Get type and route name
        public IActionResult GetByCraiteria(string? position)// Endpoint // IActionResult allwo to return response
        {

            // return Ok(new { Name = "emp", Age = 30 });// 200 ok

            // return BadRequest("data not loaded ");//400 Bad  request 
            // return NotFound("employee not found");//404 not found 
            //  return StatusCode(500, "something went wrong");  //500 Internal error                                                                   



            ////////////////////////////////////////////////////////////////////////////////////////////


            var data = from emp in employee
                       where (position==null||emp.Position== position)
                       orderby emp.Id descending
                       select new EmployeeDto//// dont return object with type model or take  return object with type model u should use DTO
                       {
                           Id = emp.Id,
                           Name = emp.FirstName +" "+ emp.lastName,
                           Position = emp.Position,
                           BirthDate = emp.BirthDate,
                           StartDate = emp.StartDate,
                           EndDate = emp.EndDate

                       };

            return Ok(data);

        }

        [HttpGet("GetById")]
        public IActionResult GetById(long id)
        {

            var emp = employee.Select(x=>new EmployeeDto
            {Id=x.Id,Name=x.FirstName +" "+ x.lastName, Position=x.Position,BirthDate=x.BirthDate,StartDate=x.StartDate,EndDate=x.EndDate})
                .FirstOrDefault(x => x.Id == id);//FirstOrDefault=> return the first one with and if i enter invalid id will return null without exeption
                                                 //Select()=> to ensure that will return Dto not object model
            if (emp == null)
            {
                return NotFound("employee not found");
            }
            else
            return Ok(emp);



        }



        [HttpPost]
        public IActionResult Add(SaveEmployeeDto employeeDto)
        {


            var emp = new Employee()
            { 


                Id = (employee.LastOrDefault()?.Id??0)+1,
                FirstName = employeeDto.FirstName,
                lastName = employeeDto.lastName,
                Position = employeeDto.Position,
                BirthDate = employeeDto.BirthDate,
                StartDate = employeeDto.StartDate,
                EndDate = employeeDto.EndDate,
                Email = employeeDto.Email,
                IsActive = employeeDto.IsActive,
                phoneNumber = employeeDto.phoneNumber,
                Salary = employeeDto.Salary

            };


            employee.Add(emp);
            return Ok(emp.Id);
        }



    }


   
   




}
