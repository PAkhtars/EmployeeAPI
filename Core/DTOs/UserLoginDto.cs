namespace EmployeeAPI.Core.DTOs
{
    public class UserLoginDto
    {
        public string EmailOrUsername { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
