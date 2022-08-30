using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelRequisitionSystem.Models
{
    public class ApiResponse<T>
    {
        public T TProperty { get; set; }
    }
}
