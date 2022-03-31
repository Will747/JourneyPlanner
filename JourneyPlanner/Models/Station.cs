
namespace JourneyPlanner.Models
{
    // Details of a train station
    public class Station
    {
        public string Name { get; set; }
        public string CodeName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LinkedStations{ get; set; }

        public double[] GetCoordinates()
        {
            return new[] { Longitude, Latitude };
        }
    }
}
