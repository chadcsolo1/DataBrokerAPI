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
        public CustomerRepo(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public async Task<Customer> GetCustomerByUsername(CustomerDTO request)
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


            return customer;
        }


    }
}
