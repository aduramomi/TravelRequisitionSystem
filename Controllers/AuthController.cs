using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelRequisitionSystem.Models;
using TravelRequisitionSystem.Services;

namespace TravelRequisitionSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("GenerateToken")]
        public IActionResult GenerateToken([FromBody] TokenPayLoad tokenPayLoad)
        {
            string msg = "";

            ServiceResponse<string> response = new ServiceResponse<string>();

            try
            {
                if (tokenPayLoad == null)
                {
                    response.ErrorMessage = "Parameter missing in request body";
                    return BadRequest(response);
                }

                if (string.IsNullOrEmpty(tokenPayLoad.UserName))
                {
                    response.ErrorMessage = "Username is required";
                    return BadRequest(response);
                }               

                response = _authService.GenerateToken(tokenPayLoad);

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
                msg += eX.Message;

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
