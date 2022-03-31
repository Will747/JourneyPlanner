namespace JourneyPlanner.Models
{
    // Holds the geographical coordinates to form a line between two stations
    public class Line
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Distance { get; set; }
        public Coordinates[] Coordinates { get; set; }
    }

    public class Coordinates
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}