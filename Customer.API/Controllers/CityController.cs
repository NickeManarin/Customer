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
    public class CityController : ControllerBase
    {
        private readonly ILogger<CityController> _logger;
        private readonly ICityService _service;

        public CityController(ILogger<CityController> logger, ICityService service)
        {
            _logger = logger;
            _service = service;
        }

        // GET: api/v1/<CityController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetCities();

            return StatusCode(result.Code, result);
        }
    }
}