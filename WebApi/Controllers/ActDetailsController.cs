using EmployeeAPI.Core.Entities;
using EmployeeAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActDetailsController : ControllerBase
    {
        private readonly IActDetailsRepository _repository;

        public ActDetailsController(IActDetailsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ActDetails>>> GetActDetails()
        {
            var actDetails = await _repository.GetAllAsync();
            return Ok(actDetails);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActDetails>> GetActDetail(int id)
        {
            var actDetail = await _repository.GetByIdAsync(id);
            if (actDetail == null)
                return NotFound($"ActDetails with id {id} not found.");

            return Ok(actDetail);
        }

        [HttpPost]
        public async Task<ActionResult<ActDetails>> PostActDetail(ActDetails actDetails)
        {
            var createdActDetail = await _repository.AddAsync(actDetails);
            return CreatedAtAction(nameof(GetActDetail), new { id = createdActDetail.Id }, createdActDetail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutActDetail(int id, ActDetails actDetails)
        {
            if (id != actDetails.Id)
                return BadRequest("ActDetails ID mismatch.");

            if (!await _repository.ExistsAsync(id))
                return NotFound();

            await _repository.UpdateAsync(actDetails);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActDetail(int id)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
