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
    public class RegionController : ControllerBase
    {
        private readonly ILogger<RegionController> _logger;
        private readonly IRegionService _service;

        public RegionController(ILogger<RegionController> logger, IRegionService service)
        {
            _logger = logger;
            _service = service;
        }

        // GET: api/v1/<RegionController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetRegions();

            return StatusCode(result.Code, result);
        }
    }
}