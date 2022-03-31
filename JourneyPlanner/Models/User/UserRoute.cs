namespace JourneyPlanner.Models.User
{
    // A saved user created route
    public class UserRoute
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public Station[] Stops { get; set; }
        public int AlgorithmType { get; set; }
    }
}