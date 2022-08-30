using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelRequisitionSystem.Models
{
    public class TokenPayLoad
    {
        [Required]
        public string UserName { get; set; }
    }
}
