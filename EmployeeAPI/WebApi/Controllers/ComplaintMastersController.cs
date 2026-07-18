using EmployeeAPI.Core.DTOs;
using EmployeeAPI.Core.Entities;
using EmployeeAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComplaintMastersController : ControllerBase
    {
        private readonly IComplaintMasterRepository _repository;

        public ComplaintMastersController(IComplaintMasterRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ComplaintMaster>>> GetComplaintMasters()
        {
            var complaints = await _repository.GetAllAsync();
            return Ok(complaints);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComplaintMaster>> GetComplaintMaster(int id)
        {
            var complaint = await _repository.GetByIdAsync(id);
            if (complaint == null)
                return NotFound($"ComplaintMaster with id {id} not found.");

            return Ok(complaint);
        }

        [HttpPost]
        public async Task<ActionResult<ComplaintMaster>> PostComplaintMaster([FromBody] ComplaintMasterCreateDto request)
        {
            var complaintMaster = new ComplaintMaster
            {
                Name = request.Name?.Trim(),
                Country = request.Country?.Trim(),
                State = request.State?.Trim(),
                District = request.District?.Trim(),
                Tehsil = request.Tehsil?.Trim(),
                Address = request.Address?.Trim(),
                ConcernedDepartment = request.ConcernedDepartment?.Trim(),
                ComplaintDate = request.ComplaintDate?.Trim(),
                Details = request.Details?.Trim(),
                VideoPath = request.VideoPath?.Trim(),
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "API"
            };

            var created = await _repository.AddAsync(complaintMaster);
            return CreatedAtAction(nameof(GetComplaintMaster), new { id = created.ComplaintId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComplaintMaster(int id, [FromBody] ComplaintMaster complaintMaster)
        {
            if (id != complaintMaster.ComplaintId)
                return BadRequest("ComplaintMaster ID mismatch.");

            if (!await _repository.ExistsAsync(id))
                return NotFound();

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = complaintMaster.Name?.Trim();
            existing.Country = complaintMaster.Country?.Trim();
            existing.State = complaintMaster.State?.Trim();
            existing.District = complaintMaster.District?.Trim();
            existing.Tehsil = complaintMaster.Tehsil?.Trim();
            existing.Address = complaintMaster.Address?.Trim();
            existing.ConcernedDepartment = complaintMaster.ConcernedDepartment?.Trim();
            existing.ComplaintDate = complaintMaster.ComplaintDate?.Trim();
            existing.Details = complaintMaster.Details?.Trim();
            existing.VideoPath = complaintMaster.VideoPath?.Trim();
            existing.ModifiedOn = DateTime.UtcNow;
            existing.ModifiedBy = "API";

            await _repository.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComplaintMaster(int id)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
