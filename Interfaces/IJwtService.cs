using EmployeeAPI.Core.Entities;

namespace EmployeeAPI.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        (bool IsValid, Guid UserId) ValidateToken(string token);
        string GenerateRefreshToken();
    }
}
