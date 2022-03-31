namespace JourneyPlanner.Models
{
    // The format of the request when a user attempts to log in
    public class LoginInput
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}