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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // we can use the modelBuilder to configure the model, such as setting up relationships between entities, configuring table names, etc.
            // we will use the modelBuilder to configure the model in the future if needed

         
            // seeding=>lookup table 

            modelBuilder.Entity<lookup>().HasData(

                //Employee Positions => [Major code=0]

                new lookup { Id = 1, MajorCode = 0, MinorCode = 0, Name = "Employee Position" },
                new lookup { Id = 2, MajorCode = 0, MinorCode = 1, Name = "HR" },
                new lookup { Id = 3, MajorCode = 0, MinorCode = 2, Name = "manager" },
                new lookup { Id = 4, MajorCode = 0, MinorCode = 3, Name = "developer" },



                     //Department Types => [Major code=1]
                new lookup { Id = 5, MajorCode = 1, MinorCode = 0, Name = "Department Types" },
                new lookup { Id = 6, MajorCode = 1, MinorCode = 1, Name = "finance" },
                new lookup { Id = 7, MajorCode =1, MinorCode = 2, Name = "adminstrative" },
                new lookup { Id = 8, MajorCode = 1, MinorCode = 3, Name = "Technical" }


            );







        }





        // DbSet(Table) is a collection of entities of a specific type that can be queried from the database. It represents a table in the database and allows you to perform CRUD operations on that table.
        public DbSet<Employee> Employees { get; set; }// the name of the DbSet property (Employees) will be used as the name of the table in the database.and the property in the model class (Employee) will be used as the name of the columns in the table.

      public DbSet<Department> Departments{ get; set; }
        public DbSet<lookup> lookups { get; set; }














    }
}
