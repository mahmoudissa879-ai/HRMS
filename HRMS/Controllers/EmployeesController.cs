using HRMS.DbContexts;
using HRMS.Dtos.Employees;
using HRMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;
namespace HRMS.Controllers
{

    //Data Annotations (Extra informations )
    [Route("api/[controller]")]// Route which Controller should requistAPI go /////[controller] => when run the app will replace it with class name( Employees ) 
    [ApiController]// Asp understand that this class is the Controller
    public class EmployeesController : ControllerBase  // there are something done i cane inherit it from ControllerBase
    {


        //HRMSContext _context=new HRMSContext(); normal way 

        // depandency injection
       
        private readonly HRMSContext _context;
   
      public EmployeesController(HRMSContext dbContext) // here i will receive the instance of the context from the DI container(container => file program.cs)
        {
            _context = dbContext;
        }

        public static List<Employee> employee = new List<Employee>()
        {

            new Employee { Id = 1,FirstName="ahmad",lastName="Emad",Email="ahmad123@gmail.com",Position="developer",BirthDate=new DateTime(2001,2,28),phoneNumber="0792432565" ,IsActive=true,StartDate=new DateTime(),Salary=1000},
                   new Employee { Id = 2,FirstName="sami",lastName="tareq",Email="sami123@gmail.com",Position="HR",BirthDate=new DateTime(2001,2,28),phoneNumber="07725458895" ,IsActive=true,StartDate=new DateTime(),Salary=1000},

                               new Employee { Id = 3,FirstName="mahmoud",lastName="hamed",Email="mahmoud23@gmail.com",Position="manager",BirthDate=new DateTime(2001,2,28),phoneNumber="0795645665" ,IsActive=true,StartDate=new DateTime(),Salary=1000},

                                           new Employee { Id = 4,FirstName="saleh",lastName="omar",Email="saleh123@gmail.com",Position="developer",BirthDate=new DateTime(2001,2,28),phoneNumber="0789089055" ,IsActive=true,StartDate=new DateTime(),Salary=1000}


 };
        // CRUD operations

        [HttpGet]//for Get type and route name 
        public IActionResult GetByCraiteria(string? position, string? name)// Endpoint // IActionResult allwo to return response //   quere parametiers => show in url
        {

            // return Ok(new { Name = "emp", Age = 30 });// 200 ok

            // return BadRequest("data not loaded ");//400 Bad  request 
            // return NotFound("employee not found");//404 not found 
            //  return StatusCode(500, "something went wrong");  //500 Internal error                                                                   



            ////////////////////////////////////////////////////////////////////////////////////////////

            ///IQueryable=> before execute the query to DB i can add more conditions to it and when i execute it will execute all the conditions in one query to DB
            var data = from emp in _context.Employees
                       from dep in _context.Departments.Where(x=>x.Id==emp.DepartmentId).DefaultIfEmpty()// join/ inner join// with department table to get the department name//DefaultIfEmpty()=>left join to return the employee even if he has no department
                       from manager in _context.Employees.Where(x=>x.Id==emp.ManagerId).DefaultIfEmpty()
                       where ((position == null || emp.Position.ToUpper().Contains(position.ToUpper())) && (name == null || emp.FirstName.ToUpper().Contains(name.ToUpper())))
                       orderby emp.Id descending
                       select new EmployeeDto//// dont return object with type model or take  return object with type model u should use DTO
                       {
                           Id = emp.Id,
                           Name = emp.FirstName + " " + emp.lastName,
                           Position = emp.Position,
                           BirthDate = emp.BirthDate,
                           StartDate = emp.StartDate,
                           EndDate = emp.EndDate,
                           Salary = emp.Salary,
                           Email = emp.Email,
                           IsActive = emp.IsActive,
                           phoneNumber = emp.phoneNumber,
                           DepartmentId = emp.DepartmentId,
                           ManagerId = emp.ManagerId,
                           DepartmentName = dep.Name ,
                          ManagerName=manager.FirstName+" "+manager.lastName


                       };

            return Ok(data);

        }

