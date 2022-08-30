using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelRequisitionSystem.Models
{
    public class Location
    {
        public string name { get; set; }
        public string local_names { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string country { get; set; }
        public string state { get; set; }
    }

    public class Locations
    {
        public List<Location> LocationList { get; set; }
    }

    public class LocationDemo
    {
        public string name { get; set; }
    }
}
