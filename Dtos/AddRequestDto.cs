using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelRequisitionSystem.Dtos
{
    public class AddRequestDto
    {
        [Required]
        public string TravelerName { get; set; }

        [Required]       
        public string SourceLocation { get; set; }

        [Required]
        public string SourceCountry { get; set; }

        [Required]       
        public string DestinationLocation { get; set; }

        [Required]
        public string DestinationCountry { get; set; }

        [Required]
        public string ProposedDepartureDate { get; set; }

        [Required]
        public string ProposedDepartureTime { get; set; }

        [Required]
        public string TravelClass { get; set; }

        [Required]
        public string TripType { get; set; }

        [Required]        
        public string ChargeCode { get; set; }       
    }
}
