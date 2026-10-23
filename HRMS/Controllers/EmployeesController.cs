using HRMS.DbContexts;
using HRMS.Dtos.Employees;
using HRMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
namespace HRMS.Controllers
{

    //Data Annotations (Extra informations )
    [Authorize]
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


        // CRUD operations

        [HttpGet]//for Get type and route name 
        public IActionResult GetByCraiteria([FromQuery]SearchEmployeeDto searchEmployeeDto)// Endpoint // IActionResult allwo to return response //   quere parametiers => show in url
        {

            // return Ok(new { Name = "emp", Age = 30 });// 200 ok

            // return BadRequest("data not loaded ");//400 Bad  request 
            // return NotFound("employee not found");//404 not found 
            //  return StatusCode(500, "something went wrong");  //500 Internal error                                                                   



            ////////////////////////////////////////////////////////////////////////////////////////////
            ///


            try {
                ///IQueryable=> before execute the query to DB i can add more conditions to it and when i execute it will execute all the conditions in one query to DB
                var data = from emp in _context.Employees
                           from dep in _context.Departments.Where(x => x.Id == emp.DepartmentId).DefaultIfEmpty()// join/ inner join// with department table to get the department name//DefaultIfEmpty()=>left join to return the employee even if he has no department
                           from manager in _context.Employees.Where(x => x.Id == emp.ManagerId).DefaultIfEmpty()
                           from position in _context.lookups.Where(x => x.Id == emp.PositionId).DefaultIfEmpty()
                           where ((searchEmployeeDto.PositionId == null || emp.PositionId == searchEmployeeDto.PositionId)) && (searchEmployeeDto.Name == null || emp.FirstName.ToUpper().Contains(searchEmployeeDto.Name.ToUpper()))
                           orderby emp.Id descending
                           select new EmployeeDto//// dont return object with type model or take  return object with type model u should use DTO
                           {
                               Id = emp.Id,
                               Name = emp.FirstName + " " + emp.lastName,
                               PositionId = emp.PositionId,
                               PositionName = position.Name,
                               BirthDate = emp.BirthDate,
                               StartDate = emp.StartDate,
                               EndDate = emp.EndDate,
                               Salary = emp.Salary,
                               Email = emp.Email,
                               IsActive = emp.IsActive,
                               phoneNumber = emp.phoneNumber,
                               DepartmentId = emp.DepartmentId,
                               ManagerId = emp.ManagerId,
                               DepartmentName = dep.Name,
                               ManagerName = manager.FirstName + " " + manager.lastName


                           };

                return Ok(data);

            }

            catch (Exception ex)
            {
                return StatusCode(500, new Exception(ex.Message));
            }



        }

        [HttpGet("{id:long}")]//Route paramiter // {id:long}=> to ensure that the id is long type and not string or int or any other type
        public IActionResult GetById(long id)
        {


            try {


                var emp = _context.Employees.Select(x => new EmployeeDto
                {
                    Id = x.Id,
                    Name = x.FirstName + " " + x.lastName,
                    PositionId = x.PositionId,
                    PositionName = x.Lookup.Name,
                    BirthDate = x.BirthDate,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsActive = x.IsActive,
                    phoneNumber = x.phoneNumber,
                    DepartmentId = x.DepartmentId,
                    ManagerId = x.ManagerId,
                    // projection => select() => i call navigation proparty i can return only the fields that i want and not all the fields in the model Department and manager
                    DepartmentName = x.Department.Name,
                    ManagerName = x.Manager.FirstName + " " + x.Manager.lastName



                })
                    .FirstOrDefault(x => x.Id == id);//FirstOrDefault=> return the first one with and if i enter invalid id will return null without exeption
                                                     //Select()=> to ensure that will return Dto not object model







                // Lazy loading => opposite of Egger loading











                // Egger loading => include() => to include the related data from the Department table this for performance
                /*   var emp = _context.Employees.Include(x=>x.Department).Include(x=>x.Manager).FirstOrDefault(x => x.Id == id);*///Include()=> to include the related data from the Department table this for performance 
                if (emp == null)
                {
                    return NotFound("employee not found");
                }
                else
                    return Ok(emp);


            }
            catch (Exception ex)
            {
                return StatusCode(500, new Exception(ex.Message));
            }





        }



        [HttpPost]
        public IActionResult Add(SaveEmployeeDto employeeDto) {// request => Body  hidden from url 

            try
            {

                var emp = new Employee()
                {


                    // Id =  (employee.LastOrDefault()?.Id ?? 0) + 1,
                    FirstName = employeeDto.FirstName,
                    lastName = employeeDto.lastName,
                    PositionId = employeeDto.PositionId,
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
     



                   catch (Exception ex)
            {
                return StatusCode(500, new Exception(ex.Message));
            }


        }





        [HttpPut("{id:long}")]// resource Update (whole object)
                 //  [HttpPatch]// the same as put but the difference is that put will update all the fields and patch will update only the fields that i want to update
        public IActionResult Update( long id,  SaveEmployeeDto employeeDto)//[FromBody] (for Dto),[FromQuery](for id)    by defalute but i swape it 
        {
 

            try
            {
                if (id != employeeDto.Id)// validation
                {
                    return BadRequest("id not match");
                } 



 var emp = _context.Employees.FirstOrDefault(x => x.Id == employeeDto.Id);

                if (emp == null)// validation
               
                {
                    return NotFound("employee not found");
                } 

                emp.FirstName = employeeDto.FirstName;
                emp.lastName = employeeDto.lastName;
                emp.PositionId = employeeDto.PositionId;
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


            catch (Exception ex)
            {
                return StatusCode(500, new Exception (ex.Message));
            }


            
              
              

        }

        [HttpDelete("{id:long}")]
            public IActionResult Delete(long id)
            {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, new Exception(ex.Message));
            }



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











