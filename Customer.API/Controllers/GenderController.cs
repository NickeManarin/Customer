using System.Threading.Tasks;
using Customers.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Customer.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GenderController : ControllerBase
    {
        private readonly ILogger<GenderController> _logger;
        private readonly IGenderService _service;

        public GenderController(ILogger<GenderController> logger, IGenderService service)
        {
            _logger = logger;
            _service = service;
        }

        // GET: api/v1/<GenderController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetGenders();

            return StatusCode(result.Code, result);
        }
    }
}