using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelRequisitionSystem.Dtos;
using TravelRequisitionSystem.Models;

namespace TravelRequisitionSystem.Services
{
    public interface IRequestService
    {
        Task<ServiceResponse<string>> AddRequest(AddRequestDto requestDto);
        Task<ServiceResponse<string>> UpdateRequest(string requisitionNumber, UpdateRequestDto requestDto);
        Task<ServiceResponse<RequestDto>> GetRequestByRequisitionNumber(string requisitionNumber);
        Task<ServiceResponse<string>> DeleteRequest(string requisitionNumber);
        Task<ServiceResponse<List<LocationDetail>>> GetLocationDetails(string locationName);
    }
}
