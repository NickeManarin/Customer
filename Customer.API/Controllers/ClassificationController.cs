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
    public class ClassificationController : ControllerBase
    {
        private readonly ILogger<ClassificationController> _logger;
        private readonly IClassificationService _service;

        public ClassificationController(ILogger<ClassificationController> logger, IClassificationService service)
        {
            _logger = logger;
            _service = service;
        }

        // GET: api/v1/<ClassificationController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetClassifications();

            return StatusCode(result.Code, result);
        }
    }
}