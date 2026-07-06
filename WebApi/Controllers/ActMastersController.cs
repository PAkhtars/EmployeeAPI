using EmployeeAPI.Core.Entities;
using EmployeeAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActMastersController : ControllerBase
    {
        private readonly IActMasterRepository _repository;

        public ActMastersController(IActMasterRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActMaster>>> GetActMasters()
        {
            var actMasters = await _repository.GetAllAsync();
            return Ok(actMasters);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActMaster>> GetActMaster(int id)
        {
            var actMaster = await _repository.GetByIdAsync(id);
            if (actMaster == null)
                return NotFound($"ActMaster with id {id} not found.");

            return Ok(actMaster);
        }

        [HttpPost]
        public async Task<ActionResult<ActMaster>> PostActMaster(ActMaster actMaster)
        {
            var createdActMaster = await _repository.AddAsync(actMaster);
            return CreatedAtAction(nameof(GetActMaster), new { id = createdActMaster.ActId }, createdActMaster);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutActMaster(int id, ActMaster actMaster)
        {
            if (id != actMaster.ActId)
                return BadRequest("ActMaster ID mismatch.");

            if (!await _repository.ExistsAsync(id))
                return NotFound();

            await _repository.UpdateAsync(actMaster);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActMaster(int id)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
