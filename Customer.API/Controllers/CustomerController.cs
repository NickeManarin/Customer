using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Customers.Application.Interfaces;
using Customers.Application.Models;
using Microsoft.AspNetCore.Authorization;

namespace Customer.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _service;
        private readonly IAuthService _authService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService service, IAuthService authService)
        {
            _logger = logger;
            _service = service;
            _authService = authService;
        }

        // POST: api/v1/<CustomerController>
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] CustomerRequest request)
        {
            var (caller, isAdmin) = _authService.AdminTokenVerification(Request.Headers["Authorization"]);
            
            var result = await _service.GetCustomers(request, caller, isAdmin);
            
            return StatusCode(result.Status, result.Content);
        }
    }
}