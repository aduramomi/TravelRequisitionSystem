using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelRequisitionSystem.Models
{
    public class ServiceResponse<T>
    {
        public bool IsSuccessful { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
    }
}
