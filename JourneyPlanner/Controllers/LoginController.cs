using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using JourneyPlanner.Models;
using JourneyPlanner.Models.User;
using JourneyPlanner.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace JourneyPlanner.Controllers
{
    // Controller for handling users logging in or creating new accounts
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/login")]
    public class LoginController : Controller
    {
        private readonly DatabaseService _database;
        private readonly IPasswordService _password;

        public LoginController(DatabaseService databaseService, IPasswordService passwordService)
        {
            _database = databaseService;
            _password = passwordService;
        }

        // POST /api/v1/login
        [HttpPost]
        public async Task<bool> Login([FromBody] LoginInput input)
        {
            // Make SQL request to get the user from the database
            await using var conn = _database.GetConnection();
            conn.Open();
            var command = new MySqlCommand(
                "SELECT * FROM users WHERE Username = @username;", conn);
            command.Parameters.AddWithValue("@username", input.username);
            var reader = await command.ExecuteReaderAsync();
            
            // Create User from database response
            var user = new User();
            while (await reader.ReadAsync())
            {
                user = new User
                {
                    Email =  Convert.ToString(reader["Email"]),
                    Id = new Guid(Convert.ToString(reader["ID"])!), 
                    Password = (byte[])reader["Password"],
                    Username = Convert.ToString(reader["Username"])
                };
            }
            
            // Check if the password is correct or not
            if (_password.VerifyPassword(input.password, user.Password))
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.Username));
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                claims.Add(new Claim(ClaimTypes.Sid, Convert.ToString(user.Id)));
                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "User")));
                return true;
            } 
            return false;
        }
        
        // POST /api/v1/login/create
        // Creates a new user, adding them to the database
        [HttpPost("create")]
        public async Task<bool> CreateUser([FromBody] CreateUser userDetails)
        {
            // Make SQL request to create a new user
            await using var conn = _database.GetConnection();
            conn.Open();
            var command = new MySqlCommand(
                "INSERT INTO users (ID, Username, Password, Email) VALUES (@id, @username, @password, @email);", conn);
            command.Parameters.AddWithValue("@id", Guid.NewGuid());
            command.Parameters.AddWithValue("@username", userDetails.Username);
            command.Parameters.AddWithValue("@email", userDetails.Email);
            command.Parameters.Add("@password", MySqlDbType.Blob).Value = _password.EncryptPassword(userDetails.Password);
            await command.ExecuteNonQueryAsync();

            // Log the new user in so they don't have to re-enter all login information
            await Login(new LoginInput{ username = userDetails.Username, password = userDetails.Password});
            
            return true;
        }
        
        // GET /api/v1/login
        // Returns true if the user is authenticated
        [HttpGet]
        public bool LoggedIn()
        {
            return HttpContext.User.FindFirstValue(ClaimTypes.Sid) != null;
        }
    }
}