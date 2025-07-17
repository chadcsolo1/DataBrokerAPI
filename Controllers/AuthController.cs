using DataBrokerAPI.Entities;
using DataBrokerAPI.Entities.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DataBrokerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
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
            //Check that the usernames match or return bad request

            //Verify the hashed password
            return "Login successful";  
        }
    }
}
