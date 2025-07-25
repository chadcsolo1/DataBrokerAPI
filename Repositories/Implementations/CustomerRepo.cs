using DataBrokerAPI.Data;
using DataBrokerAPI.Entities;
using DataBrokerAPI.Entities.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DataBrokerAPI.Repositories.Implementations
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public CustomerRepo(ApplicationDbContext db, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public async Task<TokenResponseDTO> GetCustomerByUsername(CustomerDTO request)
        {
            if (string.IsNullOrEmpty(request.Username))
            {
                return null;
                //throw new ArgumentException("Username cannot be null or empty", nameof(request.Username));
            }

            var customer = _db.Customers.FirstOrDefault(c => c.Username == request.Username);

            if (new PasswordHasher<Customer>().VerifyHashedPassword(customer, customer.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var response = new TokenResponseDTO
            {
                AccessToken = CreateToken(customer),
                RefreshToken = await GenerateAndSaveRefreshToken(customer),
            };

            return response;
        }


    }
}
