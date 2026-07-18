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
    public class ActMastersController : ControllerBase
    {
        private readonly IActMasterRepository _repository;
        private readonly IWebHostEnvironment _environment;

        public ActMastersController(IActMasterRepository repository, IWebHostEnvironment environment)
        {
            _repository = repository;
            _environment = environment;
        }

        [HttpGet]
        [AllowAnonymous]
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
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ActMaster>> PostActMaster([FromForm] MasterActCreateDto request)
        {
            if (string.IsNullOrWhiteSpace(request.ActName))
            {
                return BadRequest("ActName is required.");
            }

            var actMaster = new ActMaster
            {
                ActName = request.ActName.Trim(),
                Alias = request.Alias?.Trim(),
                DateOfEffect = request.DateOfEffect,
                ActDetails = request.ActDetails,
                LegalCategoryId = request.LegalCategoryId,
                ImagePath = await SaveImageAsync(request.ActImage),
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "API"
            };

            var createdActMaster = await _repository.AddAsync(actMaster);
            return CreatedAtAction(nameof(GetActMaster), new { id = createdActMaster.ActId }, createdActMaster);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PutActMaster(int id, [FromForm] ActMaster actMaster)
        {
            if (id != actMaster.ActId)
                return BadRequest("ActMaster ID mismatch.");

            if (!await _repository.ExistsAsync(id))
                return NotFound();

            var existingActMaster = await _repository.GetByIdAsync(id);
            if (existingActMaster == null)
                return NotFound();

            existingActMaster.ActName = actMaster.ActName?.Trim();
            existingActMaster.Alias = actMaster.Alias?.Trim();
            existingActMaster.DateOfEffect = actMaster.DateOfEffect;
            existingActMaster.ActDetails = actMaster.ActDetails;
            existingActMaster.LegalCategoryId = actMaster.LegalCategoryId;
            existingActMaster.ModifiedOn = DateTime.UtcNow;
            existingActMaster.ModifiedBy = "API";

            if (actMaster.ActImage != null)
            {
                existingActMaster.ImagePath = await SaveImageAsync(actMaster.ActImage);
            }

            await _repository.UpdateAsync(existingActMaster);
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

        private async Task<string?> SaveImageAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "act-masters");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid():N}_{Path.GetFileName(file.FileName)}";
            var fullPath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/act-masters/{fileName}";
        }
    }
}