        [HttpGet("{id:long}")]//Route paramiter // {id:long}=> to ensure that the id is long type and not string or int or any other type
        public IActionResult GetById(long id)
        {

            var emp = _context.Employees.Select(x => new EmployeeDto
            { Id = x.Id,
                Name = x.FirstName + " " + x.lastName,
                Position = x.Position,
                BirthDate = x.BirthDate,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                IsActive = x.IsActive,
                phoneNumber = x.phoneNumber,
                DepartmentId = x.DepartmentId,
                ManagerId = x.ManagerId,
                DepartmentName = "dep.Name",
                ManagerName = " manager.FirstName +"



            })
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
        public IActionResult Add(SaveEmployeeDto employeeDto) {// request => Body  hidden from url 


            var emp = new Employee()
            {


                // Id =  (employee.LastOrDefault()?.Id ?? 0) + 1,
                FirstName = employeeDto.FirstName,
                lastName = employeeDto.lastName,
                Position = employeeDto.Position,
                BirthDate = employeeDto.BirthDate,
                StartDate = employeeDto.StartDate,
                EndDate = employeeDto.EndDate,
                Email = employeeDto.Email,
                IsActive = employeeDto.IsActive,
                phoneNumber = employeeDto.phoneNumber,
                Salary = employeeDto.Salary,
                DepartmentId = employeeDto.DepartmentId,
                ManagerId = employeeDto.ManagerId


            };


           _context.Employees.Add(emp);// here dont go to DB i must write  _context.SaveChanges(); why its important? => if i have more than one     _context.Employees.Add(emp) and for every add go to DB=>  low performance then the _context.SaveChanges(); go once
            _context.SaveChanges();
            return Ok(emp.Id);
        }





        [HttpPut("{id:long}")]// resource Update (whole object)
                 //  [HttpPatch]// the same as put but the difference is that put will update all the fields and patch will update only the fields that i want to update
        public IActionResult Update([FromBody] long id, [FromQuery] SaveEmployeeDto employeeDto)//[FromBody] (for Dto),[FromQuery](for id)    by defalute but i swape it 
        {
            if (id != employeeDto.Id)
            {
                return BadRequest("Id missMatch");
            }
            
                var emp = _context.Employees.FirstOrDefault(x => x.Id == employeeDto.Id);
                if (emp == null)
                {
                    return NotFound("employee not found");
                }
                else
                {
                    emp.FirstName = employeeDto.FirstName;
                    emp.lastName = employeeDto.lastName;
                    emp.Position = employeeDto.Position;
                    emp.BirthDate = employeeDto.BirthDate;
                    emp.StartDate = employeeDto.StartDate;
                    emp.EndDate = employeeDto.EndDate;
                    emp.Email = employeeDto.Email;
                    emp.IsActive = employeeDto.IsActive;
                    emp.phoneNumber = employeeDto.phoneNumber;
                    emp.Salary = employeeDto.Salary;
                    emp.DepartmentId = employeeDto.DepartmentId;
                    emp.ManagerId = employeeDto.ManagerId;
                
                    _context.SaveChanges();


                    return Ok(emp.Id);
                }

        }

        [HttpDelete("{id:long}")]
            public IActionResult Delete(long id)
            {

                var emp = _context.Employees.FirstOrDefault(x => x.Id == id);
                if (emp == null)
                {

                    return NotFound("employee not found");
                }
                else
              _context.Employees.Remove(emp);
            return Ok();
                }



            }



        }
    



// simple data type=> string , int , long ,...==>(by defalute ) query paramiters
// complix data type => model , Dto , object ...==>(by defalute ) request Body 


// Query paramiter => [frome query]

// request Body => [frome Body] for sinsative information like password or email or phone number

// method can use more than one query paramiter but only one paramiter can be request Body and the rest should be query paramiter

// HttpDelete  or HttpPut or HttpPatch or HttpPost => request Body but HttpGet => query paramiter


// Restfull API =>  is the way to design API that follow the principles of REST (Representational State Transfer) and use HTTP methods (GET, POST, PUT, DELETE) to perform CRUD operations on resources (data entities)

// the principles of RESTfull are: 1-dont named the API methods with verbs (Get, Post, Put, Delete) but with nouns (Employees, Customers, Orders) and the beast dont named it  2- use HTTP methods to perform CRUD operations 3- use status codes to indicate the result of the operation 4- use HATEOAS (Hypermedia as the Engine of Application State) to provide links to related resources 5- use versioning to manage changes in the API 6- use caching to improve performance 7- use security to protect the API from unauthorized access











