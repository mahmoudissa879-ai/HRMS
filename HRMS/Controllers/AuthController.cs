using HRMS.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Dtos.Auth;
using HRMS.Models;
using Microsoft.Identity.Client;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly HRMSContext _context;

        public AuthController(HRMSContext dbContext) // here i will receive the instance of the context from the DI container(container => file program.cs)
        {
            _context = dbContext;
        }

        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            // Validate the user credentials
            var user = _context.Users.FirstOrDefault(u => u.UserName.ToUpper() == loginDto.UserName.ToUpper());
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");// Unauthorized => 401
            }
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.HashedPassword)) // Admin123=>$2a$11$HzPJwNE.sqSGWyGFKcYeAuY/cgkdq6dQL70YNez/SVvXs6Me8AHKC
            {
                // but  the Bcrypt.HashPassword  hashing the password randomly so the hashed password will be different every time
                // it using the salt and cost factor to hash the password so the hashed password will be different every time
                // salt and cost factor values are in the hashed password itself but we dont know where  $2a$11$HzPJwNE.sqSGWyGFKcYeAuY/cgkdq6dQL70YNez/SVvXs6Me8AHKC
                // if we use the same salt and cost factor values to hashe Admin123 we will get the same hashed password every time


                // Generate a JWT token or any other authentication mechanism
                // For simplicity, let's return a success message
                return Unauthorized("Invalid username or password."); ;

            }



            // Token=> JWT barer 
            var token = GenerateJwtToken(user);
            return Ok(token);


            


        }

        private string GenerateJwtToken(User user)// its not a endpoint its a method to generate the JWT token
        {
            // Implement JWT token generation logic here





            // 1-claims => user information (username , role , userID)
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));// key => value
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            if (user.IsAdmin) {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
}
            else {

                var employee = _context.Employees.Include(x => x.Lookup).FirstOrDefault(x => x.UserId == user.Id);
           
              
                    claims.Add(new Claim(ClaimTypes.Role,employee.Lookup.Name));
           
               

            }











            //2- secret key + signing Token (password no one know it except me )


            //WHAFWEI#!@!!112312WQEQW@RWQEQW432(THIS is the secret key its a random numbers and sampolys i wrote but there are a limite of that secret kye )

            // SymmetricSecurityKey() takes a byte array as a parameter so we need to convert the secret key to byte array


            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("WHAFWEI#!@!!112312WQEQW@RWQEQW432"));

            //  signing credentials => secret key + hashing algorithm to sign the token
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);// HmacSha256 => hashing algorithm to sign the token



            // token settings => issuer(optinal) , audience (optinal), expiration time (optinal) , claims (optinal), signing credentials (optinal)


            var tokenSettings = new JwtSecurityToken
            (
                claims: claims,
                signingCredentials: credentials,
   expires: DateTime.Now.AddDays(1)   // token will expire in 1 day 

            );


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenSettings);// convert the token to string



            return token;










        }


    }
}
