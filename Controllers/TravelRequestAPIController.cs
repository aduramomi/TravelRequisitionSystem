using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelRequisitionSystem.Dtos;
using TravelRequisitionSystem.Models;
using TravelRequisitionSystem.Services;

namespace TravelRequisitionSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TravelRequestAPIController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public TravelRequestAPIController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost("CreateRequest")]
        public async Task<IActionResult> Post([FromBody] AddRequestDto requestDto)
        {
            string msg = "";
            ServiceResponse<string> response = new ServiceResponse<string>();

            try
            {
                if (requestDto == null)
                {
                    response.ErrorMessage = "Parameter missing in request body";
                    return BadRequest(response);
                }

                response = await _requestService.AddRequest(requestDto);

                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }               
            }
            catch (Exception eX)
            {
                msg = eX.Message;

                if (eX.InnerException != null)
                {
                    msg += "; " + eX.InnerException.Message; if (eX.InnerException.InnerException != null) { msg += ";" + eX.InnerException.InnerException.Message; }
                }

                response.ErrorMessage = msg;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }           
        }

        [HttpPut("UpdateRequest/{requisitionNumber}")]
        public async Task<IActionResult> Put([FromRoute] string requisitionNumber, [FromBody] UpdateRequestDto requestDto)
        {
            string msg = "";
            ServiceResponse<string> response = new ServiceResponse<string>();

            try
            {
                if (string.IsNullOrEmpty(requisitionNumber))
                {
                    response.ErrorMessage = "Requisition number is required";
                    return BadRequest(response);
                }

                if (requestDto == null)
                {
                    response.ErrorMessage = "Parameter missing in request body";
                    return BadRequest(response);
                }

                response = await _requestService.UpdateRequest(requisitionNumber, requestDto);

                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception eX)
            {
                msg = eX.Message;

                if (eX.InnerException != null)
                {
                    msg += "; " + eX.InnerException.Message; if (eX.InnerException.InnerException != null) { msg += ";" + eX.InnerException.InnerException.Message; }
                }

                response.ErrorMessage = msg;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpDelete("DeleteRequest/{requisitionNumber}")]
        public async Task<IActionResult> Delete([FromRoute] string requisitionNumber)
        {
            string msg = "";
            ServiceResponse<string> response = new ServiceResponse<string>();

            try
            {
                if (string.IsNullOrEmpty(requisitionNumber))
                {
                    response.ErrorMessage = "Requisition number is required";
                    return BadRequest(response);
                }

                response = await _requestService.DeleteRequest(requisitionNumber);

                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception eX)
            {
                msg = eX.Message;

                if (eX.InnerException != null)
                {
                    msg += "; " + eX.InnerException.Message; if (eX.InnerException.InnerException != null) { msg += ";" + eX.InnerException.InnerException.Message; }
                }

                response.ErrorMessage = msg;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("GetRequest/{requisitionNumber}")]
        public async Task<IActionResult> GetRequest([FromRoute] string requisitionNumber)
        {
            string msg = "";
            ServiceResponse<RequestDto> response = new ServiceResponse<RequestDto>();

            try
            {
                if (string.IsNullOrEmpty(requisitionNumber))
                {
                    response.ErrorMessage = "Requisition number is required";
                    return BadRequest(response);
                }

                response = await _requestService.GetRequestByRequisitionNumber(requisitionNumber);

                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception eX)
            {
                msg = eX.Message;

                if (eX.InnerException != null)
                {
                    msg += "; " + eX.InnerException.Message; if (eX.InnerException.InnerException != null) { msg += ";" + eX.InnerException.InnerException.Message; }
                }

                response.ErrorMessage = msg;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("GetLocationDetails/{locationName}")]
        public async Task<IActionResult> GetLocationDetails([FromRoute] string locationName)
        {
            string msg = "";
            ServiceResponse<List<LocationDetail>> response = new ServiceResponse<List<LocationDetail>>();

            try
            {
                if (string.IsNullOrEmpty(locationName))
                {
                    response.ErrorMessage = "Location name is required";
                    return BadRequest(response);
                }

                response = await _requestService.GetLocationDetails(locationName);

                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception eX)
            {
                msg = eX.Message;

                if (eX.InnerException != null)
                {
                    msg += "; " + eX.InnerException.Message; if (eX.InnerException.InnerException != null) { msg += ";" + eX.InnerException.InnerException.Message; }
                }

                response.ErrorMessage = msg;

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
