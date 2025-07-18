using DataBrokerAPI.Data;
using DataBrokerAPI.Entities;
using System.Reflection.Metadata.Ecma335;

namespace DataBrokerAPI.Repositories.Implementations
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly ApplicationDbContext _db;
        public CustomerRepo(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public Customer GetCustomerByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty", nameof(username));
            }

            var customer = _db.Customers.FirstOrDefault(c => c.Username == username);

            return customer ?? throw new KeyNotFoundException($"Customer with username '{username}' not found.");
        }
    }
}
