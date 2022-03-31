using System.Collections.Generic;

namespace JourneyPlanner.Models
{
    // A node used during the path finding algorithm
    public class Node
    {
        public int Score { get; set; }
        public int Cost { get; set; }
        public string StationCRS { get; set; }
        public string ParentCRS { get; set; }
        public List<string> ParentList { get; set; }
    }
}