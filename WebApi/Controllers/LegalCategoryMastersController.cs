using EmployeeAPI.Core.Entities;
using EmployeeAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace EmployeeAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LegalCategoryMastersController : ControllerBase
    {
        private readonly ILegalCategoryMasterRepository _repository;
        private readonly IWebHostEnvironment _environment;

        public LegalCategoryMastersController(ILegalCategoryMasterRepository repository, IWebHostEnvironment environment)
        {
            _repository = repository;
            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LegalCategoryMaster>>> GetAll()
        {
            var categories = await _repository.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LegalCategoryMaster>> GetById(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<LegalCategoryMaster>> Create([FromForm] CreateLegalCategoryRequest request)
        {
            var iconPath = await SaveIconAsync(request.CategoryIcon);

            var category = new LegalCategoryMaster
            {
                CategoryName = request.CategoryName,
                Alias = request.Alias,
                Description = request.Description,
                CategoryIconPath = iconPath,
                IconClass = request.IconClass
            };

            var created = await _repository.AddAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateLegalCategoryRequest request)
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

            existing.CategoryName = request.CategoryName;
            existing.Alias = request.Alias;
            existing.Description = request.Description;
            existing.IconClass = request.IconClass;
            existing.UpdatedOn = DateTime.UtcNow;

            if (request.CategoryIcon != null)
            {
                existing.CategoryIconPath = await SaveIconAsync(request.CategoryIcon);
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

        private async Task<string?> SaveIconAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "legal-category-icons");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid():N}_{Path.GetFileName(file.FileName)}";
            var fullPath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/legal-category-icons/{fileName}";
        }
    }

    public class CreateLegalCategoryRequest
    {
        public string CategoryName { get; set; } = string.Empty;
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public IFormFile? CategoryIcon { get; set; }
        public string? IconClass { get; set; }
    }

    public class UpdateLegalCategoryRequest
    {
        public string CategoryName { get; set; } = string.Empty;
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public IFormFile? CategoryIcon { get; set; }
        public string? IconClass { get; set; }
    }
}
