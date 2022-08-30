using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelRequisitionSystem.Models
{
    public class Weather
    {
        public string coord { get; set; }
        public string weather { get; set; }      
        //public string base { get; set; }
        public string main { get; set; }

        public string visibility { get; set; }
        public string wind { get; set; }
        public string clouds { get; set; }

        public string dt { get; set; }
        public string sys { get; set; }
        public string timezone { get; set; }
        public long id { get; set; }

        public string name { get; set; }
        public string cod { get; set; }
    }
}
