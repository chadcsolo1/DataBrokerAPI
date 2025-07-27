using DataBrokerAPI.Entities;
using DataBrokerAPI.Entities.DTOs;
using DataBrokerAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DataBrokerAPI.Services.AuthService
{
    public class AuthService : IAuthService
    {
        readonly ICustomerRepo _customerRepo;
        readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(ICustomerRepo customerRepo, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _customerRepo = customerRepo;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        //We need to add the login and registar mthods here in the Service Layer

        private string GenerateRefreshToken()
        {
            //Create a byte array of 32 bytes
            var randomNumber = new byte[32];

            //Create an instance of cryptographic random number generator
            using var rng = RandomNumberGenerator.Create();

            //Pass the byte array to the random number generator
            rng.GetBytes(randomNumber);

            //Convert the byte array to a Base64 string and return it
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<string> GenerateAndSaveRefreshToken(Customer customer)
        {
            //Create refresh token
            var refreshToken = GenerateRefreshToken();

            //Save refresh token to corresponding user properties and set expirey time
            customer.RefreshToken = refreshToken;
            customer.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);

            //Save changes to the database
            await _unitOfWork.SaveAsync();

            //return the refresh token
            return refreshToken;
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

        public async Task<TokenResponseDTO?> Login(CustomerDTO request)
        {
            //Validate the request
            if (request == null)
            {
                return null;
            }

            //Validate expected format of request
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return null;
            }

            if (request.Username.GetType() != typeof(string) && request.Password.GetType() != typeof(string))
            {
                throw new ArgumentException("Username and Password must be of type string.");
            }

            //Get the customer by username and if null , return null
            var customer = await _customerRepo.GetCustomerByUsername(request);
            if (customer == null)
            {
                return null; // or throw an exception
            }

            //Validate the password
            if (new PasswordHasher<Customer>().VerifyHashedPassword(customer, customer.PasswordHash,
                request.Password) == PasswordVerificationResult.Failed)
            {
                return null; // or throw an exception
            }


            //Create and populate TokenResponseDTO object
            var response = new TokenResponseDTO
            {
                AccessToken = await GenerateToken(request),
                RefreshToken = await GenerateAndSaveRefreshToken(customer),
            };

            //return the token and refresh token
            return response;

        }
    }
}
