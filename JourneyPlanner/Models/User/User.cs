using System;

namespace JourneyPlanner.Models.User
{
    // User information including encrypted password
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
    }
}