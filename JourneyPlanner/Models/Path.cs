namespace JourneyPlanner.Models
{
    // Contains array of stops in order and the lines to be drawn on the map
    public class Path
    {
        public Station[] _stations { get; set; }
        public Line[] lines { get; set; }
        public int TotalDistance { get; set; }
    }
}