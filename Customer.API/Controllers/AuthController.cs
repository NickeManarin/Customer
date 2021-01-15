using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Customers.Application.Interfaces;
using Customers.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Customer.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _service;

        public AuthController(ILogger<AuthController> logger, IAuthService service)
        {
            _logger = logger;
            _service = service;
        }

        // POST: api/v1/<UserController>/signin
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] AuthorizationRequest request)
        {
            //Tries to create an user entry.
            var response = await _service.SignIn(request);

            return StatusCode(response.Code, response);
        }

        // POST: api/v1/<UserController>/refresh
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
        {
            //If the client detects that its access token is expired, it should call this method to recieve a new one.
            var response = await _service.RefreshAccessToken(request);

            return StatusCode(response.Code, response);
        }

        // POST: api/v1/<UserController>/revoke
        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshRequest request)
        {
            //Tries to logout.
            var response = await _service.RevokeRefreshToken(request);

            return StatusCode(response.Code, response);
        }
    }
}