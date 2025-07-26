using DataBrokerAPI.Entities;
using DataBrokerAPI.Entities.DTOs;
using DataBrokerAPI.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DataBrokerAPI.Services.AuthService
{
    public class AuthService : IAuthService
    {
        readonly ICustomerRepo _customerRepo;
        readonly IConfiguration _configuration;
        public AuthService(ICustomerRepo customerRepo, IConfiguration configuration)
        {
            _customerRepo = customerRepo;
            _configuration = configuration;
        }

        public Task<TokenResponseDTO> GenerateRefreshToken(CustomerDTO request)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GenerateToken(CustomerDTO request)
        {
            //Retrieve Customer by Username
            Customer customer = await _customerRepo.GetCustomerByUsername(request);

            //Create Claim
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, customer.MemberShip)
            };

            //Create Key
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!)); 

            //Create Creds
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken
            (
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
