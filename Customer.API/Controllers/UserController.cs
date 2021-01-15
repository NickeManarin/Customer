using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Customers.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Customer.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _service;

        public UserController(ILogger<UserController> logger, IUserService service)
        {
            _logger = logger;
            _service = service;
        }

        // GET: api/v1/<UserController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetUsers();

            return StatusCode(result.Code, result);
        }
    }
}