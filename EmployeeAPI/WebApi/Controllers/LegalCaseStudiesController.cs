using EmployeeAPI.Core.Entities;
using EmployeeAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LegalCaseStudiesController : ControllerBase
    {
        private readonly ILegalCaseStudyRepository _repository;
        private readonly IWebHostEnvironment _environment;

        public LegalCaseStudiesController(ILegalCaseStudyRepository repository, IWebHostEnvironment environment)
        {
            _repository = repository;
            _environment = environment;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<LegalCaseStudy>>> GetAll()
        {
            var cases = await _repository.GetAllAsync();
            return Ok(cases);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LegalCaseStudy>> GetById(int id)
        {
            var legalCaseStudy = await _repository.GetByIdAsync(id);
            if (legalCaseStudy == null)
            {
                return NotFound();
            }

            return Ok(legalCaseStudy);
        }

        [HttpPost]
        public async Task<ActionResult<LegalCaseStudy>> Create([FromForm] CreateLegalCaseStudyRequest request)
        {
            var imagePath = await SaveImageAsync(request.CaseImage);

            var caseStudy = new LegalCaseStudy
            {
                CaseTitleAndCitation = request.CaseTitleAndCitation,
                FactsOfTheCase = request.FactsOfTheCase,
                ProceduralHistory = request.ProceduralHistory,
                LegalIssues = request.LegalIssues,
                ArgumentsOfTheParties = request.ArgumentsOfTheParties,
                RelevantLaw = request.RelevantLaw,
                CourtsAnalysis = request.CourtsAnalysis,
                Judgment_Holding = request.Judgment_Holding,
                CriticalAnalysis = request.CriticalAnalysis,
                ImpactOfTheJudgment = request.ImpactOfTheJudgment,
                Conclusion = request.Conclusion,
                References = request.References,
                CaseName = request.CaseName,
                ImagePath = imagePath
            };

            var created = await _repository.AddAsync(caseStudy);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateLegalCaseStudyRequest request)
        {
            if (!await _repository.ExistsAsync(id))
            {
                return NotFound();
            }

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.CaseTitleAndCitation = request.CaseTitleAndCitation;
            existing.FactsOfTheCase = request.FactsOfTheCase;
            existing.ProceduralHistory = request.ProceduralHistory;
            existing.LegalIssues = request.LegalIssues;
            existing.ArgumentsOfTheParties = request.ArgumentsOfTheParties;
            existing.RelevantLaw = request.RelevantLaw;
            existing.CourtsAnalysis = request.CourtsAnalysis;
            existing.Judgment_Holding = request.Judgment_Holding;
            existing.CriticalAnalysis = request.CriticalAnalysis;
            existing.ImpactOfTheJudgment = request.ImpactOfTheJudgment;
            existing.Conclusion = request.Conclusion;
            existing.References = request.References;
            existing.CaseName = request.CaseName;
            existing.UpdatedOn = DateTime.UtcNow;

            if (request.CaseImage != null)
            {
                existing.ImagePath = await SaveImageAsync(request.CaseImage);
            }

            await _repository.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _repository.ExistsAsync(id))
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<string?> SaveImageAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "legal-case-studies");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid():N}_{Path.GetFileName(file.FileName)}";
            var fullPath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/legal-case-studies/{fileName}";
        }
    }

    public class CreateLegalCaseStudyRequest
    {
        public string CaseTitleAndCitation { get; set; } = string.Empty;
        public string? FactsOfTheCase { get; set; }
        public string? ProceduralHistory { get; set; }
        public string? LegalIssues { get; set; }
        public string? ArgumentsOfTheParties { get; set; }
        public string? RelevantLaw { get; set; }
        public string? CourtsAnalysis { get; set; }
        public string? Judgment_Holding { get; set; }
        public string? CriticalAnalysis { get; set; }
        public string? ImpactOfTheJudgment { get; set; }
        public string? Conclusion { get; set; }
        public string? References { get; set; }
        public string? CaseName { get; set; }
        public IFormFile? CaseImage { get; set; }
    }

    public class UpdateLegalCaseStudyRequest
    {
        public string CaseTitleAndCitation { get; set; } = string.Empty;
        public string? FactsOfTheCase { get; set; }
        public string? ProceduralHistory { get; set; }
        public string? LegalIssues { get; set; }
        public string? ArgumentsOfTheParties { get; set; }
        public string? RelevantLaw { get; set; }
        public string? CourtsAnalysis { get; set; }
        public string? Judgment_Holding { get; set; }
        public string? CriticalAnalysis { get; set; }
        public string? ImpactOfTheJudgment { get; set; }
        public string? Conclusion { get; set; }
        public string? References { get; set; }
        public string? CaseName { get; set; }
        public IFormFile? CaseImage { get; set; }
    }
}
