
namespace JourneyPlanner.Models.User
{
    // Format used when a create new user request is received
    public class CreateUser
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}