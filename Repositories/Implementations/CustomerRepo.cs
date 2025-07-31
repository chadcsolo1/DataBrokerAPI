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

            return customer ?? throw new Exception("No customer found in the database.");
        }

        public async Task<string> CreateCustomer(Customer customer)
        {
            // Validate the customer object
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer), "Customer cannot be null");
            }


            // Check if the customer already exists
            var existingCustomer = _db.Customers.FirstOrDefault(c => c.Username == customer.Username);

            if (existingCustomer != null)
            {
                throw new InvalidOperationException("Customer with this username already exists.");
            }

            // Save this transaction within the service layer via UnitOfWork
            _db.Customers.Add(customer); 
            return "CreateCustomer method not implemented yet.";
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid customer ID", nameof(id));
            }
            var customer = _db.Customers.FirstOrDefault(c => c.CustomerId == id);
            return customer ?? throw new Exception("No customer found with the provided ID.");


        }
}
