
using HRMS.DbContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HRMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddEndpointsApiExplorer();// i added it
            builder.Services.AddSwaggerGen();// i added it


            //////////////////////////Token//////////////////////////////////////// i added it
            ///

            //////////////////////////JwtBearerDefaults => token type

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>

                {
                    var key = Encoding.UTF8.GetBytes("WHAFWEI#!@!!112312WQEQW@RWQEQW432");// define the secret key
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,  // the source where the token is Issued (the source where the token is created(from HR, or , fainance))
                        ValidateAudience = false,  // the User whome can use this token (opposite ValidateIssuer  ,   from mobile , angular ,  disktop)
                        ValidateIssuerSigningKey = true,// make sure the token is used my secret key only(if was false will take any token without useing secret key)
                        IssuerSigningKey = new SymmetricSecurityKey(key)//  generate the token using our secret key



                    };



                });


            //////////////////////////Token//////////////////////////////////////// close i added it




            // Container for dependency injection// i added it
            builder.Services.AddDbContext<HRMSContext>(options =>// i added it
            options.UseSqlServer(builder.Configuration.GetConnectionString("HRMSContext"))// i added it
            );



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwagger();// i added it
                app.UseSwaggerUI();// i added it

            }


            // Middleware (methods that  the requests before going to its controller it must go to Middleware in order )

            app.UseHttpsRedirection();
            app.UseAuthentication();// i added it (check if requests hold WHAFWEI#!@!!112312WQEQW@RWQEQW432 secret key only if the controller have [Authorize] data annotation      )
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
