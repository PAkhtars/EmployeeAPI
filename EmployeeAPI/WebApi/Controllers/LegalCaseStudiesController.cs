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

        public LegalCaseStudiesController(ILegalCaseStudyRepository repository)
        {
            _repository = repository;
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
        public async Task<ActionResult<LegalCaseStudy>> Create([FromBody] LegalCaseStudy request)
        {
            var created = await _repository.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LegalCaseStudy request)
        {
            if (!await _repository.ExistsAsync(id))
            {
                return NotFound();
            }

            request.Id = id;
            request.UpdatedOn = DateTime.UtcNow;
            await _repository.UpdateAsync(request);
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
    }
}
