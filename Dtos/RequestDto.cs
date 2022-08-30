using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelRequisitionSystem.Dtos
{
    public class RequestDto
    {
        public string RequisitionNumber { get; set; }
        public string TravelerName { get; set; }        
        public string SourceLocation { get; set; }        
        public string SourceCountry { get; set; }        
        public string DestinationLocation { get; set; }        
        public string DestinationCountry { get; set; }        
        public string ProposedDepartureDateAndTime { get; set; } 
        public string TravelClass { get; set; }        
        public string TripType { get; set; }
        public string RequestStatus { get; set; }
        public string ChargeCode { get; set; }
        public string RequestDate { get; set; }
    }
}
