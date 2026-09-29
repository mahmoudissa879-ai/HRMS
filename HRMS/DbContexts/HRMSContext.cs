using HRMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS.DbContexts

{
    public class HRMSContext : DbContext
    {

        // the parameter options is used to configure the context, such as the database provider and connection string.
        // options is an instance of the DbContextOptions class, which contains the configuration settings for the context.
        public HRMSContext(DbContextOptions<HRMSContext> options) : base(options)
        {
            // options 
            // 1) which database provider to use (e.g., SQL Server, SQLite, etc.)
            // 2) the connection string to the database
            // we will configure the options in appsettings.json file and then pass them to the context when we create an instance of it

        }
        // DbSet(Table) is a collection of entities of a specific type that can be queried from the database. It represents a table in the database and allows you to perform CRUD operations on that table.
        public DbSet<Employee> Employees { get; set; }// the name of the DbSet property (Employees) will be used as the name of the table in the database.and the property in the model class (Employee) will be used as the name of the columns in the table.

        public DbSet<Department> Departments{ get; set; }







    }
}
