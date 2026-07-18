using Microsoft.AspNetCore.Http;

namespace EmployeeAPI.Core.DTOs
{
    public class MasterActCreateDto
    {
        public string ActName { get; set; } = string.Empty;
        public string? Alias { get; set; }
        public DateTime? DateOfEffect { get; set; }
        public string? ActDetails { get; set; }
        public int? LegalCategoryId { get; set; }
        public IFormFile? ActImage { get; set; }
    }
}
