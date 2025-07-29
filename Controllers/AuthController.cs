using DataBrokerAPI.Entities;
using DataBrokerAPI.Entities.DTOs;
using DataBrokerAPI.Repositories;
using DataBrokerAPI.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DataBrokerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration, IAuthService authService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }
        [HttpPost("registar")]
        public ActionResult<string> Register(RegistarCustomerReqeust request)
        {
            //Validate the request
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }

            if (string.IsNullOrEmpty(request.FirstName) ||
                string.IsNullOrEmpty(request.LastName) ||
                string.IsNullOrEmpty(request.Username) ||
                string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("All fields are required.");
            }

            if (request.FirstName.GetType() != typeof(string) ||
                request.LastName.GetType() != typeof(string) ||
                request.Username.GetType() != typeof(string) ||
                request.Password.GetType() != typeof(string))
            {
                return BadRequest("All fields must be of type string.");
            }

            var response = _authService.Registar(request);

            return Ok(response);

        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDTO>> Login(CustomerDTO request)
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

            //Create a token for the customer
            var tokens = await _authService.Login(request);

            return Ok(tokens);  
        }

        [Authorize] // This attribute ensures that the endpoint can only be accessed by authenticated users
        [HttpGet("auth")]
        public IActionResult AuthenticatedOnly()
        {
            return Ok("You are authenticated!");
        }

        [Authorize(Roles = "gold")]
        [HttpGet("goldmember")]
        public IActionResult AuthorizedOnly()
        {
            return Ok("You are a gold member!");
        }


    }
}
