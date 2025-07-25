using DataBrokerAPI.Data;
using DataBrokerAPI.Entities;
using DataBrokerAPI.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DataBrokerAPI.Services.RefreshTokenService
{
    public class RefreshTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public RefreshTokenService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        private string CreateToken(Customer customer)
        {
            //Create claims for the customer
            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.NameIdentifier, customer.CustomerId.ToString()),
                new Claim(ClaimTypes.Name, customer.Username),
                new Claim(ClaimTypes.Role, customer.MemberShip) //Example role, can be dynamic based on customer data
            };

            //Symmetric Security Key - Signing key to ensure that the JWT actually came from this api
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));

            //Create the signing credentials using the key and the algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new JwtSecurityToken
            (
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1), // Token expiration time
                signingCredentials: creds
            );

            //Create the token using the JwtSecurityTokenHandler and return it
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private string GenerateRefreshToken()
        {
            //Create a byte array
            var randomNumber = new byte[32];

            //Create an instance of Random Number Generator class
            using var rng = RandomNumberGenerator.Create();

            //Fill the byte array with random bytes
            rng.GetBytes(randomNumber);

            //Convert the byte array to a Base64 string and return it
            return Convert.ToBase64String(randomNumber);
        }

        

        private async Task<string> GenerateAndSaveRefreshToken(Customer customer)
        {
            //Generate a new refresh token
            var refreshToken = GenerateRefreshToken();

            //Save the refresh token to the customer object
            customer.RefreshToken = refreshToken;

            //Set the refresh token expiry time to 1 days from now
            customer.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);

            //Save the changes to the database
            _unitOfWork.Save();

            //Return the refresh token
            return refreshToken;
        }
    }
}
