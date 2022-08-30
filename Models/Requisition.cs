using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TravelRequisitionSystem.Enums;

namespace TravelRequisitionSystem.Models
{
    public class Requisition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequisitionId { get; set; }
       
        [Required]
        [MaxLength(50)]
        public string RequisitionNumber { get; set; } 

        [Required]
        public DateTime RequestDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string SourceLocation { get; set; }

        [Required]      
        public Country SourceCountry { get; set; }

        [Required]
        [MaxLength(100)]
        public string DestinationLocation { get; set; }

        [Required]
        public Country DestinationCountry { get; set; }

        [Required]
        public DateTime ProposedDepartureDateAndTime { get; set; }

        [Required]
        public TravelClass TravelClass { get; set; }

        [Required]
        public TripType TripType { get; set; }

        [Required]
        [MaxLength(20)]
        public string ChargeCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string TravelerName { get; set; }

        [Required]
        public RequisitionStatus RequisitionStatus { get; set; }       

        public DateTime? DateUpdated { get; set; }
        
        
    }
}
