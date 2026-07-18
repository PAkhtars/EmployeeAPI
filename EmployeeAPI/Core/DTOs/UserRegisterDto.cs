using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.Core.DTOs
{
    public class UserRegisterDto
    {
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string? FullName { get; set; }
    }

}
