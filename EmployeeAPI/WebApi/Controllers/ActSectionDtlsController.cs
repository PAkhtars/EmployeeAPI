using EmployeeAPI.Core.Entities;
using EmployeeAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActSectionDtlsController : ControllerBase
    {
        private readonly IActSectionDtlsRepository _repository;

        public ActSectionDtlsController(IActSectionDtlsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ActSectionDtls>>> GetActSectionDtls()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{sectionId}")]
        public async Task<ActionResult<ActSectionDtls>> GetActSectionDtl(int sectionId)
        {
            var item = await _repository.GetByIdAsync(sectionId);
            if (item == null)
                return NotFound($"ActSectionDtls with id {sectionId} not found.");

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ActSectionDtls>> PostActSectionDtl(ActSectionDtls entity)
        {
            var created = await _repository.AddAsync(entity);
            return CreatedAtAction(nameof(GetActSectionDtl), new { sectionId = created.SectionId }, created);
        }

        [HttpPut("{sectionId}")]
        public async Task<IActionResult> PutActSectionDtl(int sectionId, ActSectionDtls entity)
        {
            if (sectionId != entity.SectionId)
                return BadRequest("ActSectionDtls ID mismatch.");

            if (!await _repository.ExistsAsync(sectionId))
                return NotFound();

            await _repository.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{sectionId}")]
        public async Task<IActionResult> DeleteActSectionDtl(int sectionId)
        {
            if (!await _repository.ExistsAsync(sectionId))
                return NotFound();

            await _repository.DeleteAsync(sectionId);
            return NoContent();
        }
    }
}
