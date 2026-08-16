using EmployeeAPI.Core.Entities;
using EmployeeAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VotersController : ControllerBase
    {
        private readonly IVoterRepository _repository;

        public VotersController(IVoterRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Voter>>> GetAll()
        {
            var voters = await _repository.GetAllAsync();
            return Ok(voters);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Voter>> GetById(int id)
        {
            var voter = await _repository.GetByIdAsync(id);
            if (voter == null)
            {
                return NotFound();
            }

            return Ok(voter);
        }

        [HttpPost]
        public async Task<ActionResult<Voter>> Create([FromBody] CreateVoterRequest request)
        {
            var voter = new Voter
            {
                SerialNo = request.SerialNo,
                EpicNo = request.EpicNo?.Trim() ?? string.Empty,
                Name = request.Name?.Trim() ?? string.Empty,
                EnglishName = request.EnglishName?.Trim(),
                RelativeName = request.RelativeName?.Trim(),
                EnglishRelativeName = request.EnglishRelativeName?.Trim(),
                HouseNo = request.HouseNo?.Trim(),
                Age = request.Age,
                Gender = request.Gender?.Trim(),
                PartNumber = request.PartNumber?.Trim(),
                AreaName = request.AreaName?.Trim(),
                AreaNumber = request.AreaNumber?.Trim()
            };

            var created = await _repository.AddAsync(voter);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<Voter>>> CreateBulk([FromBody] List<CreateVoterRequest> requests)
        {
            if (requests == null || requests.Count == 0)
            {
                return BadRequest("At least one voter is required.");
            }

            var voters = requests.Select(request => new Voter
            {
                SerialNo = request.SerialNo,
                EpicNo = request.EpicNo?.Trim() ?? string.Empty,
                Name = request.Name?.Trim() ?? string.Empty,
                EnglishName = request.EnglishName?.Trim(),
                RelativeName = request.RelativeName?.Trim(),
                EnglishRelativeName = request.EnglishRelativeName?.Trim(),
                HouseNo = request.HouseNo?.Trim(),
                Age = request.Age,
                Gender = request.Gender?.Trim(),
                PartNumber = request.PartNumber?.Trim(),
                AreaName = request.AreaName?.Trim(),
                AreaNumber = request.AreaNumber?.Trim()
            }).ToList();

            var created = await _repository.AddRangeAsync(voters);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVoterRequest request)
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

            existing.SerialNo = request.SerialNo;
            existing.EpicNo = request.EpicNo?.Trim() ?? string.Empty;
            existing.Name = request.Name?.Trim() ?? string.Empty;
            existing.EnglishName = request.EnglishName?.Trim();
            existing.RelativeName = request.RelativeName?.Trim();
            existing.EnglishRelativeName = request.EnglishRelativeName?.Trim();
            existing.HouseNo = request.HouseNo?.Trim();
            existing.Age = request.Age;
            existing.Gender = request.Gender?.Trim();
            existing.PartNumber = request.PartNumber?.Trim();
            existing.AreaName = request.AreaName?.Trim();
            existing.AreaNumber = request.AreaNumber?.Trim();
            existing.UpdatedOn = DateTime.UtcNow;

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
    }

    public class CreateVoterRequest
    {
        public int SerialNo { get; set; }
        public string? EpicNo { get; set; }
        public string? Name { get; set; }
        public string? EnglishName { get; set; }
        public string? RelativeName { get; set; }
        public string? EnglishRelativeName { get; set; }
        public string? HouseNo { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? PartNumber { get; set; }
        public string? AreaName { get; set; }
        public string? AreaNumber { get; set; }
    }

    public class UpdateVoterRequest
    {
        public int SerialNo { get; set; }
        public string? EpicNo { get; set; }
        public string? Name { get; set; }
        public string? EnglishName { get; set; }
        public string? RelativeName { get; set; }
        public string? EnglishRelativeName { get; set; }
        public string? HouseNo { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? PartNumber { get; set; }
        public string? AreaName { get; set; }
        public string? AreaNumber { get; set; }
    }

    public class CreateBulkVoterRequest
    {
        public List<CreateVoterRequest> Voters { get; set; } = new();
    }
}
