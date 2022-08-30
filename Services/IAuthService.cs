using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelRequisitionSystem.Models;

namespace TravelRequisitionSystem.Services
{
    public interface IAuthService
    {
        ServiceResponse<string> GenerateToken(TokenPayLoad tokenPayLoad);
    }
}
