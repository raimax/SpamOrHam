using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpamOrHam.Models;
using SpamOrHam.Services.Interfaces;
using SpamOrHam.SqlServer;

namespace SpamOrHam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassificationController : ControllerBase
    {
        private readonly IClassificationService _classificationService;
        private readonly DatabaseContext _context;

        public ClassificationController(IClassificationService classificationService, DatabaseContext context)
        {
            _classificationService = classificationService;
            _context = context;
        }

        [HttpPost("classify")]
        public async Task<IActionResult> Classify([FromBody] ClassificationRequest request)
        {
            var result = await _classificationService.Classify(request);

            return Ok(result);
        }

        [HttpGet("dataset")]
        public async Task<IActionResult> GetDataset()
        {
            var dataset = await _context.Datasets.Select(d => new DatasetResponse
            {
                Id = d.Id,
                HamCount = d.HamCount,
                SpamCount = d.SpamCount,
                PriorHamProbability = d.PriorHamProbability,
                PriorSpamProbability = d.PriorSpamProbability,
                DataPointCount = _context.DataPoints.Where(x => x.DatasetId == d.Id).Count()
            })
            .FirstOrDefaultAsync();

            return Ok(dataset);
        }
    }
}
