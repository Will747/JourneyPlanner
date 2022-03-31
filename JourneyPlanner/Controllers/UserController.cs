using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using JourneyPlanner.Models;
using JourneyPlanner.Models.User;
using JourneyPlanner.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace JourneyPlanner.Controllers
{
    // Controller for handling requests related to user information
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseService _database;
        private readonly IStationService _stations;
        private readonly IPasswordService _password;

        public UserController(DatabaseService databaseService, IStationService stationService, IPasswordService passwordService)
        {
            _database = databaseService;
            _stations = stationService;
            _password= passwordService;
        }
        
        // GET /api/v1/user
        // Returns the currently logged in users information
        [HttpGet]
        public async Task<User> GetUser()
        {
            // Get the user ID
            var id = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            
            // Create SQL request to get user from the database
            await using var conn = _database.GetConnection();
            conn.Open();
            var command = new MySqlCommand(
                "SELECT * FROM users WHERE ID = @id ;", conn);
            command.Parameters.AddWithValue("@id", id);
            var reader = await command.ExecuteReaderAsync();
            
            // Create user based on response from the database
            var user = new User();
            while (await reader.ReadAsync())
            {
                user = new User
                {
                    Email =  Convert.ToString(reader["Email"]),
                    Id = new Guid(Convert.ToString(reader["ID"])!) ,
                    Username = Convert.ToString(reader["Username"])
                };
            }
            
            return user;
        }
        
        // POST /api/v1/user/update
        // Changes an existing users information
        [HttpPost("update")]
        public async Task<bool> UpdateUserDetails([FromBody] User userData)
        {
            var id = HttpContext.User.FindFirstValue(ClaimTypes.Sid);

            // If the ID in the request matches the one currently logged in make the update request to the database
            if (id == Convert.ToString(userData.Id))
            {
                await using var conn = _database.GetConnection();
                conn.Open();
                var command = new MySqlCommand(
                    "UPDATE users SET Username = @username , Email = @email WHERE ID= @id;", conn);
                command.Parameters.AddWithValue("@username", userData.Username);
                command.Parameters.AddWithValue("@email", userData.Email);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            else
            {
                return false;
            }

            return true;
        }
        
        // GET /api/v1/user/logout
        // Logs the user out
        [HttpGet("logout")]
        public async Task<bool> Logout()
        {
            await HttpContext.SignOutAsync();
            return true;
        }
        
        // GET /api/v1/user/routes
        // Gets any routes the user has saved
        [HttpGet("routes")]
        public async Task<List<UserRoute>> GetRoutes()
        {
            var id = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            
            // Make SQL request to get all routes saved by this user
            await using var conn = _database.GetConnection();
            conn.Open();
            var command = new MySqlCommand(
                "SELECT * FROM routes WHERE UserID = @id ;", conn);
            command.Parameters.AddWithValue("@id", id);
            var reader = await command.ExecuteReaderAsync();
            
            // Read data from SQL response
            var routes = new List<UserRoute>();
            while (await reader.ReadAsync())
            {
                var stops = new List<Station>();

                // Add all the stations in the route to the 'stops' array
                for (var i = 1; i <= 7; i++)
                {
                    var resultStr = Convert.ToString(reader["Stop" + i]);
                    if (resultStr != "")
                    {
                        stops.Add(_stations.GetStation(resultStr));
                    }
                }
                
                var route = new UserRoute
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    Type = Convert.ToInt32(reader["Type"]),
                    Stops = stops.ToArray(),
                    AlgorithmType = Convert.ToInt32(reader["AlgorithmType"])
                };
                
                routes.Add(route);
            }
            return routes;
        }
        
        // POST /api/v1/user/routes
        // Saves a new route to the database linked to the currently logged in user
        [HttpPost("routes")]
        public async Task<bool> AddRoute([FromBody] UserRoute input)
        {
            var id = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            
            // Make SQL request
            await using var conn = _database.GetConnection();
            conn.Open();
            var command = new MySqlCommand(
                @"INSERT INTO routes (UserID, Type, Stop1, Stop2, Stop3, Stop4, Stop5, Stop6, Stop7, AlgorithmType) 
                VALUES (@id, @type, @stop1, @stop2, @stop3, @stop4, @stop5, @stop6, @stop7, @algorithm);", conn);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@type", input.Type);
            
            // Add every required stop on the route
            for (var i = 1; i < 8; i++)
            {
                if (input.Stops.Length >= i)
                {
                    command.Parameters.AddWithValue("@stop" + i, input.Stops[i - 1].CodeName);
                }
                else
                {
                    command.Parameters.AddWithValue("@stop" + i, "");
                }
            }
            command.Parameters.AddWithValue("@algorithm", input.AlgorithmType);
            command.ExecuteNonQuery();

            return true;
        }
        
        // DELETE /api/v1/user/route/{id}
        // Removes a saved route from the database
        [HttpDelete("route/{id}")]
        public async Task<bool> DeleteRoute(int id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            await using var conn = _database.GetConnection();
            
            // Make SQL request to delete the route
            conn.Open();
            var command = new MySqlCommand(
                "DELETE FROM routes WHERE UserID = @userId AND ID = @id ;", conn);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@userId", userId);
            command.ExecuteNonQuery();

            return true;
        }
        
        
        
        // POST /api/v1/user/changePw
        // Changes the users password
        [HttpPost("changePw")]
        public async Task<bool> ChangePassword([FromBody] string[] passwords)
        {
            var id = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            var password = await _password.GetUserPassword(id);
            
            // Verify the original password was correct
            if (_password.VerifyPassword(passwords[0], password))
            {
                await using var conn = _database.GetConnection();
                
                // Make SQL request
                conn.Open();
                var command = new MySqlCommand(
                    "UPDATE users SET Password = @newPw WHERE ID = @id;", conn);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@newPw", _password.EncryptPassword(passwords[1]));
                await  command.ExecuteNonQueryAsync();
                return true;

            }

            return false;
        }
        
        // POST /api/v1/user/delete
        // Deletes a user removing all data linked to the user from the database
        [HttpPost("delete")]
        public async Task<bool> DeleteAccount([FromBody] LoginInput login)
        {
            var id = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            var hashedPassword = await _password.GetUserPassword(id);

            // Verify the user entered the correct password
            if (_password.VerifyPassword(login.password, hashedPassword))
            {
                await using var conn = _database.GetConnection();
                
                // Make SQL request to delete user and their saved routes
                conn.Open();
                var command = new MySqlCommand(
                    "DELETE FROM users WHERE ID = @id; DELETE FROM routes WHERE UserID = @id;", conn);
                command.Parameters.AddWithValue("@id", id);
                await  command.ExecuteNonQueryAsync();
                return true;

            }

            return false;
        }
        
    }
}