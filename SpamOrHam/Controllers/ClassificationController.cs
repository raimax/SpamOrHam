using Microsoft.AspNetCore.Mvc;
using SpamOrHam.Models;
using SpamOrHam.Services.Interfaces;

namespace SpamOrHam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassificationController : ControllerBase
    {
        private readonly IClassificationService _classificationService;

        public ClassificationController(IClassificationService classificationService)
        {
            _classificationService = classificationService;
        }

        [HttpPost("classify")]
        public IActionResult Classify([FromBody] ClassificationRequest request)
        {
            var result = _classificationService.Classify(request).Result;

            return Ok(result);
        }
    }
}
