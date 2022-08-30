using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TravelRequisitionSystem.Models;

namespace TravelRequisitionSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly string _token;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            _token = _configuration.GetSection("AppSettings:JwtToken").Value;
        }

        public ServiceResponse<string> GenerateToken(TokenPayLoad tokenPayLoad)
        {
            string msg = "";
            ServiceResponse<string> response = new ServiceResponse<string>();

            try
            {
                List<Claim> listClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, tokenPayLoad.UserName),
                    new Claim(ClaimTypes.Name, tokenPayLoad.UserName)
                };

                SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_token));

                SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

                SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(listClaims),
                    Expires = DateTime.Now.AddHours(1),
                    SigningCredentials = signingCredentials
                };

                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

                response.Data = jwtSecurityTokenHandler.WriteToken(securityToken);
                response.IsSuccessful = true;
            }
            catch (Exception eX)
            {
                msg = eX.Message;

                if (eX.InnerException != null)
                {
                    msg += "; " + eX.InnerException.Message; if (eX.InnerException.InnerException != null) { msg += ";" + eX.InnerException.InnerException.Message; }
                }

                response.ErrorMessage = msg;
            }

            return response;
        }
    }
}
