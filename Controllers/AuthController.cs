using DataBrokerAPI.Entities;
using DataBrokerAPI.Entities.DTOs;
using DataBrokerAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DataBrokerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuthController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        [HttpPost("registar")]
        public ActionResult<CustomerDTO> Register(CustomerDTO request)
        {
            //Create a new customer object
            Customer customer = new Customer();

            //hash the password
            var hashedPassword = new PasswordHasher<Customer>()
                .HashPassword(customer, request.Password);

            //map the request or customerDTO to an actual customer object
            customer.Username = request.Username;
            customer.PasswordHash = hashedPassword;

            return Ok(customer);

        }

        [HttpPost("login")]
        public ActionResult<string> Login(CustomerDTO request)
        {
            //Pull customer from the database by username
            var customer = _unitOfWork.CustomerRepo.GetCustomerByUsername(request.Username);

            //Check that the usernames match or return bad request
            if (customer == null || customer.Username != request.Username)
            {
                return BadRequest("Username or password is incorrect");
            }

            //Verify the hashed password
            if (new PasswordHasher<Customer>().VerifyHashedPassword(customer, customer.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return BadRequest("Username or password is incorrect");
            }
            return Ok("Login successful");  
        }
    }
}
