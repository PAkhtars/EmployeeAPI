using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.Core.DTOs
{
    public class EmailRequestDto
    {
        [Required]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string ClientEmail { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;
    }
}
